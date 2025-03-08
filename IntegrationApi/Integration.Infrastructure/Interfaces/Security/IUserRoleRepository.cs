using Integration.Core.Entities.Security;
using System.Linq.Expressions;
namespace Integration.Infrastructure.Interfaces.Security
{
    public interface IUserRoleRepository
    {
        Task<UserRole> CreateAsync(UserRole userRole);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<UserRole>> GetAllActiveAsync();
        Task<List<UserRole>> GetAllAsync(Expression<Func<UserRole, bool>> predicate);
        Task<List<UserRole>> GetAllAsync(List<Expression<Func<UserRole, bool>>> predicates);
        Task<UserRole> GetByIdAsync(int id);
        Task<UserRole> UpdateAsync(UserRole userRole);
    }
}