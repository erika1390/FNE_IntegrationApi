using AutoMapper;

using Integration.Application.Interfaces.Security;
using Integration.Infrastructure.Interfaces.Security;
using Integration.Shared.DTO.Header;
using Integration.Shared.DTO.Security;

using Microsoft.Extensions.Logging;

using System.Data;
using System.Linq.Expressions;
namespace Integration.Application.Services.Security
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<PermissionService> _logger;
        private readonly IUserRepository _userRepository;
        public PermissionService(IPermissionRepository repository, IMapper mapper, ILogger<PermissionService> logger, IUserRepository userRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _userRepository = userRepository;
        }

        public async Task<PermissionDTO> CreateAsync(HeaderDTO header, PermissionDTO permissionDTO)
        {
            if (header == null || string.IsNullOrEmpty(header.UserCode))
            {
                throw new ArgumentNullException(nameof(header), "El encabezado o UserCode no pueden ser nulos.");
            }
            _logger.LogInformation("Creando permiso: {Name}", permissionDTO.Name);
            try
            {
                var user = await _userRepository.GetByCodeAsync(header.UserCode);
                if (user == null)
                {
                    throw new Exception($"No se encontró el usuario con código {header.UserCode}.");
                }
                var permission = _mapper.Map<Integration.Core.Entities.Security.Permission>(permissionDTO);
                permission.CreatedBy = user.UserName;
                permission.UpdatedBy = user.UserName;
                var result = await _repository.CreateAsync(permission);
                _logger.LogInformation("Permiso creado con éxito: {PermissionId}, Nombre: {Name}", result.Id, result.Name);
                return _mapper.Map<PermissionDTO>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el permiso: {Name}", permissionDTO.Name);
                throw;
            }
        }

        public async Task<bool> DeactivateAsync(HeaderDTO header, string code)
        {
            _logger.LogInformation("Desactivar permiso con codigo: {PermissionCodigo}", code);
            try
            {
                var user = await _userRepository.GetByCodeAsync(header.UserCode);
                if (user == null)
                {
                    throw new Exception($"No se encontró el usuario con código {header.UserCode}.");
                }
                bool success = await _repository.DeactivateAsync(code, user.UserName);
                if (success)
                {
                    _logger.LogInformation("Permiso con codigo {PermissionCodigo} desactivar correctamente.", code);
                }
                else
                {
                    _logger.LogWarning("No se encontró el permiso con codigo {PermissionCodigo} para desactivar.", code);
                }
                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al desactivar el permiso con codigo {PermissionCodigo}.", code);
                throw;
            }
        }

        public async Task<IEnumerable<PermissionDTO>> GetAllActiveAsync()
        {
            _logger.LogInformation("Obteniendo todos los permisos.");
            try
            {
                var permission = await _repository.GetAllActiveAsync();
                var permissionDTO = _mapper.Map<IEnumerable<PermissionDTO>>(permission);
                _logger.LogInformation("{Count} permisos obtenidas con éxito.", permissionDTO.Count());
                return permissionDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los permisos.");
                throw;
            }
        }

        public async Task<List<PermissionDTO>> GetAllAsync(Expression<Func<PermissionDTO, bool>> predicate)
        {
            try
            {
                _logger.LogInformation("Obteniendo todos los permisos y aplicando el filtro en memoria.");
                var permissions = await _repository.GetAllAsync(a => true);
                var permissionsDTOs = _mapper.Map<List<PermissionDTO>>(permissions);
                var filteredPermissions = permissionsDTOs.AsQueryable().Where(predicate).ToList();
                return filteredPermissions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio al obtener los permisos.");
                throw;
            }
        }

        public async Task<List<PermissionDTO>> GetAllAsync(List<Expression<Func<PermissionDTO, bool>>> predicates)
        {
            try
            {
                _logger.LogInformation("Obteniendo todos los permisos y aplicando múltiples filtros en memoria.");
                var permissions = await _repository.GetAllAsync(a => true);
                var permissionDTOs = _mapper.Map<List<PermissionDTO>>(permissions);
                IQueryable<PermissionDTO> query = permissionDTOs.AsQueryable();
                foreach (var predicado in predicates)
                {
                    query = query.Where(predicado);
                }
                return query.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio al obtener los permisos con múltiples filtros.");
                throw;
            }
        }

        public async Task<PermissionDTO> GetByCodeAsync(string code)
        {
            _logger.LogInformation("Buscando pemiso con PermissionCode: {PermissionCode}", code);
            try
            {
                var permission = await _repository.GetByCodeAsync(code);
                if (permission == null)
                {
                    _logger.LogWarning("No se encontró el permiso con PermissionCode {PermissionCode}.", code);
                    return null;
                }
                _logger.LogInformation("Permiso encontrada: PermissionCode: {PermissionCode}, Nombre: {Name}", permission.Code, permission.Name);
                return _mapper.Map<PermissionDTO>(permission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el permiso con PermissionCode {PermissionCode}.", code);
                throw;
            }
        }

        public async Task<PermissionDTO> UpdateAsync(HeaderDTO header, PermissionDTO permissionDTO)
        {
            _logger.LogInformation("Actualizando permiso con PermissionCode: {PermissionCode}, Nombre: {Name}", permissionDTO.Code, permissionDTO.Name);
            try
            {
                var user = await _userRepository.GetByCodeAsync(header.UserCode);
                if (user == null)
                {
                    throw new Exception($"No se encontró el usuario con código {header.UserCode}.");
                }
                var permission = _mapper.Map<Integration.Core.Entities.Security.Permission>(permissionDTO);
                permission.UpdatedBy = user.UserName;
                var updatedPermission = await _repository.UpdateAsync(permission);
                if (updatedPermission == null)
                {
                    _logger.LogWarning("No se pudo actualizar el permiso con PermissionCode {PermissionCode}.", permissionDTO.Code);
                    return null;
                }
                _logger.LogInformation("Permiso actualizada con éxito: {PermissionCode}, Nombre: {Name}", updatedPermission.Code, updatedPermission.Name);
                return _mapper.Map<PermissionDTO>(updatedPermission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el permiso con PermissionCode {PermissionCode}.", permissionDTO.Code);
                throw new Exception("Error al actualizar el permiso", ex);
            }
        }
    }
}