using Integration.Application.Interfaces.Security;
using Integration.Infrastructure.Interfaces.Security;
using Integration.Shared.DTO.Security;

using Microsoft.Extensions.Logging;

namespace Integration.Application.Services.Security
{
    public class UserPermissionService : IUserPermissionService
    {
        private readonly IUserPermissionRepository _repository;
        private readonly ILogger<UserPermissionService> _logger;
        private readonly IApplicationRepository _applicationRepository;
        public UserPermissionService(IUserPermissionRepository repository, ILogger<UserPermissionService> logger, IApplicationRepository applicationRepository)
        {
            _repository = repository;
            _logger = logger;
            _applicationRepository = applicationRepository;
        }
        public async Task<IEnumerable<UserPermissionDTO>> GetAllActiveByUserCodeAsync(string userCode, string applicationCode)
        {
            _logger.LogInformation("Obteniendo todos los UserPermissionDTOResponse.");
            try
            {
                var application = await _applicationRepository.GetByCodeAsync(applicationCode);
                var permissions = await _repository.GetAllActiveByUserIdAsync(userCode,application.Id);               
                _logger.LogInformation("{Count} UserPermissionDTOResponse obtenidas con éxito.", permissions.Count());
                return permissions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los UserPermissionDTOResponse.");
                throw;
            }
        }
    }
}