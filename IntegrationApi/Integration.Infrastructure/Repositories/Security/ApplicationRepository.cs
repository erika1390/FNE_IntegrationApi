using Integration.Core.Entities.Security;
using Integration.Infrastructure.Data.Contexts;
using Integration.Infrastructure.Interfaces.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
namespace Integration.Infrastructure.Repositories.Security
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ApplicationRepository> _logger;

        public ApplicationRepository(ApplicationDbContext context, ILogger<ApplicationRepository> logger)
        {
            _context = context;
            _logger = logger;
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
                var application = await _context.Applications.FindAsync(id);
                if (application == null)
                {
                    _logger.LogWarning("No se encontró la aplicación con ID {ApplicationId} para eliminar.", id);
                    return false;
                }

                application.IsActive = false;
                application.UpdatedAt = DateTime.UtcNow;
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

        public async Task<IEnumerable<Application>> GetAllActiveAsync()
        {
            try
            {
                var applications = await _context.Applications.Where(a => a.IsActive == true).ToListAsync();
                _logger.LogInformation("Se obtuvieron {Count} aplicaciones de la base de datos.", applications.Count);
                return applications;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las aplicaciones.");
                return Enumerable.Empty<Application>();
            }
        }

        public async Task<List<Application>> GetAllAsync(Expression<Func<Application, bool>> predicado)
        {
            try
            {
                _logger.LogInformation("Obteniendo aplicaciones con un predicado específico.");
                var applications = await _context.Applications.Where(predicado).ToListAsync();
                _logger.LogInformation("Se obtuvieron {Count} aplicaciones.", applications.Count);
                return applications;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los aplicaciones con el predicado especificado.");
                throw;
            }
        }

        public async Task<List<Application>> GetAllAsync(List<Expression<Func<Application, bool>>> predicados)
        {
            try
            {
                _logger.LogInformation("Obteniendo aplicaciones con múltiples predicados.");
                var query = _context.Applications.AsQueryable();
                foreach (var predicado in predicados)
                {
                    query = query.Where(predicado);
                }
                var modules = await query.ToListAsync();
                _logger.LogInformation("Se obtuvieron {Count} aplicaciones tras aplicar múltiples predicados.", modules.Count);
                return modules;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los aplicaciones con múltiples predicados.");
                throw;
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
                var applicationEntity = await _context.Applications.FindAsync(application.ApplicationId);
                if (applicationEntity == null)
                {
                    _logger.LogWarning("No se encontró la aplicación con ID {ApplicationId} para actualizar.", application.ApplicationId);
                    return null;
                }

                applicationEntity.Name = application.Name;
                applicationEntity.UpdatedBy = application.UpdatedBy;
                applicationEntity.UpdatedAt = DateTime.UtcNow;
                applicationEntity.IsActive = application.IsActive;

                _context.Applications.Update(applicationEntity);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Aplicación actualizada: {ApplicationId}, Nombre: {Name}",
                    application.ApplicationId, application.Name);

                return applicationEntity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la aplicación con ID {ApplicationId}.", application.ApplicationId);
                return null;
            }
        }
    }
}