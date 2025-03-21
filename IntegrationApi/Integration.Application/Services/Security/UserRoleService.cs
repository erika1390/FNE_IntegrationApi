using AutoMapper;

using Integration.Application.Interfaces.Security;
using Integration.Core.Entities.Security;
using Integration.Infrastructure.Interfaces.Security;
using Integration.Infrastructure.Repositories.Security;
using Integration.Shared.DTO.Header;
using Integration.Shared.DTO.Security;

using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;

using System.Linq;
using System.Linq.Expressions;

namespace Integration.Application.Services.Security
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UserRoleService> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public UserRoleService(IUserRoleRepository userRoleRepository, IMapper mapper, ILogger<UserRoleService> logger, IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRoleRepository = userRoleRepository;
            _mapper = mapper;
            _logger = logger;
            _userRepository = userRepository;
            _roleRepository = roleRepository;            
        }
        public async Task<UserRoleDTO> CreateAsync(HeaderDTO header, UserRoleDTO userRoleDTO)
        {
            if (header == null || string.IsNullOrEmpty(header.UserCode))
            {
                throw new ArgumentNullException(nameof(header), "El encabezado o UserCode no pueden ser nulos.");
            }
            _logger.LogInformation("Creando UserRole: UserCode: {UserCode}, RoleCode: {RoleCode}", userRoleDTO.UserCode, userRoleDTO.RoleCode);
            try
            {
                var userHeader = await _userRepository.GetByCodeAsync(header.UserCode);
                if (userHeader == null)
                {
                    throw new Exception($"No se encontró el usuario con código {header.UserCode}.");
                }
                var userBody = await _userRepository.GetByCodeAsync(userRoleDTO.UserCode);
                var roleBody = await _roleRepository.GetByCodeAsync(userRoleDTO.RoleCode);
                var userRole = _mapper.Map<Integration.Core.Entities.Security.UserRole>(userRoleDTO);
                userRole.UserId = userBody.Id;
                userRole.RoleId = roleBody.Id;
                userRole.CreatedBy = userHeader.UserName;
                userRole.UpdatedBy = userHeader.UserName;
                var result = await _userRoleRepository.CreateAsync(userRole);
                _logger.LogInformation("UserRole creado con éxito: UserCode: {UserCode}, RoleCode: {RoleCode}", userRoleDTO.UserCode, userRoleDTO.RoleCode);
                return _mapper.Map<UserRoleDTO>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el UserRole: UserCode: {UserCode}, RoleCode: {RoleCode}", userRoleDTO.UserCode, userRoleDTO.RoleCode);
                throw;
            }
        }

        public async Task<bool> DeactivateAsync(HeaderDTO header, string userCode, string roleCode)
        {
            _logger.LogInformation("Desactivando aplicación con UserRole: UserCode: {UserCode}, RoleCode: {RoleCode}", userCode, roleCode);
            try
            {
                var userHeader = await _userRepository.GetByCodeAsync(header.UserCode);
                if (userHeader == null)
                {
                    throw new Exception($"No se encontró el usuario con código {header.UserCode}.");
                }
                var userBody = await _userRepository.GetByCodeAsync(userCode);
                var roleBody = await _roleRepository.GetByCodeAsync(roleCode);
                bool success = await _userRoleRepository.DeactivateAsync(userBody.Id, roleBody.Id, userHeader.UserName);
                if (success)
                {
                    _logger.LogInformation("UserRole con UserCode {UserCode}, RoleCode {RoleCode} desactivada correctamente.", userCode, roleCode);
                }
                else
                {
                    _logger.LogWarning("No se encontró el UserRole con UserCode {UserCode}, RoleCode {RoleCode} para desactivar.", userCode, roleCode);
                }
                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el UserRole con UserCode {UserCode}, RoleCode {RoleCode}.", userCode, roleCode);
                throw;
            }
        }

        public async Task<IEnumerable<UserRoleDTO>> GetAllActiveAsync()
        {
            _logger.LogInformation("Obteniendo todos los UserRoles.");
            try
            {
                var userRole = await _userRoleRepository.GetAllActiveAsync();
                var userRoleDTOs = _mapper.Map<IEnumerable<UserRoleDTO>>(userRole);
                _logger.LogInformation("{Count} UserRoles obtenidas con éxito.", userRoleDTOs.Count());
                return userRoleDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los UserRoles.");
                throw;
            }
        }

        public async Task<List<UserRoleDTO>> GetByFilterAsync(Expression<Func<UserRoleDTO, bool>> predicate)
        {
            try
            {
                _logger.LogInformation("Obteniendo usuarops por rol y aplicando los filtros.");

                // Inicializar filtro base
                Expression<Func<UserRole, bool>> userRoleFilter = a => true;

                // Obtener filtros de RoleCode y UserCode
                string roleCode = null, userCode = null;

                if (predicate != null)
                {
                    roleCode = GetFilterValue(predicate, "RoleCode");
                    userCode = GetFilterValue(predicate, "UserCode");
                }

                // Aplicar filtro de RoleCode
                if (!string.IsNullOrEmpty(roleCode))
                {
                    var role = await _roleRepository.GetByCodeAsync(roleCode);
                    if (role == null) return new List<UserRoleDTO>();
                    userRoleFilter = a => a.RoleId == role.Id;
                }

                // Aplicar filtro de UserCode
                if (!string.IsNullOrEmpty(userCode))
                {
                    var user = await _userRepository.GetByCodeAsync(userCode);
                    if (user == null) return new List<UserRoleDTO>();
                    userRoleFilter = a => a.UserId == user.Id;
                }

                // Obtener los permisos filtrados desde la base de datos
                var userRoles = await _userRoleRepository.GetAllAsync(userRoleFilter);
                var userRoleDTOs = _mapper.Map<List<UserRoleDTO>>(userRoles);

                // Aplicar otros filtros en memoria si es necesario
                if (predicate != null && string.IsNullOrEmpty(roleCode) && string.IsNullOrEmpty(userCode))
                {
                    userRoleDTOs = userRoleDTOs.AsQueryable().Where(predicate).ToList();
                }
                return userRoleDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio al obtener los permisos.");
                throw;
            }
        }

        private string GetFilterValue(Expression<Func<UserRoleDTO, bool>> predicate, string propertyName)
        {
            if (predicate.Body is BinaryExpression binaryExp &&
            binaryExp.Left is MemberExpression member &&
                member.Member.Name == propertyName &&
                binaryExp.Right is ConstantExpression constant)
            {
                return constant.Value?.ToString();
            }
            return null;
        }

        public async Task<List<UserRoleDTO>> GetByMultipleFiltersAsync(List<Expression<Func<UserRoleDTO, bool>>> predicates)
        {
            try
            {
                _logger.LogInformation("Obteniendo usuarios por rol con múltiples filtros.");

                // Filtro base para la base de datos
                Expression<Func<UserRole, bool>> userRoleFilter = a => true;

                // Aplicar filtros en la base de datos si existen
                foreach (var predicate in predicates ?? new List<Expression<Func<UserRoleDTO, bool>>>())
                {
                    if (TryGetFilterValue(predicate, "RoleCode", out string roleCode))
                    {
                        var role = await _roleRepository.GetByCodeAsync(roleCode);
                        if (role == null) return new List<UserRoleDTO>();
                        userRoleFilter = a => a.RoleId == role.Id;
                    }
                    if (TryGetFilterValue(predicate, "UserCode", out string userCode))
                    {
                        var user = await _userRepository.GetByCodeAsync(userCode);
                        if (user == null) return new List<UserRoleDTO>();
                        userRoleFilter = a => a.UserId == user.Id;
                    }
                }

                // Obtener datos de la base de datos con los filtros aplicados
                var userRole = await _userRoleRepository.GetAllAsync(userRoleFilter);
                var userRoleDTOs = _mapper.Map<List<UserRoleDTO>>(userRole);

                // Aplicar los filtros en memoria si aún quedan predicados
                if (predicates != null && predicates.Any(p => !IsFilteringByCode(p)))
                {
                    userRoleDTOs = userRoleDTOs.AsQueryable()
                        .Where(p => predicates.All(predicate => predicate.Compile()(p)))
                        .ToList();
                }

                return userRoleDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio al obtener los usuarios por rol.");
                throw;
            }
        }

        private bool IsFilteringByCode(Expression<Func<UserRoleDTO, bool>> predicate)
        {
            return TryGetFilterValue(predicate, "RoleCode", out _) ||
                   TryGetFilterValue(predicate, "UserCode", out _);
        }

        private bool TryGetFilterValue(Expression<Func<UserRoleDTO, bool>> predicate, string propertyName, out string value)
        {
            value = null;

            if (predicate.Body is BinaryExpression binaryExp &&
                binaryExp.Left is MemberExpression member &&
                member.Member.Name == propertyName &&
                binaryExp.Right is ConstantExpression constant)
            {
                value = constant.Value?.ToString();
                return !string.IsNullOrEmpty(value);
            }

            return false;
        }

        public async Task<UserRoleDTO> GetByUserCodeRoleCodeAsync(string userCode, string roleCode)
        {
            _logger.LogInformation("Buscando aplicación con UserCode: {UserCode}, RoleCode: {RoleCode}", userCode,roleCode);
            try
            {
                var user = await _userRepository.GetByCodeAsync(userCode);
                var role = await _roleRepository.GetByCodeAsync(roleCode);
                var userRole = await _userRoleRepository.GetByUserIdRoleIdAsync(user.Id, role.Id);
                if (userRole == null)
                {
                    _logger.LogWarning("No se encontró la UserRole con UserCode: {UserCode}, RoleCode: {RoleCode}.", userCode, roleCode);
                    return null;
                }
                _logger.LogInformation("UserRole encontrada: UserCode: {UserCode}, RoleCode: {RoleCode}.", userCode, roleCode);
                return _mapper.Map<UserRoleDTO>(userRole);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el UserRole con UserCode: {UserCode}, RoleCode: {RoleCode}.", userCode, roleCode);
                throw;
            }
        }

        public async Task<UserRoleDTO> UpdateAsync(HeaderDTO header, UserRoleDTO userRoleDTO)
        {
            _logger.LogInformation("UserRole creado exitosamente: UserCode: {UserCode}, RoleCode: {RoleCode}.", userRoleDTO.UserCode, userRoleDTO.RoleCode);
            try
            {
                var userHeader = await _userRepository.GetByCodeAsync(header.UserCode);
                if (userHeader == null)
                {
                    throw new Exception($"No se encontró el usuario con código {header.UserCode}.");
                }
                var userBody = await _userRepository.GetByCodeAsync(userRoleDTO.UserCode);
                var roleBody = await _roleRepository.GetByCodeAsync(userRoleDTO.RoleCode);
                var userRoleExist = await _userRoleRepository.GetByUserIdRoleIdAsync(userBody.Id, roleBody.Id);
                var userRole = _mapper.Map<Integration.Core.Entities.Security.UserRole>(userRoleDTO);
                userRole.UserId = userBody.Id; 
                userRole.RoleId = roleBody.Id;                
                userRole.Id = userRoleExist.Id;
                userRole.UpdatedBy = userHeader.UserName;
                var updatedUserRole = await _userRoleRepository.UpdateAsync(userRole);
                if (updatedUserRole == null)
                {
                    _logger.LogWarning("UserRole con UserCode: {UserCode}, RoleCode: {RoleCode}.", userRoleDTO.UserCode, userRoleDTO.RoleCode);
                    return null;
                }
                _logger.LogInformation("UserRole actualizado con éxito: UserCode {UserCode}, RoleCode: {RoleCode}.", userRoleDTO.UserCode, userRoleDTO.RoleCode);
                return _mapper.Map<UserRoleDTO>(updatedUserRole);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar UserRole con UserCode {UserCode}, RoleCode: {RoleCode}.", userRoleDTO.UserCode, userRoleDTO.RoleCode);
                throw;
            }
        }
    }
}