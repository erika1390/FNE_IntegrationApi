using Integration.Core.Entities.Security;
using Integration.Infrastructure.Data.Contexts;
using Integration.Infrastructure.Interfaces.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
namespace Integration.Infrastructure.Repositories.Security
{
    public class RoleModuleRepository : IRoleModuleRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RoleModuleRepository> _logger;
        public RoleModuleRepository(ApplicationDbContext context, ILogger<RoleModuleRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<RoleModule> CreateAsync(RoleModule roleModule)
        {
            if (roleModule == null)
            {
                _logger.LogWarning("Intento de crear un RoleModule con datos nulos.");
                throw new ArgumentNullException(nameof(roleModule), "El RoleModule no puede ser nulo.");
            }
            try
            {
                await _context.RoleModules.AddAsync(roleModule);
                await _context.SaveChangesAsync();
                _logger.LogInformation("RoleModule creado exitosamente: RoleId: {RoleId}, ModuleId: {ModuleId}, PermissionId: {PermissionId}",roleModule.RoleId, roleModule.ModuleId, roleModule.PermissionId);
                return roleModule;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error de base de datos al crear el RoleModule.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al crear el RoleModule.");
                throw;
            }
        }
        public async Task<IEnumerable<RoleModule>> GetAllActiveAsync()
        {
            try
            {
                var roleModules = await _context.RoleModules
                    .Include(rm => rm.Role)
                    .Include(rm => rm.Module)
                    .Include(rm => rm.Permission)
                    .Where(rm => rm.IsActive)
                    .AsNoTracking()
                    .ToListAsync();

                _logger.LogInformation("Se obtuvieron {Count} RoleModules activos.", roleModules.Count);
                return roleModules;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los RoleModules activos.");
                return Enumerable.Empty<RoleModule>();
            }
        }

        public async Task<List<RoleModule>> GetAllAsync(Expression<Func<RoleModule, bool>> predicate)
        {
            try
            {
                var roleModules = await _context.RoleModules
                    .Include(rm => rm.Role)
                    .Include(rm => rm.Module)
                    .Include(rm => rm.Permission)
                    .Where(predicate)
                    .AsNoTracking()
                    .ToListAsync();

                _logger.LogInformation("Se obtuvieron {Count} RoleModules con el criterio de búsqueda.", roleModules.Count);
                return roleModules;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener RoleModules con criterio de búsqueda.");
                return new List<RoleModule>();
            }
        }

        public async Task<List<RoleModule>> GetAllAsync(List<Expression<Func<RoleModule, bool>>> predicates)
        {
            try
            {
                IQueryable<RoleModule> query = _context.RoleModules
                    .Include(rm => rm.Role)
                    .Include(rm => rm.Module)
                    .Include(rm => rm.Permission)
                    .AsQueryable();

                foreach (var predicate in predicates)
                {
                    query = query.Where(predicate);
                }

                var roleModules = await query.AsNoTracking().ToListAsync();
                _logger.LogInformation("Se obtuvieron {Count} RoleModules con múltiples criterios de búsqueda.", roleModules.Count);
                return roleModules;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener RoleModules con múltiples criterios de búsqueda.");
                return new List<RoleModule>();
            }
        }

        public async Task<RoleModule> GetByIdAsync(int id)
        {
            try
            {
                var roleModule = await _context.RoleModules
                    .Include(rm => rm.Role)
                    .Include(rm => rm.Module)
                    .Include(rm => rm.Permission)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(rm => rm.Id == id);

                if (roleModule == null)
                {
                    _logger.LogWarning("No se encontró el RoleModule con ID {RoleModuleId}.", id);
                    return null;
                }

                _logger.LogInformation("RoleModule encontrado: RoleId: {RoleId}, ModuleId: {ModuleId}, PermissionId: {PermissionId}",
                    roleModule.RoleId, roleModule.ModuleId, roleModule.PermissionId);

                return roleModule;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el RoleModule con ID {RoleModuleId}.", id);
                return null;
            }
        }

        public async Task<RoleModule> UpdateAsync(RoleModule roleModule)
        {
            if (roleModule == null)
            {
                _logger.LogWarning("Intento de actualizar un RoleModule con datos nulos.");
                throw new ArgumentNullException(nameof(roleModule), "El RoleModule no puede ser nulo.");
            }

            try
            {
                var existingRoleModule = await _context.RoleModules.FindAsync(roleModule.Id);
                if (existingRoleModule == null)
                {
                    _logger.LogWarning("No se encontró el RoleModule con ID {RoleModuleId} para actualizar.", roleModule.Id);
                    return null;
                }

                existingRoleModule.RoleId = roleModule.RoleId;
                existingRoleModule.ModuleId = roleModule.ModuleId;
                existingRoleModule.PermissionId = roleModule.PermissionId;
                existingRoleModule.UpdatedBy = roleModule.UpdatedBy;
                existingRoleModule.UpdatedAt = DateTime.UtcNow;
                existingRoleModule.IsActive = roleModule.IsActive;

                _context.RoleModules.Update(existingRoleModule);
                await _context.SaveChangesAsync();

                _logger.LogInformation("RoleModule actualizado: RoleId: {RoleId}, ModuleId: {ModuleId}, PermissionId: {PermissionId}",
                    roleModule.RoleId, roleModule.ModuleId, roleModule.PermissionId);

                return existingRoleModule;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error de base de datos al actualizar el RoleModule con ID {RoleModuleId}.", roleModule.Id);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al actualizar el RoleModule con ID {RoleModuleId}.", roleModule.Id);
                return null;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var roleModule = await _context.RoleModules.FindAsync(id);
                if (roleModule == null)
                {
                    _logger.LogWarning("No se encontró el RoleModule con ID {Id} para eliminar.", id);
                    return false;
                }

                roleModule.IsActive = false;
                roleModule.UpdatedAt = DateTime.UtcNow;

                _context.RoleModules.Update(roleModule);
                await _context.SaveChangesAsync();

                _logger.LogInformation("RoleModule desactivado: {Id}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el RoleModule con ID {Id}.", id);
                return false;
            }
        }
    }
}