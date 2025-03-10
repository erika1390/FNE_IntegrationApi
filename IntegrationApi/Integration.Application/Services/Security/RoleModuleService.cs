using AutoMapper;
using Integration.Application.Interfaces.Security;
using Integration.Infrastructure.Interfaces.Security;
using Integration.Shared.DTO.Security;
using Microsoft.Extensions.Logging;

using System.Linq;
using System.Linq.Expressions;
namespace Integration.Application.Services.Security
{
    public class RoleModuleService : IRoleModuleService
    {
        private readonly IRoleModuleRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<RoleModuleService> _logger;
        public RoleModuleService(IRoleModuleRepository repository, IMapper mapper, ILogger<RoleModuleService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<RoleModuleDTO> CreateAsync(RoleModuleDTO roleModuleDTO)
        {
            _logger.LogInformation("Creando RolModuleId: {RolModuleId}", roleModuleDTO.RoleModuleId);
            try
            {
                var roleModule = _mapper.Map<Integration.Core.Entities.Security.RoleModule>(roleModuleDTO);
                var result = await _repository.CreateAsync(roleModule);
                _logger.LogInformation("RolModule creado con éxito: {RolModuleId}", roleModuleDTO.RoleModuleId);
                return _mapper.Map<RoleModuleDTO>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el RolModuleId: {RolModuleId}", roleModuleDTO.RoleModuleId);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation("Eliminando RolModule con ID: {RolModuleId}", id);
            try
            {
                bool success = await _repository.DeleteAsync(id);
                if (success)
                {
                    _logger.LogInformation("RolModule con ID {RolModuleId} eliminada correctamente.", id);
                }
                else
                {
                    _logger.LogWarning("No se encontró el RolModule con ID {RolModuleId} para eliminar.", id);
                }
                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el RolModule con ID {RolModuleId}.", id);
                throw;
            }
        }

        public async Task<IEnumerable<RoleModuleDTO>> GetAllActiveAsync()
        {
            _logger.LogInformation("Obteniendo todos los roleModules.");
            try
            {
                var roleModules = await _repository.GetAllActiveAsync();
                var roleModuleDTO = _mapper.Map<IEnumerable<RoleModuleDTO>>(roleModules);
                _logger.LogInformation("{Count} roleModules obtenidos con éxito.", roleModuleDTO.Count());
                return roleModuleDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los roleModules.");
                throw;
            }
        }

        public async Task<List<RoleModuleDTO>> GetAllAsync(Expression<Func<RoleModuleDTO, bool>> filterDto)
        {
            try
            {
                _logger.LogInformation("Obteniendo todos los roleModules y aplicando el filtro en memoria.");
                var roleModules = await _repository.GetAllAsync(a => true);
                var roleModuleDTOs = _mapper.Map<List<RoleModuleDTO>>(roleModules);
                var filteredApplications = roleModuleDTOs.AsQueryable().Where(filterDto).ToList();
                return filteredApplications;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio al obtener los roleModules.");
                throw;
            }
        }

        public async Task<List<RoleModuleDTO>> GetAllAsync(List<Expression<Func<RoleModuleDTO, bool>>> predicados)
        {
            try
            {
                _logger.LogInformation("Obteniendo todos los roleModules y aplicando múltiples filtros en memoria.");
                var roles = await _repository.GetAllAsync(a => true);
                var rolesDTOs = _mapper.Map<List<RoleModuleDTO>>(roles);
                IQueryable<RoleModuleDTO> query = rolesDTOs.AsQueryable();
                foreach (var predicado in predicados)
                {
                    query = query.Where(predicado);
                }
                return query.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio al obtener los roleModules con múltiples filtros.");
                throw;
            }
        }

        public async Task<RoleModuleDTO> GetByIdAsync(int id)
        {
            _logger.LogInformation("Buscando roleModule con ID: {RoleModuleId}", id);
            try
            {
                var permission = await _repository.GetByIdAsync(id);
                if (permission == null)
                {
                    _logger.LogWarning("No se encontró el roleModule con ID {RoleModuleId}.", id);
                    return null;
                }
                _logger.LogInformation("RoleModule encontrada: {RoleModuleId}", permission.Id);
                return _mapper.Map<RoleModuleDTO>(permission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el roleModule con ID {RoleModuleId}.", id);
                throw;
            }
        }

        public async Task<RoleModuleDTO> UpdateAsync(RoleModuleDTO roleModuleDTO)
        {
            _logger.LogInformation("Actualizando roleModule con ID: {RoleModuleId}", roleModuleDTO.RoleModuleId);
            try
            {
                var roleModule = _mapper.Map<Integration.Core.Entities.Security.RoleModule>(roleModuleDTO);
                var updatedRoleModule = await _repository.UpdateAsync(roleModule);
                if (updatedRoleModule == null)
                {
                    _logger.LogWarning("No se pudo actualizar el roleModule con ID {RoleModuleId}.", roleModuleDTO.RoleModuleId);
                    return null;
                }
                _logger.LogInformation("RoleModule actualizado con éxito: {RoleModuleId}", updatedRoleModule.Id);
                return _mapper.Map<RoleModuleDTO>(updatedRoleModule);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el roleModule con ID {RoleModuleId}.", roleModuleDTO.RoleModuleId);
                throw;
            }
        }
    }
}