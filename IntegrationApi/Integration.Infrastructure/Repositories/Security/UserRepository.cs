using Integration.Core.Entities.Security;
using Integration.Infrastructure.Data.Contexts;
using Integration.Infrastructure.Interfaces.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Linq.Expressions;
namespace Integration.Infrastructure.Repositories.Security
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserRepository> _logger;
        public UserRepository(ApplicationDbContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<User> CreateAsync(User user)
        {
            if (user == null)
            {
                _logger.LogWarning("Intento de crear un usuario con datos nulos.");
                throw new ArgumentNullException(nameof(user), "El usuario no puede ser nulo.");
            }
            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Usuario creado exitosamente: UserId: {UserId}, Nombre: {UserName}", user.Id, user.UserName);
                return user;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error de base de datos al crear el usuario.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al crear el usuario.");
                throw;
            }
        }
        public async Task<bool> DeactivateAsync(string code, string userName)
        {
            try
            {
                var user = await _context.Users
                    .Where(a => a.Code == code)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
                if (user == null)
                {
                    _logger.LogWarning("No se encontró el usuario con ID {UserId} para eliminar.", code);
                    return false;
                }
                user.IsActive = false;
                user.UpdatedAt = DateTime.UtcNow;
                user.UpdatedBy = userName;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Usuario desactivado: {UserId}", code);
                return true;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error de base de datos al eliminar el usuario con ID {UserId}.", code);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al eliminar el usuario con ID {UserId}.", code);
                return false;
            }
        }
        public async Task<IEnumerable<User>> GetAllActiveAsync()
        {
            try
            {
                var users = await _context.Users.ToListAsync();

                _logger.LogInformation("Se obtuvieron {Count} usuarios de la base de datos.", users.Count);
                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los usuarios.");
                return Enumerable.Empty<User>();
            }
        }
        public async Task<List<User>> GetAllAsync(Expression<Func<User, bool>> predicate)
        {
            try
            {
                _logger.LogInformation("Obteniendo usuarios con un predicado específico.");
                var users = await _context.Users
                    .Where(predicate)
                    .ToListAsync();
                _logger.LogInformation("Se obtuvieron {Count} usuarios.", users.Count);
                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los usuarios con el predicado especificado.");
                throw;
            }
        }
        public async Task<List<User>> GetByMultipleFiltersAsync(List<Expression<Func<User, bool>>> predicates)
        {
            try
            {
                _logger.LogInformation("Obteniendo usuarios con múltiples predicados.");
                var query = _context.Users.AsQueryable();
                foreach (var predicado in predicates)
                {
                    query = query.Where(predicado);
                }
                var users = await query.ToListAsync();
                _logger.LogInformation("Se obtuvieron {Count} usuarios tras aplicar múltiples predicados.", users.Count);
                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los usuarios con múltiples predicados.");
                throw;
            }
        }
        public async Task<User> GetByCodeAsync(string code)
        {
            try
            {
                var user = await _context.Users
                    .Where(a => a.Code == code)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
                if (user == null)
                {
                    _logger.LogWarning("No se encontró el usuario con UserCode {UserCode}.", code);
                    return null;
                }

                _logger.LogInformation("Rol encontrado: UserCode: {UserCode}, Nombre: {Name}", user.Code, user.UserName);

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el usuario con UserCode {UserCode}.", code);
                return null;
            }
        }
        public async Task<User> UpdateAsync(User user)
        {
            if (user == null)
            {
                _logger.LogWarning("Intento de actualizar un usuario con datos nulos.");
                throw new ArgumentNullException(nameof(user), "El usuario no puede ser nulo.");
            }
            try
            {
                var existingUser = await _context.Users
                    .Where(a => a.Code == user.Code)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
                if (existingUser == null)
                {
                    _logger.LogWarning("No se encontró el usuario con ID {UserId} para actualizar.", user.Id);
                    return null;
                }
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.DateOfBirth = user.DateOfBirth;
                existingUser.UpdatedBy = user.UpdatedBy;
                existingUser.UpdatedAt = DateTime.UtcNow;
                existingUser.UserName = user.UserName;
                existingUser.NormalizedUserName = user.NormalizedUserName;
                existingUser.Email = user.Email;
                existingUser.NormalizedEmail = user.NormalizedEmail;
                existingUser.PhoneNumber = user.PhoneNumber;
                existingUser.IsActive = user.IsActive;
                _context.Users.Update(existingUser);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Usuario actualizado: {UserId}, Nombre: {FirstName} {LastName}", user.Id, user.FirstName, user.LastName);
                return existingUser;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error de base de datos al actualizar el usuario con ID {UserId}.", user.Id);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al actualizar el usuario con ID {UserId}.", user.Id);
                return null;
            }
        }

        public async Task<string> GetUserNameByCodeAsync(string userCode)
        {
            var user = await _context.Users
            .Where(u => u.Code == userCode)
            .Select(u => u.UserName)
            .FirstOrDefaultAsync();

            return user ?? "unknown";
        }
    }
}
