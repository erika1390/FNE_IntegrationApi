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

        public async Task<Integration.Core.Entities.Security.Application> CreateAsync(Integration.Core.Entities.Security.Application application)
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
                application.Id, application.Name);
                return application;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la aplicación.");
                throw;
            }
        }
        public async Task<bool> DeactivateAsync(string code, string userName)
        {
            try
            {
                var application = await _context.Applications
                    .FirstOrDefaultAsync(a => a.Code == code);

                if (application == null)
                {
                    _logger.LogWarning("No se encontró la aplicación con ApplicationCode {ApplicationCode} para desactivar.", code);
                    return false;
                }
                application.IsActive = false;
                application.UpdatedAt = DateTime.UtcNow;
                application.UpdatedBy = userName;
                await _context.SaveChangesAsync();
                _logger.LogInformation("Aplicación desactivada: {ApplicationCode}", code);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al desactivar la aplicación con ApplicationCode {ApplicationCode}.", code);
                return false;
            }
        }

        public async Task<IEnumerable<Integration.Core.Entities.Security.Application>> GetAllActiveAsync()
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
                return Enumerable.Empty<Integration.Core.Entities.Security.Application>();
            }
        }

        public async Task<List<Integration.Core.Entities.Security.Application>> GetByFilterAsync(Expression<Func<Integration.Core.Entities.Security.Application, bool>> predicate)
        {
            try
            {
                _logger.LogInformation("Obteniendo aplicaciones con un predicado específico.");
                var applications = await _context.Applications.Where(predicate).ToListAsync();
                _logger.LogInformation("Se obtuvieron {Count} aplicaciones.", applications.Count);
                return applications;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los aplicaciones con el predicado especificado.");
                throw;
            }
        }

        public async Task<List<Integration.Core.Entities.Security.Application>> GetByMultipleFiltersAsync(List<Expression<Func<Integration.Core.Entities.Security.Application, bool>>> predicates)
        {
            try
            {
                _logger.LogInformation("Obteniendo aplicaciones con múltiples predicados.");
                var query = _context.Applications.AsQueryable();
                foreach (var predicado in predicates)
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
        public async Task<Integration.Core.Entities.Security.Application> GetByCodeAsync(string code)
        {
            try
            {
                var application = await _context.Applications
                    .Where(a => a.Code == code)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
                if (application == null)
                {
                    _logger.LogWarning("No se encontró la aplicación con ApplicationeCode {ApplicationeCode}.", code);
                }
                else
                {
                    _logger.LogInformation("Aplicación encontrada ApplicationeCode: {ApplicationeCode}, Nombre: {Name}",
                        application.Code, application.Name);
                }
                return application;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la aplicación con ApplicationeCode {ApplicationeCode}.", code);
                return null;
            }
        }

        public async Task<Integration.Core.Entities.Security.Application> UpdateAsync(Integration.Core.Entities.Security.Application application)
        {
            try
            {
                var applicationEntity = await _context.Applications
                    .Where(a => a.Code == application.Code)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
                if (applicationEntity == null)
                {
                    _logger.LogWarning("No se encontró la aplicación con ApplicationCode {ApplicationCode} para actualizar.", application.Code);
                    return null;
                }

                applicationEntity.Name = application.Name;
                applicationEntity.UpdatedBy = application.UpdatedBy;
                applicationEntity.UpdatedAt = DateTime.UtcNow;
                applicationEntity.IsActive = application.IsActive;

                _context.Applications.Update(applicationEntity);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Aplicación actualizada: {ApplicationCode}, Nombre: {Name}",
                    application.Code, application.Name);

                return applicationEntity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la aplicación con ApplicationCode {ApplicationCode}.", application.Code);
                return null;
            }
        }
    }
}