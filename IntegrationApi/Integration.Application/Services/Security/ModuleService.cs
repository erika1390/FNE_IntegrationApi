using AutoMapper;
using Integration.Application.Interfaces.Security;
using Integration.Infrastructure.Interfaces.Security;
using Integration.Shared.DTO.Header;
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
        private readonly IUserRepository _userRepository;

        public ModuleService(IModuleRepository moduleRepository, IMapper mapper, ILogger<ModuleService> logger, IApplicationRepository applicationRepository, IUserRepository userRepository)
        {
            _moduleRepository = moduleRepository;
            _mapper = mapper;
            _logger = logger;
            _applicationRepository = applicationRepository;
            _userRepository = userRepository;
        }

        public async Task<ModuleDTO> CreateAsync(HeaderDTO header, ModuleDTO moduleDTO)
        {
            if (header == null || string.IsNullOrEmpty(header.UserCode))
            {
                throw new ArgumentNullException(nameof(header), "El encabezado o UserCode no pueden ser nulos.");
            }
            _logger.LogInformation("Creando modulo: {Name}", moduleDTO.Name);
            try
            {
                var user = await _userRepository.GetByCodeAsync(header.UserCode);
                if (user == null)
                {
                    throw new Exception($"No se encontró el usuario con código {header.UserCode}.");
                }
                var application = await _applicationRepository.GetByCodeAsync(moduleDTO.ApplicationCode);
                var module = _mapper.Map<Integration.Core.Entities.Security.Module>(moduleDTO);
                module.ApplicationId = application.Id;
                module.CreatedBy = user.UserName;
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

        public async Task<bool> DeactivateAsync(HeaderDTO header, string code)
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
                        return new List<ModuleDTO>();
                    }
                    applicationId = application.Id;
                    moduleFilter = a => a.ApplicationId == applicationId.Value;
                }
                var modules = await _moduleRepository.GetAllAsync(moduleFilter);
                var modulesDTOs = _mapper.Map<List<ModuleDTO>>(modules);
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
                _logger.LogInformation("Obteniendo todos los módulos y aplicando múltiples filtros.");

                int? applicationId = null;
                string applicationCode = null;
                List<Expression<Func<ModuleDTO, bool>>> otherFilters = new List<Expression<Func<ModuleDTO, bool>>>();
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
                        return new List<ModuleDTO>();
                    }
                    applicationId = application.Id;
                }
                Expression<Func<Integration.Core.Entities.Security.Module, bool>> moduleFilter = a => true;
                if (applicationId.HasValue)
                {
                    moduleFilter = a => a.ApplicationId == applicationId.Value;
                }
                var modules = await _moduleRepository.GetAllAsync(moduleFilter);
                var moduleDTOs = _mapper.Map<List<ModuleDTO>>(modules);
                foreach (var filter in otherFilters)
                {
                    moduleDTOs = moduleDTOs.Where(filter.Compile()).ToList();
                }
                return moduleDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio al obtener los módulos con múltiples filtros.");
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

        public async Task<ModuleDTO> UpdateAsync(HeaderDTO header, ModuleDTO moduleDTO)
        {
            _logger.LogInformation("Módulo creado exitosamente: ModuleCode={ModuleCode}, Name={Name}", moduleDTO.Code, moduleDTO.Name);
            try
            {
                var application = await _applicationRepository.GetByCodeAsync(moduleDTO.ApplicationCode);
                var moduleExist = await _moduleRepository.GetByCodeAsync(moduleDTO.Code); 
                var module = _mapper.Map<Integration.Core.Entities.Security.Module>(moduleDTO);
                module.ApplicationId = application.Id;
                module.Id = moduleExist.Id;
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