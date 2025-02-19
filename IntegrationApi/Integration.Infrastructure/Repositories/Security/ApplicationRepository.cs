using Integration.Core.Entities.Security;
using Integration.Infrastructure.Data.Contexts;
using Integration.Infrastructure.Interfaces.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Integration.Infrastructure.Repositories.Security
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ApplicationRepository> _logger;

        public ApplicationRepository(ApplicationDbContext context, ILogger<ApplicationRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Application> CreateAsync(Application application)
        {
            if (application == null)
            {
                _logger.LogWarning("Intento de crear una aplicación con datos nulos.");
                throw new ArgumentNullException(nameof(application), "La aplicación no puede ser nula.");
            }

            try
            {
                await _context.Applications.AddAsync(application);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Aplicación creada exitosamente: {ApplicationId}, Nombre: {Name}",
                    application.ApplicationId, application.Name);

                return application;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la aplicación.");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var entity = await _context.Applications.FindAsync(id);
                if (entity == null)
                {
                    _logger.LogWarning("No se encontró la aplicación con ID {ApplicationId} para eliminar.", id);
                    return false;
                }

                entity.IsActive = false;
                entity.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                _logger.LogInformation("Aplicación desactivada: {ApplicationId}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la aplicación con ID {ApplicationId}.", id);
                return false;
            }
        }

        public async Task<IEnumerable<Application>> GetAllAsync()
        {
            try
            {
                var applications = await _context.Applications.ToListAsync();
                _logger.LogInformation("Se obtuvieron {Count} aplicaciones de la base de datos.", applications.Count);
                return applications;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las aplicaciones.");
                return Enumerable.Empty<Application>();
            }
        }

        public async Task<Application> GetByIdAsync(int id)
        {
            try
            {
                var application = await _context.Applications.FindAsync(id);
                if (application == null)
                {
                    _logger.LogWarning("No se encontró la aplicación con ID {ApplicationId}.", id);
                }
                else
                {
                    _logger.LogInformation("Aplicación encontrada: {ApplicationId}, Nombre: {Name}",
                        application.ApplicationId, application.Name);
                }
                return application;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la aplicación con ID {ApplicationId}.", id);
                return null;
            }
        }

        public async Task<Application> UpdateAsync(Application application)
        {
            try
            {
                var existingEntity = await _context.Applications.FindAsync(application.ApplicationId);
                if (existingEntity == null)
                {
                    _logger.LogWarning("No se encontró la aplicación con ID {ApplicationId} para actualizar.", application.ApplicationId);
                    return null;
                }

                existingEntity.Name = application.Name;
                existingEntity.UpdatedBy = application.UpdatedBy;
                existingEntity.UpdatedAt = DateTime.UtcNow;
                existingEntity.IsActive = application.IsActive;

                _context.Applications.Update(existingEntity);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Aplicación actualizada: {ApplicationId}, Nombre: {Name}",
                    application.ApplicationId, application.Name);

                return existingEntity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la aplicación con ID {ApplicationId}.", application.ApplicationId);
                return null;
            }
        }
    }
}