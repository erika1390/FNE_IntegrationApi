using AutoMapper;
using Integration.Application.Interfaces.Security;
using Integration.Infrastructure.Interfaces.Security;
using Integration.Shared.DTO.Security;
using Microsoft.Extensions.Logging;
namespace Integration.Application.Services.Security
{
    public class ModuleService : IModuleService
    {
        private readonly IModuleRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ModuleService> _logger;

        public ModuleService(IModuleRepository repository, IMapper mapper, ILogger<ModuleService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<ModuleDTO> CreateAsync(ModuleDTO moduleDTO)
        {
            _logger.LogInformation("Creando modulo: {Name}", moduleDTO.Name);
            try
            {
                var module = _mapper.Map<Integration.Core.Entities.Security.Module>(moduleDTO);
                var result = await _repository.CreateAsync(module);
                _logger.LogInformation("Modulo creado con éxito: {ModuleId}, Nombre: {Name}", result.ModuleId, result.Name);
                return _mapper.Map<ModuleDTO>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el modulo: {Name}", moduleDTO.Name);
                throw;
            }
        }
        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation("Eliminando modulo con ID: {ModuleId}", id);
            try
            {
                bool success = await _repository.DeleteAsync(id);
                if (success)
                {
                    _logger.LogInformation("Modulo con ID {ModuleId} eliminada correctamente.", id);
                }
                else
                {
                    _logger.LogWarning("No se encontró el modulo con ID {ModuleId} para eliminar.", id);
                }
                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el modulo con ID {ModuleId}.", id);
                throw;
            }
        }
        public async Task<IEnumerable<ModuleDTO>> GetAllAsync()
        {
            _logger.LogInformation("Obteniendo todos los modulos.");
            try
            {
                var modules = await _repository.GetAllAsync();
                var modulesDTO = _mapper.Map<IEnumerable<ModuleDTO>>(modules);
                _logger.LogInformation("{Count} modulos obtenidas con éxito.", modulesDTO.Count());
                return modulesDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los modulos.");
                throw;
            }
        }
        public async Task<ModuleDTO> GetByIdAsync(int id)
        {
            _logger.LogInformation("Buscando modulos con ID: {ModuleId}", id);
            try
            {
                var molule = await _repository.GetByIdAsync(id);
                if (molule == null)
                {
                    _logger.LogWarning("No se encontró el modulo con ID {ModuleId}.", id);
                    return null;
                }
                _logger.LogInformation("Modulo encontrada: {ModuleId}, Nombre: {Name}", molule.ModuleId, molule.Name);
                return _mapper.Map<ModuleDTO>(molule);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el modulo con ID {ModuleId}.", id);
                throw;
            }
        }
        public async Task<ModuleDTO> UpdateAsync(ModuleDTO moduleDTO)
        {
            _logger.LogInformation("Actualizando modulo con ID: {ModuleId}, Nombre: {Name}", moduleDTO.ModuleId, moduleDTO.Name);
            try
            {
                var module = _mapper.Map<Integration.Core.Entities.Security.Module>(moduleDTO);
                var updatedModule = await _repository.UpdateAsync(module);
                if (updatedModule == null)
                {
                    _logger.LogWarning("No se pudo actualizar el modulo con ID {ModuleId}.", moduleDTO.ModuleId);
                    return null;
                }
                _logger.LogInformation("Modulo actualizado con éxito: {ModuleId}, Nombre: {Name}", updatedModule.ModuleId, updatedModule.Name);
                return _mapper.Map<ModuleDTO>(updatedModule);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el modulo con ID {ModuleDTO}.", moduleDTO.ModuleId);
                throw;
            }
        }
    }
}