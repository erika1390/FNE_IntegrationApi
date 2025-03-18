using Integration.Core.Entities.Security;
using System.Linq.Expressions;
namespace Integration.Infrastructure.Interfaces.Security
{
    public interface IUserRoleRepository
    {
        Task<UserRole> CreateAsync(UserRole userRole);
        Task<bool> DeactivateAsync(string userCode, string roleCode, string userName);
        Task<IEnumerable<UserRole>> GetAllActiveAsync();
        Task<List<UserRole>> GetAllAsync(Expression<Func<UserRole, bool>> predicate);
        Task<List<UserRole>> GetAllAsync(List<Expression<Func<UserRole, bool>>> predicates);
        Task<UserRole> GetByUserCodeRoleCodeAsync(string userCode, string roleCode);
        Task<UserRole> UpdateAsync(UserRole userRole);
    }
}