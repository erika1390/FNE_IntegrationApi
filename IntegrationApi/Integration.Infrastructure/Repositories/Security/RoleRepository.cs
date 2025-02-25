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
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Role> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Role> UpdateAsync(Role role)
        {
            throw new NotImplementedException();
        }
    }
}