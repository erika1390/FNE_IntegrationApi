using AutoMapper;
using Integration.Application.Interfaces.Security;
using Integration.Infrastructure.Interfaces.Security;
using Integration.Shared.DTO.Security;
using Microsoft.Extensions.Logging;

using System.Linq.Expressions;

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
                _logger.LogInformation("Modulo creado con éxito: {ModuleId}, Nombre: {Name}", result.Id, result.Name);
                return _mapper.Map<ModuleDTO>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el modulo: {Name}", moduleDTO.Name);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(string code)
        {
            _logger.LogInformation("Eliminando modulo con ModuleCode: {ModuleCode}", code);
            try
            {
                bool success = await _repository.DeactivateAsync(code);
                if (success)
                {
                    _logger.LogInformation("Modulo con ModuleCode {ModuleCode} eliminada correctamente.", code);
                }
                else
                {
                    _logger.LogWarning("No se encontró el modulo con ModuleCode {ModuleCode} para eliminar.", code);
                }
                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el modulo con ModuleCode {ModuleCode}.", code);
                throw;
            }
        }

        public async Task<IEnumerable<ModuleDTO>> GetAllActiveAsync()
        {
            _logger.LogInformation("Obteniendo todos los modulos.");
            try
            {
                var modules = await _repository.GetAllActiveAsync();
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

        public async Task<List<ModuleDTO>> GetAllAsync(Expression<Func<ModuleDTO, bool>> filterDto)
        {
            try
            {
                _logger.LogInformation("Obteniendo todos los modulos y aplicando el filtro en memoria.");
                var modules = await _repository.GetAllAsync(a => true);
                var modulesDTOs = _mapper.Map<List<ModuleDTO>>(modules);
                var filteredApplications = modulesDTOs.AsQueryable().Where(filterDto).ToList();
                return filteredApplications;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio al obtener aplicaciones.");
                throw;
            }
        }

        public async Task<List<ModuleDTO>> GetAllAsync(List<Expression<Func<ModuleDTO, bool>>> predicados)
        {
            try
            {
                _logger.LogInformation("Obteniendo todos los modulos y aplicando múltiples filtros en memoria.");
                var applications = await _repository.GetAllAsync(a => true);
                var applicationDTOs = _mapper.Map<List<ModuleDTO>>(applications);
                IQueryable<ModuleDTO> query = applicationDTOs.AsQueryable();
                foreach (var predicado in predicados)
                {
                    query = query.Where(predicado);
                }
                return query.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio al obtener los modulos con múltiples filtros.");
                throw;
            }
        }

        public async Task<ModuleDTO> GetByCodeAsync(string code)
        {
            _logger.LogInformation("Buscando aplicación con ModuleCode: {ModuleCode}", code);
            try
            {
                var module = await _repository.GetByCodeAsync(code);
                if (module == null)
                {
                    _logger.LogWarning("No se encontró la modulo con ModuleCode {ModuleCode}.", code);
                    return null;
                }
                _logger.LogInformation("Modulo encontrada: {ModuleCode}, Nombre: {Name}", module.Code, module.Name);
                return _mapper.Map<ModuleDTO>(module);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el modulo con ModuleCode {ModuleCode}.", code);
                throw;
            }
        }

        public async Task<ModuleDTO> UpdateAsync(ModuleDTO moduleDTO)
        {
            _logger.LogInformation("Módulo creado exitosamente: ModuleCode={ModuleCode}, Name={Name}", moduleDTO.Code, moduleDTO.Name);
            try
            {
                var module = _mapper.Map<Integration.Core.Entities.Security.Module>(moduleDTO);
                var updatedModule = await _repository.UpdateAsync(module);
                if (updatedModule == null)
                {
                    _logger.LogWarning("Módulo con ModuleCode {ModuleCode} no encontrado.", moduleDTO.Code);
                    return null;
                }
                _logger.LogInformation("Modulo actualizado con éxito: ModuleCode: {ModuleCode}, Nombre: {Name}", updatedModule.Code, updatedModule.Name);
                return _mapper.Map<ModuleDTO>(updatedModule);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar módulo con ModuleCode {ModuleCode}.", moduleDTO.Code);
                throw;
            }
        }
    }
}