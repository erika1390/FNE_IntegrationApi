using AutoMapper;
using Integration.Application.Interfaces.Security;
using Integration.Infrastructure.Interfaces.Security;
using Integration.Shared.DTO.Header;
using Integration.Shared.DTO.Security;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

using System.Linq.Expressions;
namespace Integration.Application.Services.Security
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ApplicationService> _logger;
        private readonly IUserRepository _userRepository;
        public ApplicationService(IApplicationRepository repository, IMapper mapper, ILogger<ApplicationService> logger, IUserRepository userRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _userRepository = userRepository;
        }

        public async Task<ApplicationDTO> CreateAsync(HeaderDTO header, ApplicationDTO applicationDTO)
        {
            if (header == null || string.IsNullOrEmpty(header.UserCode))
            {
                throw new ArgumentNullException(nameof(header), "El encabezado o UserCode no pueden ser nulos.");
            }

            if (applicationDTO == null)
            {
                throw new ArgumentNullException(nameof(applicationDTO), "La aplicación no puede ser nula.");
            }
            _logger.LogInformation("Creando aplicación: {Name}", applicationDTO.Name);
            try
            {
                var user = await _userRepository.GetByCodeAsync(header.UserCode);
                if (user == null)
                {
                    throw new Exception($"No se encontró el usuario con código {header.UserCode}.");
                }
                var application = _mapper.Map<Integration.Core.Entities.Security.Application>(applicationDTO);
                if (application == null)
                {
                    throw new Exception("Error al mapear ApplicationDTO a Application.");
                }
                application.CreatedBy = user.UserName;
                application.UpdatedBy = user.UserName;
                var result = await _repository.CreateAsync(application);
                _logger.LogInformation("Aplicación creada con éxito: {ApplicationId}, Nombre: {Name}", result.Id, result.Name);
                return _mapper.Map<ApplicationDTO>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la aplicación: {Name}", applicationDTO.Name);
                throw new Exception("Error al crear la aplicación", ex);
            }
        }

        public async Task<bool> DeactivateAsync(HeaderDTO header, string code)
        {
            _logger.LogInformation("Desactivando aplicación con ApplicationCode: {ApplicationCode}", code);
            try
            {
                var user = await _userRepository.GetByCodeAsync(header.UserCode);
                if (user == null)
                {
                    throw new Exception($"No se encontró el usuario con código {header.UserCode}.");
                }
                bool success = await _repository.DeactivateAsync(code);
                if (success)
                {
                    _logger.LogInformation("Aplicación con ApplicationCode {ApplicationCode} desactivada correctamente.", code);
                }
                else
                {
                    _logger.LogWarning("No se encontró la aplicación con ApplicationCode {ApplicationCode} para desactivar.", code);
                }
                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al desactivar la aplicación con ApplicationCode {ApplicationCode}.", code);
                throw new Exception("Error al desactivar la aplicación", ex);
            }
        }

        public async Task<IEnumerable<ApplicationDTO>> GetAllActiveAsync()
        {
            _logger.LogInformation("Obteniendo todas las aplicaciones activas.");
            try
            {
                var applications = await _repository.GetAllActiveAsync();
                var applicationsDTO = _mapper.Map<IEnumerable<ApplicationDTO>>(applications);
                _logger.LogInformation("{Count} aplicaciones activas obtenidas con éxito.", applicationsDTO.Count());
                return applicationsDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las aplicaciones activas.");
                throw new Exception("Error al obtener todas las aplicaciones activas", ex);
            }
        }

        public async Task<List<ApplicationDTO>> GetAllAsync(Expression<Func<ApplicationDTO, bool>> filterDto)
        {
            _logger.LogInformation("Obteniendo todas las aplicaciones y aplicando el filtro en memoria.");
            try
            {
                var applications = await _repository.GetAllAsync(a => true);
                var applicationDTOs = _mapper.Map<List<ApplicationDTO>>(applications);
                var filteredApplications = applicationDTOs.AsQueryable().Where(filterDto).ToList();
                return filteredApplications;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener aplicaciones con filtro.");
                throw new Exception("Error al obtener aplicaciones con filtro", ex);
            }
        }

        public async Task<List<ApplicationDTO>> GetAllAsync(List<Expression<Func<ApplicationDTO, bool>>> predicados)
        {
            _logger.LogInformation("Obteniendo todas las aplicaciones y aplicando múltiples filtros en memoria.");
            try
            {
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
                _logger.LogError(ex, "Error al obtener aplicaciones con múltiples filtros.");
                throw new Exception("Error al obtener aplicaciones con múltiples filtros", ex);
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
                throw new Exception("Error al obtener la aplicación", ex);
            }
        }

        public async Task<ApplicationDTO> UpdateAsync(HeaderDTO header, ApplicationDTO applicationDTO)
        {
            _logger.LogInformation("Actualizando aplicación con ApplicatioCode: {ApplicatioCode}, Nombre: {Name}", applicationDTO.Code, applicationDTO.Name);
            try
            {
                var user = await _userRepository.GetByCodeAsync(header.UserCode);
                if (user == null)
                {
                    throw new Exception($"No se encontró el usuario con código {header.UserCode}.");
                }
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
                _logger.LogError(ex, "Error al actualizar la aplicación con ApplicatioCode {ApplicatioCode}.", applicationDTO.Code);
                throw new Exception("Error al actualizar la aplicación", ex);
            }
        }
    }
}