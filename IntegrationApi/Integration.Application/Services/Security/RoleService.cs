using AutoMapper;

using Integration.Application.Interfaces.Security;
using Integration.Core.Entities.Security;
using Integration.Infrastructure.Interfaces.Security;
using Integration.Infrastructure.Repositories.Security;
using Integration.Shared.DTO.Header;
using Integration.Shared.DTO.Security;

using Microsoft.Extensions.Logging;

using System.Linq.Expressions;
using System.Security;
namespace Integration.Application.Services.Security
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RoleService> _logger;
        private readonly IApplicationRepository _applicationRepository;
        private readonly IUserRepository _userRepository;
        public RoleService(IRoleRepository roleRepository, IMapper mapper, ILogger<RoleService> logger, IApplicationRepository applicationRepository, IUserRepository userRepository)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
            _logger = logger;
            _applicationRepository = applicationRepository;
            _userRepository = userRepository;
        }
        public async Task<RoleDTO> CreateAsync(HeaderDTO header, RoleDTO roleDTO)
        {
            if (header == null || string.IsNullOrEmpty(header.UserCode))
            {
                throw new ArgumentNullException(nameof(header), "El encabezado o UserCode no pueden ser nulos.");
            }
            _logger.LogInformation("Creando rol: {Name}", roleDTO.Name);
            try
            {
                var user = await _userRepository.GetByCodeAsync(header.UserCode);
                if (user == null)
                {
                    throw new Exception($"No se encontró el rol con código {header.UserCode}.");
                }
                var application = await _applicationRepository.GetByCodeAsync(header.ApplicationCode);
                var role = _mapper.Map<Integration.Core.Entities.Security.Role>(roleDTO);
                role.ApplicationId = application.Id;
                role.CreatedBy = user.UserName;
                role.UpdatedBy = user.UserName;
                role.NormalizedName = roleDTO.Name.ToUpper();
                var result = await _roleRepository.CreateAsync(role);
                _logger.LogInformation("Rol creado con éxito: RoleCode {RoleCode}, Nombre: {Name}", result.Code, result.Name);
                return _mapper.Map<RoleDTO>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el rol: {Name}", roleDTO.Name);
                throw;
            }
        }
        public async Task<bool> DeactivateAsync(HeaderDTO header, string code)
        {
            _logger.LogInformation("Eliminando rol con RoleCode: {RoleCode}", code);
            try
            {
                var user = await _userRepository.GetByCodeAsync(header.UserCode);
                if (user == null)
                {
                    throw new Exception($"No se encontró el usuario con código {header.UserCode}.");
                }
                bool success = await _roleRepository.DeactivateAsync(code, user.UserName);
                if (success)
                {
                    _logger.LogInformation("Rol con RoleCode {RoleCode} eliminada correctamente.", code);
                }
                else
                {
                    _logger.LogWarning("No se encontró el rol con RoleCode {RoleCode} para eliminar.", code);
                }
                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el rol con RoleCode {RoleCode}.", code);
                throw;
            }
        }

        public async Task<IEnumerable<RoleDTO>> GetAllActiveAsync()
        {
            _logger.LogInformation("Obteniendo todos los roles.");
            try
            {
                var roles = await _roleRepository.GetAllActiveAsync();
                var roleDTO = _mapper.Map<IEnumerable<RoleDTO>>(roles);
                _logger.LogInformation("{Count} roles obtenidos con éxito.", roleDTO.Count());
                return roleDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los roles.");
                throw;
            }
        }

        public async Task<List<RoleDTO>> GetAllAsync(Expression<Func<RoleDTO, bool>> predicado)
        {
            try
            {
                _logger.LogInformation("Obteniendo todos los roles y aplicando el filtro.");
                int? applicationId = null;
                Expression<Func<Integration.Core.Entities.Security.Role, bool>> roleFilter = a => true;
                if (predicado != null && IsFilteringByApplicationCode(predicado, out string applicationCode))
                {
                    _logger.LogInformation("Buscando ID de la aplicación con código: {ApplicationCode}", applicationCode);
                    var application = await _applicationRepository.GetByCodeAsync(applicationCode);

                    if (application == null)
                    {
                        _logger.LogWarning("No se encontró la aplicación con código: {ApplicationCode}", applicationCode);
                        return new List<RoleDTO>();
                    }
                    applicationId = application.Id;
                    roleFilter = a => a.ApplicationId == applicationId.Value;
                }
                var modules = await _roleRepository.GetAllAsync(roleFilter);
                var modulesDTOs = _mapper.Map<List<RoleDTO>>(modules);
                if (predicado != null && !IsFilteringByApplicationCode(predicado, out _))
                {
                    modulesDTOs = modulesDTOs.AsQueryable().Where(predicado).ToList();
                }
                return modulesDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio al obtener roles.");
                throw;
            }
        }
        private bool IsFilteringByApplicationCode(Expression<Func<RoleDTO, bool>> predicado, out string applicationCode)
        {
            applicationCode = null;

            if (predicado.Body is BinaryExpression binaryExp)
            {
                if (binaryExp.Left is MemberExpression member && member.Member.Name == "ApplicationCode" &&
                    binaryExp.Right is ConstantExpression constant)
                {
                    applicationCode = constant.Value?.ToString();
                    return !string.IsNullOrEmpty(applicationCode);
                }
            }

            return false;
        }
        public async Task<List<RoleDTO>> GetAllAsync(List<Expression<Func<RoleDTO, bool>>> predicados)
        {
            try
            {
                _logger.LogInformation("Obteniendo todos los roles y aplicando múltiples filtros.");

                int? applicationId = null;
                string applicationCode = null;
                List<Expression<Func<RoleDTO, bool>>> otherFilters = new List<Expression<Func<RoleDTO, bool>>>();
                foreach (var predicado in predicados)
                {
                    if (IsFilteringByApplicationCode(predicado, out string extractedCode))
                    {
                        if (string.IsNullOrEmpty(applicationCode))
                        {
                            applicationCode = extractedCode;
                        }
                    }
                    else
                    {
                        otherFilters.Add(predicado);
                    }
                }
                if (!string.IsNullOrEmpty(applicationCode))
                {
                    _logger.LogInformation("Buscando ID de la aplicación con código: {ApplicationCode}", applicationCode);
                    var application = await _applicationRepository.GetByCodeAsync(applicationCode);
                    if (application == null)
                    {
                        _logger.LogWarning("No se encontró la aplicación con código: {ApplicationCode}", applicationCode);
                        return new List<RoleDTO>();
                    }
                    applicationId = application.Id;
                }
                Expression<Func<Integration.Core.Entities.Security.Role, bool>> roleFilter = a => true;
                if (applicationId.HasValue)
                {
                    roleFilter = a => a.ApplicationId == applicationId.Value;
                }
                var roles = await _roleRepository.GetAllAsync(roleFilter);
                var roleDTOs = _mapper.Map<List<RoleDTO>>(roles);
                foreach (var filter in otherFilters)
                {
                    roleDTOs = roleDTOs.Where(filter.Compile()).ToList();
                }
                return roleDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio al obtener los módulos con múltiples filtros.");
                throw;
            }
        }

        public async Task<RoleDTO> GetByCodeAsync(string code)
        {
            _logger.LogInformation("Buscando rol con RoleCode: {RoleCode}", code);
            try
            {
                var permission = await _roleRepository.GetByCodeAsync(code);
                if (permission == null)
                {
                    _logger.LogWarning("No se encontró el rol con RoleCode {RoleCode}.", code);
                    return null;
                }
                _logger.LogInformation("Rol encontrada: RoleCode: {RoleCode}, Nombre: {Name}", permission.Code, permission.Name);
                return _mapper.Map<RoleDTO>(permission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el rol con RoleCode {RoleCode}.", code);
                throw;
            }
        }

        public async Task<RoleDTO> UpdateAsync(HeaderDTO header, RoleDTO roleDTO)
        {
            _logger.LogInformation("Actualizando rol con RoleCode: {RoleCode}, Nombre: {Name}", roleDTO.Code, roleDTO.Name);
            try
            {
                var user = await _userRepository.GetByCodeAsync(header.UserCode);
                if (user == null)
                {
                    throw new Exception($"No se encontró el usuario con código {header.UserCode}.");
                }
                var application = await _applicationRepository.GetByCodeAsync(header.ApplicationCode);
                var role = _mapper.Map<Integration.Core.Entities.Security.Role>(roleDTO);
                role.ApplicationId = application.Id;
                role.UpdatedBy = user.UserName;
                var updatedRole = await _roleRepository.UpdateAsync(role);
                if (updatedRole == null)
                {
                    _logger.LogWarning("No se pudo actualizar el rol con RoleCode {RoleCode}.", roleDTO.Code);
                    return null;
                }
                _logger.LogInformation("Rol actualizado con éxito: {RoleCode}, Nombre: {Name}", updatedRole.Code, updatedRole.Name);
                return _mapper.Map<RoleDTO>(updatedRole);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el rol con RoleCode {RoleCode}.", roleDTO.Code);
                throw;
            }
        }
    }
}
