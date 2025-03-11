using AutoMapper;
using Integration.Application.Interfaces.Security;
using Integration.Infrastructure.Interfaces.Security;
using Integration.Shared.DTO.Security;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
namespace Integration.Application.Services.Security
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ApplicationService> _logger;
        public ApplicationService(IApplicationRepository repository, IMapper mapper, ILogger<ApplicationService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<ApplicationDTO> CreateAsync(ApplicationDTO applicationDTO)
        {
            _logger.LogInformation("Creando aplicación: {Name}", applicationDTO.Name);
            try
            {
                var application = _mapper.Map<Integration.Core.Entities.Security.Application>(applicationDTO);
                var result = await _repository.CreateAsync(application);
                _logger.LogInformation("Aplicación creada con éxito: {ApplicationId}, Nombre: {Name}", result.Id, result.Name);
                return _mapper.Map<ApplicationDTO>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la aplicación: {Name}", applicationDTO.Name);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(string code)
        {
            _logger.LogInformation("Eliminando aplicación con ApplicationCode: {ApplicationCode}", code);
            try
            {
                bool success = await _repository.DeleteAsync(code);
                if (success)
                {
                    _logger.LogInformation("Aplicación con ApplicationCode {ApplApplicationCodeicationId} eliminada correctamente.", code);
                }
                else
                {
                    _logger.LogWarning("No se encontró la aplicación con ApplicationCode {ApplicationCode} para eliminar.", code);
                }
                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la aplicación con ApplicationCode {ApplicationCode}.", code);
                throw;
            }
        }

        public async Task<IEnumerable<ApplicationDTO>> GetAllActiveAsync()
        {
            _logger.LogInformation("Obteniendo todas las aplicaciones.");
            try
            {
                var applications = await _repository.GetAllActiveAsync();
                var applicationsDTO = _mapper.Map<IEnumerable<ApplicationDTO>>(applications);
                _logger.LogInformation("{Count} aplicaciones obtenidas con éxito.", applicationsDTO.Count());
                return applicationsDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las aplicaciones.");
                throw;
            }
        }
        public async Task<List<ApplicationDTO>> GetAllAsync(Expression<Func<ApplicationDTO, bool>> filterDto)
        {
            try
            {
                _logger.LogInformation("Obteniendo todas las aplicaciones y aplicando el filtro en memoria.");
                var applications = await _repository.GetAllAsync(a => true);
                var applicationDTOs = _mapper.Map<List<ApplicationDTO>>(applications);
                var filteredApplications = applicationDTOs.AsQueryable().Where(filterDto).ToList();
                return filteredApplications;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio al obtener aplicaciones.");
                throw;
            }
        }
        public async Task<List<ApplicationDTO>> GetAllAsync(List<Expression<Func<ApplicationDTO, bool>>> predicados)
        {
            try
            {
                _logger.LogInformation("Obteniendo todas las aplicaciones y aplicando múltiples filtros en memoria.");
                var applications = await _repository.GetAllAsync(a => true);
                var applicationDTOs = _mapper.Map<List<ApplicationDTO>>(applications);
                IQueryable<ApplicationDTO> query = applicationDTOs.AsQueryable();
                foreach (var predicado in predicados)
                {
                    query = query.Where(predicado);
                }
                return query.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio al obtener aplicaciones con múltiples filtros.");
                throw;
            }
        }

        public async Task<ApplicationDTO> GetByCodeAsync(string code)
        {
            _logger.LogInformation("Buscando aplicación con ApplicationCode: {ApplicationCode}", code);
            try
            {
                var application = await _repository.GetByCodeAsync(code);
                if (application == null)
                {
                    _logger.LogWarning("No se encontró la aplicación con ApplicationCode {ApplicationCode}.", code);
                    return null;
                }
                _logger.LogInformation("Aplicación encontrada: {ApplicationCode}, Nombre: {Name}", application.Code, application.Name);
                return _mapper.Map<ApplicationDTO>(application);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la aplicación con ApplicationCode {ApplicationCode}.", code);
                throw;
            }
        }

        public async Task<ApplicationDTO> UpdateAsync(ApplicationDTO applicationDTO)
        {
            _logger.LogInformation("Actualizando aplicación con ApplicatioCode: {ApplicatioCode}, Nombre: {Name}", applicationDTO.Code, applicationDTO.Name);
            try
            {
                var application = _mapper.Map<Integration.Core.Entities.Security.Application>(applicationDTO);
                var updatedApplication = await _repository.UpdateAsync(application);
                if (updatedApplication == null)
                {
                    _logger.LogWarning("No se pudo actualizar la aplicación con ApplicatioCode {ApplicatioCode}.", applicationDTO.Code);
                    return null;
                }
                _logger.LogInformation("Aplicación actualizada con éxito: {ApplicatioCode}, Nombre: {Name}", updatedApplication.Code, updatedApplication.Name);
                return _mapper.Map<ApplicationDTO>(updatedApplication);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la aplicación con Code {ApplicatioCode}.", applicationDTO.Code);
                throw;
            }
        }
    }
}