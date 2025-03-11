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
        private readonly IModuleRepository _moduleRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ModuleService> _logger;
        private readonly IApplicationRepository _applicationRepository;

        public ModuleService(IModuleRepository moduleRepository, IMapper mapper, ILogger<ModuleService> logger, IApplicationRepository applicationRepository)
        {
            _moduleRepository = moduleRepository;
            _mapper = mapper;
            _logger = logger;
            _applicationRepository = applicationRepository;
        }

        public async Task<ModuleDTO> CreateAsync(ModuleDTO moduleDTO)
        {
            _logger.LogInformation("Creando modulo: {Name}", moduleDTO.Name);
            try
            {
                var application = await _applicationRepository.GetByCodeAsync(moduleDTO.ApplicationCode);
                var module = _mapper.Map<Integration.Core.Entities.Security.Module>(moduleDTO);
                module.ApplicationId = application.Id;
                var result = await _moduleRepository.CreateAsync(module);
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
                bool success = await _moduleRepository.DeactivateAsync(code);
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
                var modules = await _moduleRepository.GetAllActiveAsync();
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
        public async Task<List<ModuleDTO>> GetAllAsync(Expression<Func<ModuleDTO, bool>> predicado)
        {
            try
            {
                _logger.LogInformation("Obteniendo todos los módulos y aplicando el filtro.");
                int? applicationId = null;
                Expression<Func<Integration.Core.Entities.Security.Module, bool>> moduleFilter = a => true;
                if (predicado != null && IsFilteringByApplicationCode(predicado, out string applicationCode))
                {
                    _logger.LogInformation("Buscando ID de la aplicación con código: {ApplicationCode}", applicationCode);
                    var application = await _applicationRepository.GetByCodeAsync(applicationCode);

                    if (application == null)
                    {
                        _logger.LogWarning("No se encontró la aplicación con código: {ApplicationCode}", applicationCode);
                        return new List<ModuleDTO>(); // Si no existe la aplicación, devolver lista vacía
                    }

                    applicationId = application.Id;
                    moduleFilter = a => a.ApplicationId == applicationId.Value;
                }

                // Obtener módulos filtrados en la base de datos
                var modules = await _moduleRepository.GetAllAsync(moduleFilter);
                var modulesDTOs = _mapper.Map<List<ModuleDTO>>(modules);

                // Si el filtro proporcionado es diferente de ApplicationCode, aplicarlo en memoria
                if (predicado != null && !IsFilteringByApplicationCode(predicado, out _))
                {
                    modulesDTOs = modulesDTOs.AsQueryable().Where(predicado).ToList();
                }

                return modulesDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio al obtener módulos.");
                throw;
            }
        }

        // Método auxiliar para detectar si el filtro contiene ApplicationCode y extraer su valor
        private bool IsFilteringByApplicationCode(Expression<Func<ModuleDTO, bool>> predicado, out string applicationCode)
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


        public async Task<List<ModuleDTO>> GetAllAsync(List<Expression<Func<ModuleDTO, bool>>> predicados)
        {
            try
            {
                _logger.LogInformation("Obteniendo todos los modulos y aplicando múltiples filtros en memoria.");
                var modules = await _moduleRepository.GetAllAsync(a => true);
                var moduleDTOs = _mapper.Map<List<ModuleDTO>>(modules);
                IQueryable<ModuleDTO> query = moduleDTOs.AsQueryable();
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
                var module = await _moduleRepository.GetByCodeAsync(code);
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
                var updatedModule = await _moduleRepository.UpdateAsync(module);
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