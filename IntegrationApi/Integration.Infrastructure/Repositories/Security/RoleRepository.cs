using Integration.Core.Entities.Security;
using Integration.Infrastructure.Data.Contexts;
using Integration.Infrastructure.Interfaces.Security;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System.Security;
namespace Integration.Infrastructure.Repositories.Security
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RoleRepository> _logger;
        public RoleRepository(ApplicationDbContext context, ILogger<RoleRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<Role> CreateAsync(Role role)
        {
            if (role == null)
            {
                _logger.LogWarning("Intento de crear un rol con datos nulos.");
                throw new ArgumentNullException(nameof(role), "El rol no puede ser nulo.");
            }
            try
            {
                await _context.Roles.AddAsync(role);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Rol creado exitosamente: RoleId: {RoleId}, Nombre: {Name}", role.Id, role.Name);
                return role;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error de base de datos al crear el rol.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al crear el rol.");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var role = await _context.Roles.FindAsync(id);
                if (role == null)
                {
                    _logger.LogWarning("No se encontró el rol con ID {RolId} para eliminar.", id);
                    return false;
                }
                role.IsActive = false;
                role.UpdatedAt = DateTime.UtcNow;
                _context.Roles.Update(role);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Rol desactivado: {RolId}", id);
                return true;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error de base de datos al eliminar el rol con ID {RolId}.", id);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al eliminar el rol con ID {RolId}.", id);
                return false;
            }
        }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            try
            {
                var roles = await _context.Roles.AsNoTracking().ToListAsync();

                _logger.LogInformation("Se obtuvieron {Count} roles de la base de datos.", roles.Count);
                return roles;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los roles.");
                return Enumerable.Empty<Role>();
            }
        }

        public async Task<Role> GetByIdAsync(int id)
        {
            try
            {
                var role = await _context.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
                if (role == null)
                {
                    _logger.LogWarning("No se encontró el rol con ID {RolId}.", id);
                    return null;
                }

                _logger.LogInformation("Rol encontrado: RolId: {RolId}, Nombre: {Name}", role.Id, role.Name);

                return role;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el rol con ID {RolId}.", id);
                return null;
            }
        }

        public async Task<Role> UpdateAsync(Role role)
        {
            if (role == null)
            {
                _logger.LogWarning("Intento de actualizar un rol con datos nulos.");
                throw new ArgumentNullException(nameof(role), "El rol no puede ser nulo.");
            }
            try
            {
                var roleEntity = await _context.Roles.FindAsync(role.Id);
                if (roleEntity == null)
                {
                    _logger.LogWarning("No se encontró el rol con ID {RolId} para actualizar.", role.Id);
                    return null;
                }
                roleEntity.Name = role.Name;
                roleEntity.UpdatedBy = role.UpdatedBy;
                roleEntity.UpdatedAt = DateTime.UtcNow;
                roleEntity.IsActive = role.IsActive;
                _context.Roles.Update(roleEntity);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Rol actualizado: {RolId}, Nombre: {Name}", role.Id, role.Name);
                return roleEntity;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error de base de datos al actualizar el rol con ID {RolId}.", role.Id);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al actualizar el rol con ID {RolId}.", role.Id);
                return null;
            }
        }
    }
}