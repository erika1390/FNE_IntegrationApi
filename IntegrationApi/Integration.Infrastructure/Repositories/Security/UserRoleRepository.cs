using Integration.Core.Entities.Security;
using Integration.Infrastructure.Data.Contexts;
using Integration.Infrastructure.Interfaces.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System;
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

        public Task<bool> DeactivateAsync(string userCode, string roleCode, string userName)
        {
            throw new NotImplementedException();
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

        public Task<List<UserRole>> GetAllAsync(List<Expression<Func<UserRole, bool>>> predicates)
        {
            throw new NotImplementedException();
        }

        public Task<UserRole> GetByUserCodeRoleCodeAsync(string userCode, string roleCode)
        {
            throw new NotImplementedException();
        }

        public Task<UserRole> UpdateAsync(UserRole userRole)
        {
            throw new NotImplementedException();
        }
    }
}