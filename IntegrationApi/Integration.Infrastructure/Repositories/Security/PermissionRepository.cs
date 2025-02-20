using Integration.Core.Entities.Security;
using Integration.Infrastructure.Data.Contexts;
using Integration.Infrastructure.Interfaces.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
namespace Integration.Infrastructure.Repositories.Security
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PermissionRepository> _logger;
        public PermissionRepository(ApplicationDbContext context, ILogger<PermissionRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<Permission> CreateAsync(Permission permission)
        {
            if (permission == null)
            {
                _logger.LogWarning("Intento de crear un permiso con datos nulos.");
                throw new ArgumentNullException(nameof(permission), "El permiso no puede ser nulo.");
            }
            try
            {
                await _context.Permissions.AddAsync(permission);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Permiso creado exitosamente: Permiso: {PermissionId}, Nombre: {Name}", permission.PermissionId, permission.Name);
                return permission;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error de base de datos al crear el permiso.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al crear el permiso.");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var permission = await _context.Permissions.FindAsync(id);
                if (permission == null)
                {
                    _logger.LogWarning("No se encontró el permiso con ID {PermissionId} para eliminar.", id);
                    return false;
                }
                permission.IsActive = false;
                permission.UpdatedAt = DateTime.UtcNow;
                _context.Permissions.Update(permission);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Permiso desactivado: {PermissionId}", id);
                return true;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error de base de datos al eliminar el pemiso con ID {PermissionId}.", id);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al eliminar el permiso con ID {PermissionId}.", id);
                return false;
            }
        }

        public async Task<IEnumerable<Permission>> GetAllAsync()
        {
            try
            {
                var permission = await _context.Permissions.AsNoTracking().ToListAsync();

                _logger.LogInformation("Se obtuvieron {Count} permisos de la base de datos.", permission.Count);
                return permission;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los permisos.");
                return Enumerable.Empty<Permission>();
            }
        }

        public async Task<Permission> GetByIdAsync(int id)
        {
            try
            {
                var permission = await _context.Permissions.AsNoTracking().FirstOrDefaultAsync(p => p.PermissionId == id);
                if (permission == null)
                {
                    _logger.LogWarning("No se encontró el permiso con ID {PermissionId}.", id);
                    return null;
                }

                _logger.LogInformation("Permiso encontrado: Permiso: {PermissionId}, Nombre: {Name}", permission.PermissionId, permission.Name);

                return permission;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el permiso con ID {PermissionId}.", id);
                return null;
            }
        }

        public async Task<Permission> UpdateAsync(Permission permission)
        {
            if (permission == null)
            {
                _logger.LogWarning("Intento de actualizar un permiso con datos nulos.");
                throw new ArgumentNullException(nameof(permission), "El permiso no puede ser nulo.");
            }
            try
            {
                var permissionEntity = await _context.Permissions.FindAsync(permission.PermissionId);
                if (permissionEntity == null)
                {
                    _logger.LogWarning("No se encontró el permiso con ID {PermissionId} para actualizar.", permission.PermissionId);
                    return null;
                }
                permissionEntity.Name = permission.Name;
                permissionEntity.UpdatedBy = permission.UpdatedBy;
                permissionEntity.UpdatedAt = DateTime.UtcNow;
                permissionEntity.IsActive = permission.IsActive;
                _context.Permissions.Update(permissionEntity);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Permiso actualizado: {PermissionId}, Nombre: {Name}", permission.PermissionId, permission.Name);
                return permissionEntity;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error de base de datos al actualizar el permiso con ID {PermissionId}.", permission.PermissionId);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al actualizar el permiso con ID {PermissionId}.", permission.PermissionId);
                return null;
            }
        }
    }
}