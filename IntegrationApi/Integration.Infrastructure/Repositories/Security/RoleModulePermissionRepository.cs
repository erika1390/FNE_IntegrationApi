using Integration.Core.Entities.Security;
using Integration.Infrastructure.Data.Contexts;
using Integration.Infrastructure.Interfaces.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Linq.Expressions;
namespace Integration.Infrastructure.Repositories.Security
{
    public class RoleModulePermissionRepository : IRoleModulePermissionRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RoleModulePermissionRepository> _logger;
        public RoleModulePermissionRepository(ApplicationDbContext context, ILogger<RoleModulePermissionRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<RoleModulePermissions> CreateAsync(RoleModulePermissions roleModulePermissions)
        {
            if (roleModulePermissions == null)
            {
                _logger.LogWarning("Intento de crear un RoleModulePermissions con datos nulos.");
                throw new ArgumentNullException(nameof(roleModulePermissions), "El RoleModulePermissions no puede ser nulo.");
            }

            try
            {
                // Evitar que Entity Framework intente insertar relaciones
                roleModulePermissions.Role = null;
                roleModulePermissions.Module = null;
                roleModulePermissions.Permission = null;

                // Insertar el nuevo registro
                await _context.RoleModulePermissions.AddAsync(roleModulePermissions);
                await _context.SaveChangesAsync();

                // Recargar el objeto desde la base de datos con sus relaciones
                var createdEntity = await _context.RoleModulePermissions
                    .Include(rmp => rmp.Role)
                    .Include(rmp => rmp.Module)
                    .Include(rmp => rmp.Permission)
                    .FirstOrDefaultAsync(rmp => rmp.Id == roleModulePermissions.Id);

                _logger.LogInformation("RoleModulePermissions creado exitosamente: RoleId: {RoleId}, ModuleId: {ModuleId}, PermissionId: {PermissionId}, CreatedBy: {CreatedBy}",
                    createdEntity.RoleId, createdEntity.ModuleId, createdEntity.PermissionId, createdEntity.CreatedBy);

                return createdEntity;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error de base de datos al crear el RoleModulePermissions.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al crear el RoleModulePermissions.");
                throw;
            }
        }

        public async Task<bool> DeactivateAsync(RoleModulePermissions roleModulePermissions)
        {
            try
            {
                var roleModulePermission = await _context.RoleModulePermissions
                    .Where(a => a.RoleId == roleModulePermissions.RoleId
                    && a.ModuleId == roleModulePermissions.ModuleId
                    && a.PermissionId == roleModulePermissions.PermissionId)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
                if (roleModulePermission == null)
                {
                    _logger.LogWarning("No se encontró el RoleModulePermissions con RoleId {RoleId}, ModuleId {ModuleId} y PermissionId {PermissionId} para eliminar.", roleModulePermissions.RoleId, roleModulePermissions.ModuleId, roleModulePermissions.PermissionId);
                    return false;
                }
                roleModulePermission.IsActive = false;
                roleModulePermission.UpdatedAt = DateTime.UtcNow;
                _context.RoleModulePermissions.Update(roleModulePermission);
                await _context.SaveChangesAsync();
                _logger.LogInformation("RoleModulePermissions desactivado: {RoleId}, ModuleId {ModuleId} y PermissionId {PermissionId} para eliminar.", roleModulePermissions.RoleId, roleModulePermissions.ModuleId, roleModulePermissions.PermissionId);
                return true;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error de base de datos al eliminar el RoleModulePermissions con {RoleId}, ModuleId {ModuleId} y PermissionId {PermissionId} para eliminar.", roleModulePermissions.RoleId, roleModulePermissions.ModuleId, roleModulePermissions.PermissionId);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al eliminar el RoleModulePermissions con {RoleId}, ModuleId {ModuleId} y PermissionId {PermissionId} para eliminar.", roleModulePermissions.RoleId, roleModulePermissions.ModuleId, roleModulePermissions.PermissionId);
                return false;
            }
        }

        public async Task<IEnumerable<RoleModulePermissions>> GetAllActiveAsync()
        {
            try
            {
                var roleModulePermissions = await _context.RoleModulePermissions
                    .Where(r => r.IsActive == true)
                    .Include(a => a.Role)
                    .Include(a => a.Module)
                    .Include(a => a.Permission)
                    .AsNoTracking()
                    .ToListAsync();

                _logger.LogInformation("Se obtuvieron {Count} RoleModulePermissions de la base de datos.", roleModulePermissions.Count);
                return roleModulePermissions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los RoleModulePermissions.");
                return Enumerable.Empty<RoleModulePermissions>();
            }
        }

        public async Task<List<RoleModulePermissions>> GetAllAsync(Expression<Func<RoleModulePermissions, bool>> predicate)
        {
            try
            {
                _logger.LogInformation("Obteniendo RoleModulePermissions con un predicado específico.");
                var roleModulePermissions = await _context.RoleModulePermissions
                    .Where(predicate)
                    .Include(a => a.Role)
                    .Include(a => a.Module)
                    .Include(a => a.Permission)
                    .AsNoTracking()
                    .ToListAsync();
                _logger.LogInformation("Se obtuvieron {Count} RoleModulePermissions.", roleModulePermissions.Count);
                return roleModulePermissions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los RoleModulePermissions con el predicado especificado.");
                throw;
            }
        }

        public async Task<List<RoleModulePermissions>> GetAllAsync(List<Expression<Func<RoleModulePermissions, bool>>> predicates)
        {
            try
            {
                _logger.LogInformation("Obteniendo RoleModulePermissions con múltiples predicados.");
                var query = _context.RoleModulePermissions.AsQueryable();
                foreach (var predicado in predicates)
                {
                    query = query
                        .Where(predicado)
                        .Include(a => a.Role)
                        .Include(a => a.Module)
                        .Include(a => a.Permission);
                }
                var roles = await query.ToListAsync();
                _logger.LogInformation("Se obtuvieron {Count} RoleModulePermissions tras aplicar múltiples predicados.", roles.Count);
                return roles;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los RoleModulePermissions con múltiples predicados.");
                throw;
            }
        }

        public async Task<RoleModulePermissions> GetByRoleIdModuleIdPermissionsIdAsync(RoleModulePermissions roleModulePermissions)
        {
            try
            {
                var roleModulePermission = await _context.RoleModulePermissions
                    .Where(a => a.RoleId == roleModulePermissions.RoleId
                    && a.ModuleId == roleModulePermissions.ModuleId
                    && a.PermissionId == roleModulePermissions.PermissionId)
                    .Include(a => a.Role)
                    .Include(a => a.Module)
                    .Include(a => a.Permission)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
                if (roleModulePermission == null)
                {
                    _logger.LogWarning("No se encontró el RoleModulePermissions con RoleId {RoleId}, ModuleId {ModuleId} y PermissionId {PermissionId}", roleModulePermissions.RoleId, roleModulePermissions.ModuleId, roleModulePermissions.PermissionId);
                    return null;
                }

                _logger.LogInformation("RoleModulePermissions encontrado: RoleId {RoleId}, ModuleId {ModuleId} y PermissionId {PermissionId}.", roleModulePermissions.RoleId, roleModulePermissions.ModuleId, roleModulePermissions.PermissionId);

                return roleModulePermission;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el RoleModulePermissions con RoleId {RoleId}, ModuleId {ModuleId} y PermissionId {PermissionId}.", roleModulePermissions.RoleId, roleModulePermissions.ModuleId, roleModulePermissions.PermissionId);
                return null;
            }
        }

        public async Task<RoleModulePermissions> UpdateAsync(RoleModulePermissions roleModulePermissions)
        {
            if (roleModulePermissions == null)
            {
                _logger.LogWarning("Intento de actualizar un RoleModulePermissions con datos nulos.");
                throw new ArgumentNullException(nameof(roleModulePermissions), "El RoleModulePermissions no puede ser nulo.");
            }

            try
            {
                // Buscar el objeto existente sin rastreo para evitar conflictos con EF
                var roleModulePermissionEntity = await _context.RoleModulePermissions
                    .AsNoTracking()
                    .FirstOrDefaultAsync(a => a.RoleId == roleModulePermissions.RoleId
                                           && a.ModuleId == roleModulePermissions.ModuleId
                                           && a.PermissionId == roleModulePermissions.PermissionId);

                if (roleModulePermissionEntity == null)
                {
                    _logger.LogWarning("No se encontró el RoleModulePermissions con RoleId {RoleId}, ModuleId {ModuleId} y PermissionId {PermissionId} para modificar.",
                        roleModulePermissions.RoleId, roleModulePermissions.ModuleId, roleModulePermissions.PermissionId);
                    return null;
                }

                // Mapear los valores actualizados
                roleModulePermissionEntity.RoleId = roleModulePermissions.RoleId;
                roleModulePermissionEntity.ModuleId = roleModulePermissions.ModuleId;
                roleModulePermissionEntity.PermissionId = roleModulePermissions.PermissionId;
                roleModulePermissionEntity.UpdatedBy = roleModulePermissions.UpdatedBy ?? "System"; // Usar un valor por defecto si es nulo
                roleModulePermissionEntity.UpdatedAt = DateTime.UtcNow;
                roleModulePermissionEntity.IsActive = roleModulePermissions.IsActive;

                // Actualizar la entidad en la base de datos
                _context.RoleModulePermissions.Update(roleModulePermissionEntity);
                await _context.SaveChangesAsync();

                // Recargar el objeto actualizado con sus relaciones
                var updatedEntity = await _context.RoleModulePermissions
                    .Include(rmp => rmp.Role)
                    .Include(rmp => rmp.Module)
                    .Include(rmp => rmp.Permission)
                    .FirstOrDefaultAsync(rmp => rmp.Id == roleModulePermissionEntity.Id);

                _logger.LogInformation("RoleModulePermissions actualizado exitosamente: RoleId: {RoleId}, ModuleId: {ModuleId}, PermissionId: {PermissionId}, UpdatedBy: {UpdatedBy}",
                    updatedEntity.RoleId, updatedEntity.ModuleId, updatedEntity.PermissionId, updatedEntity.UpdatedBy);

                return updatedEntity;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error de base de datos al actualizar el RoleModulePermissions con RoleId {RoleId}, ModuleId {ModuleId} y PermissionId {PermissionId}.",
                    roleModulePermissions.RoleId, roleModulePermissions.ModuleId, roleModulePermissions.PermissionId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al actualizar el RoleModulePermissions con RoleId {RoleId}, ModuleId {ModuleId} y PermissionId {PermissionId}.",
                    roleModulePermissions.RoleId, roleModulePermissions.ModuleId, roleModulePermissions.PermissionId);
                throw;
            }
        }
    }
}