using Integration.Core.Entities.Security;
using System.Linq.Expressions;
namespace Integration.Infrastructure.Interfaces.Security
{
    public interface IUserRoleRepository
    {
        Task<UserRole> CreateAsync(UserRole userRole);
        Task<bool> DeactivateAsync(int userId, int roleId, string userName);
        Task<IEnumerable<UserRole>> GetAllActiveAsync();
        Task<List<UserRole>> GetByFilterAsync(Expression<Func<UserRole, bool>> predicate);
        Task<List<UserRole>> GetByMultipleFiltersAsync(List<Expression<Func<UserRole, bool>>> predicates);
        Task<UserRole> GetByUserIdRoleIdAsync(int userId, int roleId);
        Task<UserRole> UpdateAsync(UserRole userRole);
    }
}