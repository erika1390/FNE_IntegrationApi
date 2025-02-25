using AutoMapper;
using Integration.Application.Interfaces.Security;
using Integration.Infrastructure.Interfaces.Security;
using Integration.Shared.DTO.Security;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
namespace Integration.Application.Services.Security
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<RoleService> _logger;
        public RoleService(IRoleRepository repository, IMapper mapper, ILogger<RoleService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<RoleDTO> CreateAsync(RoleDTO roleDTO)
        {
            _logger.LogInformation("Creando rol: {Name}", roleDTO.Name);
            try
            {
                var role = _mapper.Map<Integration.Core.Entities.Security.Role>(roleDTO);
                var result = await _repository.CreateAsync(role);
                _logger.LogInformation("Rol creado con éxito: {RoleId}, Nombre: {Name}", result.Id, result.Name);
                return _mapper.Map<RoleDTO>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el rol: {Name}", roleDTO.Name);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation("Eliminando rol con ID: {RoleId}", id);
            try
            {
                bool success = await _repository.DeleteAsync(id);
                if (success)
                {
                    _logger.LogInformation("Rol con ID {RoleId} eliminada correctamente.", id);
                }
                else
                {
                    _logger.LogWarning("No se encontró el rol con ID {RoleId} para eliminar.", id);
                }
                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el rol con ID {RoleId}.", id);
                throw;
            }
        }

        public async Task<IEnumerable<RoleDTO>> GetAllActiveAsync()
        {
            _logger.LogInformation("Obteniendo todos los roles.");
            try
            {
                var roles = await _repository.GetAllActiveAsync();
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
            throw new NotImplementedException();
        }

        public async Task<List<RoleDTO>> GetAllAsync(List<Expression<Func<RoleDTO, bool>>> predicados)
        {
            throw new NotImplementedException();
        }

        public async Task<RoleDTO> GetByIdAsync(int id)
        {
            _logger.LogInformation("Buscando rol con ID: {RoleId}", id);
            try
            {
                var permission = await _repository.GetByIdAsync(id);
                if (permission == null)
                {
                    _logger.LogWarning("No se encontró el rol con ID {RoleId}.", id);
                    return null;
                }
                _logger.LogInformation("Rol encontrada: {RoleId}, Nombre: {Name}", permission.Id, permission.Name);
                return _mapper.Map<RoleDTO>(permission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el rol con ID {RoleId}.", id);
                throw;
            }
        }

        public async Task<RoleDTO> UpdateAsync(RoleDTO roleDTO)
        {
            _logger.LogInformation("Actualizando rol con ID: {RoleId}, Nombre: {Name}", roleDTO.RoleId, roleDTO.Name);
            try
            {
                var role = _mapper.Map<Integration.Core.Entities.Security.Role>(roleDTO);
                var updatedRole = await _repository.UpdateAsync(role);
                if (updatedRole == null)
                {
                    _logger.LogWarning("No se pudo actualizar el rol con ID {RoleId}.", roleDTO.RoleId);
                    return null;
                }
                _logger.LogInformation("Rol actualizado con éxito: {RoleId}, Nombre: {Name}", updatedRole.Id, updatedRole.Name);
                return _mapper.Map<RoleDTO>(updatedRole);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el rol con ID {permissionDTO}.", roleDTO.RoleId);
                throw;
            }
        }
    }
}
