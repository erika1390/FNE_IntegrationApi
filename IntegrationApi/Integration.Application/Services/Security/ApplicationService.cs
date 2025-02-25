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
                _logger.LogInformation("Aplicación creada con éxito: {ApplicationId}, Nombre: {Name}", result.ApplicationId, result.Name);
                return _mapper.Map<ApplicationDTO>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la aplicación: {Name}", applicationDTO.Name);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation("Eliminando aplicación con ID: {ApplicationId}", id);
            try
            {
                bool success = await _repository.DeleteAsync(id);
                if (success)
                {
                    _logger.LogInformation("Aplicación con ID {ApplicationId} eliminada correctamente.", id);
                }
                else
                {
                    _logger.LogWarning("No se encontró la aplicación con ID {ApplicationId} para eliminar.", id);
                }
                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la aplicación con ID {ApplicationId}.", id);
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

        public async Task<List<ApplicationDTO>> GetAllAsync(Expression<Func<ApplicationDTO, bool>> predicado)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ApplicationDTO>> GetAllAsync(List<Expression<Func<ApplicationDTO, bool>>> predicados)
        {
            throw new NotImplementedException();
        }

        public async Task<ApplicationDTO> GetByIdAsync(int id)
        {
            _logger.LogInformation("Buscando aplicación con ID: {ApplicationId}", id);
            try
            {
                var application = await _repository.GetByIdAsync(id);
                if (application == null)
                {
                    _logger.LogWarning("No se encontró la aplicación con ID {ApplicationId}.", id);
                    return null;
                }
                _logger.LogInformation("Aplicación encontrada: {ApplicationId}, Nombre: {Name}", application.ApplicationId, application.Name);
                return _mapper.Map<ApplicationDTO>(application);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la aplicación con ID {ApplicationId}.", id);
                throw;
            }
        }

        public async Task<ApplicationDTO> UpdateAsync(ApplicationDTO applicationDTO)
        {
            _logger.LogInformation("Actualizando aplicación con ID: {ApplicationId}, Nombre: {Name}", applicationDTO.ApplicationId, applicationDTO.Name);
            try
            {
                var application = _mapper.Map<Integration.Core.Entities.Security.Application>(applicationDTO);
                var updatedApplication = await _repository.UpdateAsync(application);
                if (updatedApplication == null)
                {
                    _logger.LogWarning("No se pudo actualizar la aplicación con ID {ApplicationId}.", applicationDTO.ApplicationId);
                    return null;
                }
                _logger.LogInformation("Aplicación actualizada con éxito: {ApplicationId}, Nombre: {Name}", updatedApplication.ApplicationId, updatedApplication.Name);
                return _mapper.Map<ApplicationDTO>(updatedApplication);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la aplicación con ID {ApplicationId}.", applicationDTO.ApplicationId);
                throw;
            }
        }
    }
}