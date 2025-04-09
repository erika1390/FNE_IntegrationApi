using Integration.Core.Entities.Security;
using Integration.Infrastructure.Data.Contexts;
using Integration.Infrastructure.Interfaces.Security;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System.Data;
using System.Linq.Expressions;
namespace Integration.Infrastructure.Repositories.Security
{
    public class RoleMenuPermissionRepository : IRoleMenuPermissionRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RoleMenuPermissionRepository> _logger;
        public RoleMenuPermissionRepository(ApplicationDbContext context, ILogger<RoleMenuPermissionRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<RoleMenuPermission> CreateAsync(RoleMenuPermission roleMenuPermissions)
        {
            if (roleMenuPermissions == null)
            {
                _logger.LogWarning("Intento de crear un RoleMenuPermissions con datos nulos.");
                throw new ArgumentNullException(nameof(roleMenuPermissions), "El RoleMenuPermissions no puede ser nulo.");
            }

            try
            {
                // Evitar que Entity Framework intente insertar relaciones
                roleMenuPermissions.Role = null;
                roleMenuPermissions.Menu = null;
                roleMenuPermissions.Permission = null;

                // Insertar el nuevo registro
                await _context.RoleMenuPermissions.AddAsync(roleMenuPermissions);
                await _context.SaveChangesAsync();

                // Recargar el objeto desde la base de datos con sus relaciones
                var createdEntity = await _context.RoleMenuPermissions
                    .Include(rmp => rmp.Role)
                    .Include(rmp => rmp.Menu)
                    .Include(rmp => rmp.Permission)
                    .FirstOrDefaultAsync(rmp => rmp.Id == roleMenuPermissions.Id);

                _logger.LogInformation("RoleMenuPermissions creado exitosamente: RoleId: {RoleId}, MenuId: {MenuId}, PermissionId: {PermissionId}, CreatedBy: {CreatedBy}",
                    createdEntity.RoleId, createdEntity.MenuId, createdEntity.PermissionId, createdEntity.CreatedBy);

                return createdEntity;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error de base de datos al crear el RoleMenuPermissions.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al crear el RoleMenuPermissions.");
                throw;
            }
        }

        public async Task<bool> DeactivateAsync(RoleMenuPermission roleMenuPermissions)
        {
            try
            {
                var roleMenuPermission = await _context.RoleMenuPermissions
                    .Where(a => a.RoleId == roleMenuPermissions.RoleId
                    && a.MenuId == roleMenuPermissions.MenuId
                    && a.PermissionId == roleMenuPermissions.PermissionId)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
                if (roleMenuPermission == null)
                {
                    _logger.LogWarning("No se encontró el RoleMenuPermissions con RoleId {RoleId}, MenuId {MenuId} y PermissionId {PermissionId} para eliminar.", roleMenuPermissions.RoleId, roleMenuPermissions.MenuId, roleMenuPermissions.PermissionId);
                    return false;
                }
                roleMenuPermission.IsActive = false;
                roleMenuPermission.UpdatedAt = DateTime.UtcNow;
                _context.RoleMenuPermissions.Update(roleMenuPermission);
                await _context.SaveChangesAsync();
                _logger.LogInformation("RoleMenuPermissions desactivado: {RoleId}, MenuId {MenuId} y PermissionId {PermissionId} para eliminar.", roleMenuPermissions.RoleId, roleMenuPermissions.MenuId, roleMenuPermissions.PermissionId);
                return true;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error de base de datos al eliminar el RoleMenuPermissions con {RoleId}, MenuId {MenuId} y PermissionId {PermissionId} para eliminar.", roleMenuPermissions.RoleId, roleMenuPermissions.MenuId, roleMenuPermissions.PermissionId);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al eliminar el RoleMenuPermissions con {RoleId}, MenuId {MenuId} y PermissionId {PermissionId} para eliminar.", roleMenuPermissions.RoleId, roleMenuPermissions.MenuId, roleMenuPermissions.PermissionId);
                return false;
            }
        }

        public async Task<IEnumerable<RoleMenuPermission>> GetAllActiveAsync()
        {
            try
            {
                var roleMenuPermission = await _context.RoleMenuPermissions
                    .Where(r => r.IsActive == true)
                    .Include(a => a.Role)
                    .Include(a => a.Menu)
                    .Include(a => a.Permission)
                    .AsNoTracking()
                    .ToListAsync();

                _logger.LogInformation("Se obtuvieron {Count} RoleMenuPermissions de la base de datos.", roleMenuPermission.Count);
                return roleMenuPermission;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los RoleMenuPermissions.");
                return Enumerable.Empty<RoleMenuPermission>();
            }
        }

        public async Task<List<RoleMenuPermission>> GetByFilterAsync(Expression<Func<RoleMenuPermission, bool>> predicate)
        {
            try
            {
                _logger.LogInformation("Obteniendo RoleMenuPermission con un predicado específico.");
                var roleMenuPermissions = await _context.RoleMenuPermissions
                    .Where(predicate)
                    .Include(a => a.Role)
                    .Include(a => a.Menu)
                    .Include(a => a.Permission)
                    .AsNoTracking()
                    .ToListAsync();
                _logger.LogInformation("Se obtuvieron {Count} RoleMenuPermission.", roleMenuPermissions.Count);
                return roleMenuPermissions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los RoleMenuPermission con el predicado especificado.");
                throw;
            }
        }

        public async Task<List<RoleMenuPermission>> GetByMultipleFiltersAsync(List<Expression<Func<RoleMenuPermission, bool>>> predicates)
        {
            try
            {
                _logger.LogInformation("Obteniendo RoleMenuPermission con múltiples predicados.");
                var query = _context.RoleMenuPermissions.AsQueryable();
                foreach (var predicado in predicates)
                {
                    query = query
                        .Where(predicado)
                        .Include(a => a.Role)
                        .Include(a => a.Menu)
                        .Include(a => a.Permission);
                }
                var roles = await query.ToListAsync();
                _logger.LogInformation("Se obtuvieron {Count} RoleMenuPermission tras aplicar múltiples predicados.", roles.Count);
                return roles;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los RoleMenuPermission con múltiples predicados.");
                throw;
            }
        }

        public async Task<RoleMenuPermission> GetByRoleIdMenuIdPermissionsIdAsync(RoleMenuPermission roleMenuPermissions)
        {
            try
            {
                var roleMenuPermission = await _context.RoleMenuPermissions
                    .Where(a => a.RoleId == roleMenuPermissions.RoleId
                    && a.MenuId == roleMenuPermissions.MenuId
                    && a.PermissionId == roleMenuPermissions.PermissionId)
                    .Include(a => a.Role)
                    .Include(a => a.Menu)
                    .Include(a => a.Permission)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
                if (roleMenuPermission == null)
                {
                    _logger.LogWarning("No se encontró el RoleMenuPermission con RoleId {RoleId}, MenuId {MenuId} y PermissionId {PermissionId}", roleMenuPermissions.RoleId, roleMenuPermissions.MenuId, roleMenuPermissions.PermissionId);
                    return null;
                }

                _logger.LogInformation("RoleMenuPermission encontrado: RoleId {RoleId}, MenuId {MenuId} y PermissionId {PermissionId}.", roleMenuPermissions.RoleId, roleMenuPermissions.MenuId, roleMenuPermissions.PermissionId);

                return roleMenuPermission;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el RoleMenuPermission con RoleId {RoleId}, MenuId {MenuId} y PermissionId {PermissionId}.", roleMenuPermissions.RoleId, roleMenuPermissions.MenuId, roleMenuPermissions.PermissionId);
                return null;
            }
        }


        public async Task<RoleMenuPermission> UpdateAsync(RoleMenuPermission roleMenuPermissions)
        {
            if (roleMenuPermissions == null)
            {
                _logger.LogWarning("Intento de actualizar un RoleMenuPermission con datos nulos.");
                throw new ArgumentNullException(nameof(roleMenuPermissions), "El RoleMenuPermission no puede ser nulo.");
            }

            try
            {
                // Buscar el objeto existente sin rastreo para evitar conflictos con EF
                var roleMenuPermissionEntity = await _context.RoleMenuPermissions
                    .AsNoTracking()
                    .FirstOrDefaultAsync(a => a.RoleId == roleMenuPermissions.RoleId
                                           && a.MenuId == roleMenuPermissions.MenuId
                                           && a.PermissionId == roleMenuPermissions.PermissionId);

                if (roleMenuPermissionEntity == null)
                {
                    _logger.LogWarning("No se encontró el RoleMenuPermission con RoleId {RoleId}, ModuleId {ModuleId} y PermissionId {PermissionId} para modificar.",
                        roleMenuPermissions.RoleId, roleMenuPermissions.MenuId, roleMenuPermissions.PermissionId);
                    return null;
                }

                // Mapear los valores actualizados
                roleMenuPermissionEntity.RoleId = roleMenuPermissions.RoleId;
                roleMenuPermissionEntity.MenuId = roleMenuPermissions.MenuId;
                roleMenuPermissionEntity.PermissionId = roleMenuPermissions.PermissionId;
                roleMenuPermissionEntity.UpdatedBy = roleMenuPermissions.UpdatedBy ?? "System"; // Usar un valor por defecto si es nulo
                roleMenuPermissionEntity.UpdatedAt = DateTime.UtcNow;
                roleMenuPermissionEntity.IsActive = roleMenuPermissions.IsActive;

                // Actualizar la entidad en la base de datos
                _context.RoleMenuPermissions.Update(roleMenuPermissionEntity);
                await _context.SaveChangesAsync();

                // Recargar el objeto actualizado con sus relaciones
                var updatedEntity = await _context.RoleMenuPermissions
                    .Include(rmp => rmp.Role)
                    .Include(rmp => rmp.Menu)
                    .Include(rmp => rmp.Permission)
                    .FirstOrDefaultAsync(rmp => rmp.Id == roleMenuPermissionEntity.Id);

                _logger.LogInformation("RoleMenuPermission actualizado exitosamente: RoleId: {RoleId}, MenuId: {MenuId}, PermissionId: {PermissionId}, UpdatedBy: {UpdatedBy}",
                    updatedEntity.RoleId, updatedEntity.MenuId, updatedEntity.PermissionId, updatedEntity.UpdatedBy);

                return updatedEntity;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error de base de datos al actualizar el RoleMenuPermission con RoleId {RoleId}, MenuId {MenuId} y PermissionId {PermissionId}.",
                    roleMenuPermissions.RoleId, roleMenuPermissions.MenuId, roleMenuPermissions.PermissionId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al actualizar el RoleMenuPermission con RoleId {RoleId}, MenuId {MenuId} y PermissionId {PermissionId}.",
                    roleMenuPermissions.RoleId, roleMenuPermissions.MenuId, roleMenuPermissions.PermissionId);
                throw;
            }
        }
    }
}