using Integration.Core.Entities.Security;
using Integration.Infrastructure.Data.Contexts;
using Integration.Infrastructure.Interfaces.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System;
using System.Linq;
using System.Linq.Expressions;
namespace Integration.Infrastructure.Repositories.Security
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserRoleRepository> _logger;
        public UserRoleRepository(ApplicationDbContext context, ILogger<UserRoleRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<UserRole> CreateAsync(UserRole userRole)
        {
            if (userRole == null)
            {
                _logger.LogWarning("Intento de crear un userRole con datos nulos.");
                throw new ArgumentNullException(nameof(userRole), "El userRole no puede ser nulo.");
            }
            try
            {
                await _context.UserRoles.AddAsync(userRole);
                await _context.SaveChangesAsync();

                var createdEntity = await _context.UserRoles
                   .Include(rmp => rmp.Role)
                   .Include(rmp => rmp.User)
                   .FirstOrDefaultAsync(rmp => rmp.Id == userRole.Id);

                _logger.LogInformation("UserRole creado exitosamente: RoleId: {RoleId}, UserId: {UserId}", userRole.RoleId, userRole.UserId);
                return userRole;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error de base de datos al crear el userRole.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al crear el userRole.");
                throw;
            }
        }

        public async Task<bool> DeactivateAsync(int userId, int roleId, string userName)
        {
            try
            {
                var userRole = await _context.UserRoles
                    .Where(a => a.UserId == userId
                    && a.RoleId == roleId)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
                if (userRole == null)
                {
                    _logger.LogWarning("No se encontró el UserRole con userId {userId}, roleId {roleId} para desactivar.", userId, roleId);
                    return false;
                }
                userRole.IsActive = false;
                userRole.UpdatedAt = DateTime.UtcNow;
                userRole.UpdatedBy = userName;
                _context.UserRoles.Update(userRole);
                await _context.SaveChangesAsync();

                _logger.LogInformation("UserRole desactivado: userId {userId}, roleId {roleId}", userId, roleId);
                return true;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error de base de datos al desactivar el UserRole con userId {userId}, roleId {roleId}.", userId, roleId);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error de base de datos al desactivar el UserRole con userId {userId}, roleId {roleId}.", userId, roleId);
                return false;
            }
        }

        public async Task<IEnumerable<UserRole>> GetAllActiveAsync()
        {
            try
            {
                var userRoles = await _context.UserRoles
                    .Include(rm => rm.Role)
                    .Include(rm => rm.User)
                    .Where(rm => rm.IsActive)
                    .AsNoTracking()
                    .ToListAsync();

                _logger.LogInformation("Se obtuvieron {Count} UserRoles activos.", userRoles.Count);
                return userRoles;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los UserRoles activos.");
                return Enumerable.Empty<UserRole>();
            }
        }

        public async Task<List<UserRole>> GetAllAsync(Expression<Func<UserRole, bool>> predicate)
        {
            try
            {
                var userRoles = await _context.UserRoles
                    .Include(rm => rm.Role)
                    .Include(rm => rm.User)
                    .Where(predicate)
                    .AsNoTracking()
                    .ToListAsync();

                _logger.LogInformation("Se obtuvieron {Count} UserRoles con el criterio de búsqueda.", userRoles.Count);
                return userRoles;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener UserRole con criterio de búsqueda.");
                return new List<UserRole>();
            }
        }

        public async Task<List<UserRole>> GetAllAsync(List<Expression<Func<UserRole, bool>>> predicates)
        {
            try
            {
                _logger.LogInformation("Obteniendo UserRole con múltiples predicados.");
                var query = _context.UserRoles.AsQueryable();
                foreach (var predicado in predicates)
                {
                    query = query
                        .Where(predicado)
                        .Include(rm => rm.Role)
                        .Include(rm => rm.User)
                        .AsNoTracking();
                }
                var modules = await query.ToListAsync();
                _logger.LogInformation("Se obtuvieron {Count} modulos tras aplicar múltiples predicados.", modules.Count);
                return modules;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los UserRole con múltiples predicados.");
                throw;
            }
        }

        public async Task<UserRole> GetByUserIdRoleIdAsync(int userId, int roleId)
        {
            try
            {
                var userRole = await _context.UserRoles
                .Where(a => a.UserId==userId
                && a.RoleId == roleId)
                .Include(rm => rm.Role)
                .Include(rm => rm.User)
                .AsNoTracking()
                .FirstOrDefaultAsync();
                if (userRole == null)
                {
                    _logger.LogWarning("No se encontró el userRole con userId {userId}, roleId {roleId}.", userId, roleId);
                    return null;
                }

                _logger.LogInformation("UserRole encontrado: userId {userId}, roleId {roleId}.", userId, roleId);

                return userRole;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el UserRole con userId {userId}, roleId {roleId}.", userId, roleId);
                return null;
            }
        }

        public async Task<UserRole> UpdateAsync(UserRole userRole)
        {
            if (userRole == null)
            {
                _logger.LogWarning("Intento de actualizar un UserRole con datos nulos.");
                throw new ArgumentNullException(nameof(userRole), "El UserRole no puede ser nulo.");
            }
            try
            {
                var userRoleEntity = await _context.UserRoles.FindAsync(userRole.Id);
                if (userRoleEntity == null)
                {
                    _logger.LogWarning("No se encontró el UserRole  con userId {userId}, roleId {roleId} para actualizar.", userRole.UserId, userRole.RoleId);
                    return null;
                }
                userRoleEntity.UpdatedBy = userRole.UpdatedBy;
                userRoleEntity.UpdatedAt = DateTime.UtcNow;
                userRoleEntity.IsActive = userRole.IsActive;
                _context.UserRoles.Update(userRoleEntity);
                await _context.SaveChangesAsync();

                var updatedEntity = await _context.UserRoles
                   .Include(rmp => rmp.Role)
                   .Include(rmp => rmp.User)
                   .FirstOrDefaultAsync(rmp => rmp.Id == userRoleEntity.Id);

                _logger.LogInformation("UserRole actualizado: UserId {userId}, RoleId {roleId}", userRoleEntity.UserId, userRoleEntity.RoleId);
                return userRoleEntity;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error de base de datos al actualizar el UserRole con UserId {userId}, RoleId {roleId}.", userRole.UserId, userRole.RoleId);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al actualizar el UserRole con UserId {userId}, RoleId {roleId}.", userRole.UserId, userRole.RoleId);
                return null;
            }
        }
    }
}