using AutoMapper;
using Integration.Application.Interfaces.Security;
using Integration.Infrastructure.Interfaces.Security;
using Integration.Shared.DTO.Security;
using Microsoft.Extensions.Logging;
namespace Integration.Application.Services.Security
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<PermissionService> _logger;
        public PermissionService(IPermissionRepository repository, IMapper mapper, ILogger<PermissionService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<PermissionDTO> CreateAsync(PermissionDTO permissionDTO)
        {
            _logger.LogInformation("Creando permiso: {Name}", permissionDTO.Name);
            try
            {
                var permission = _mapper.Map<Integration.Core.Entities.Security.Permission>(permissionDTO);
                var result = await _repository.CreateAsync(permission);
                _logger.LogInformation("Permiso creado con éxito: {PermissionId}, Nombre: {Name}", result.PermissionId, result.Name);
                return _mapper.Map<PermissionDTO>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el permiso: {Name}", permissionDTO.Name);
                throw;
            }
        }
        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation("Eliminando permiso con ID: {PermissionId}", id);
            try
            {
                bool success = await _repository.DeleteAsync(id);
                if (success)
                {
                    _logger.LogInformation("Permiso con ID {PermissionId} eliminada correctamente.", id);
                }
                else
                {
                    _logger.LogWarning("No se encontró el permiso con ID {PermissionId} para eliminar.", id);
                }
                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el permiso con ID {PermissionId}.", id);
                throw;
            }
        }
        public async Task<IEnumerable<PermissionDTO>> GetAllAsync()
        {
            _logger.LogInformation("Obteniendo todos los permisos.");
            try
            {
                var permission = await _repository.GetAllAsync();
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
        public async Task<PermissionDTO> GetByIdAsync(int id)
        {
            _logger.LogInformation("Buscando permisos con ID: {PermissionId}", id);
            try
            {
                var permission = await _repository.GetByIdAsync(id);
                if (permission == null)
                {
                    _logger.LogWarning("No se encontró el permiso con ID {PermissionId}.", id);
                    return null;
                }
                _logger.LogInformation("Permiso encontrada: {PermissionId}, Nombre: {Name}", permission.PermissionId, permission.Name);
                return _mapper.Map<PermissionDTO>(permission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el permiso con ID {PermissionId}.", id);
                throw;
            }
        }
        public async Task<PermissionDTO> UpdateAsync(PermissionDTO permissionDTO)
        {
            _logger.LogInformation("Actualizando permiso con ID: {PermissionId}, Nombre: {Name}", permissionDTO.PermissionId, permissionDTO.Name);
            try
            {
                var permission = _mapper.Map<Integration.Core.Entities.Security.Permission>(permissionDTO);
                var updatedPermission = await _repository.UpdateAsync(permission);
                if (updatedPermission == null)
                {
                    _logger.LogWarning("No se pudo actualizar el permiso con ID {PermissionId}.", permissionDTO.PermissionId);
                    return null;
                }
                _logger.LogInformation("Permiso actualizado con éxito: {PermissionId}, Nombre: {Name}", updatedPermission.PermissionId, updatedPermission.Name);
                return _mapper.Map<PermissionDTO>(updatedPermission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el permiso con ID {permissionDTO}.", permissionDTO.PermissionId);
                throw;
            }
        }
    }
}