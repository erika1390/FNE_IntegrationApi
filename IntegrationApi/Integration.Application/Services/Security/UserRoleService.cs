using AutoMapper;

using Integration.Application.Interfaces.Security;
using Integration.Core.Entities.Security;
using Integration.Infrastructure.Interfaces.Security;
using Integration.Shared.DTO.Header;
using Integration.Shared.DTO.Security;

using Microsoft.Extensions.Logging;

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
                var user = await _userRepository.GetByCodeAsync(header.UserCode);
                if (user == null)
                {
                    throw new Exception($"No se encontró el UserRole con código UserCode: {userRoleDTO.UserCode}, RoleCode: {userRoleDTO.RoleCode}.");
                }
                var userRole = _mapper.Map<Integration.Core.Entities.Security.UserRole>(userRoleDTO);
                userRole.CreatedBy = user.UserName;
                userRole.UpdatedBy = user.UserName;
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

        public async Task<bool> DeactivateAsync(HeaderDTO header, string userCode, string roleCode, string userName)
        {
            _logger.LogInformation("Desactivando aplicación con UserRole: UserCode: {UserCode}, RoleCode: {RoleCode}", userCode, roleCode);
            try
            {
                var user = await _userRepository.GetByCodeAsync(header.UserCode);
                if (user == null)
                {
                    throw new Exception($"No se encontró el UserRole con UserCode {header.UserCode}, RoleCode {roleCode}.");
                }
                var role = await _roleRepository.GetByCodeAsync(roleCode);
                bool success = await _userRoleRepository.DeactivateAsync(user.Id, role.Id, user.UserName);
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

        public async Task<List<UserRoleDTO>> GetAllAsync(Expression<Func<UserRoleDTO, bool>> predicate)
        {
            try
            {
                _logger.LogInformation("Obteniendo todos los UserRoles y aplicando el filtro.");
                Expression<Func<UserRole, bool>> entityPredicate = x => true;
                if (predicate != null)
                {
                    var compiledPredicate = predicate.Compile();
                    entityPredicate = u => compiledPredicate(_mapper.Map<UserRoleDTO>(u));
                }
                var userRoles = await _userRoleRepository.GetAllAsync(entityPredicate);
                return _mapper.Map<List<UserRoleDTO>>(userRoles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio al obtener UserRoles.");
                throw;
            }
        }

        public async Task<List<UserRoleDTO>> GetAllAsync(List<Expression<Func<UserRoleDTO, bool>>> predicates)
        {
            try
            {
                _logger.LogInformation("Obteniendo todos los UserRoles y aplicando múltiples filtros.");
                List<Expression<Func<UserRole, bool>>> entityPredicates = predicates
                    .Select(predicate => (Expression<Func<UserRole, bool>>)(u => predicate.Compile()(_mapper.Map<UserRoleDTO>(u))))
                    .ToList();
                var userRoles = await _userRoleRepository.GetAllAsync(entityPredicates);
                return _mapper.Map<List<UserRoleDTO>>(userRoles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio al obtener los UserRoles con múltiples filtros.");
                throw;
            }
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
                var user = await _userRepository.GetByCodeAsync(header.UserCode);
                if (user == null)
                {
                    throw new Exception($"No se encontró el UserRole con código UserCode: {userRoleDTO.UserCode}, RoleCode: {userRoleDTO.RoleCode}.");
                }
                var role = await _roleRepository.GetByCodeAsync(userRoleDTO.RoleCode);
                var userRoleExist = await _userRoleRepository.GetByUserIdRoleIdAsync(user.Id, role.Id);
                var userRole = _mapper.Map<Integration.Core.Entities.Security.UserRole>(userRoleDTO);
                userRole.RoleId = role.Id;
                userRole.UserId = user.Id;
                userRole.Id = userRoleExist.Id;
                userRole.UpdatedBy = user.UserName;
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