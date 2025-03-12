using Integration.Core.Entities.Security;
using System.Linq.Expressions;
namespace Integration.Infrastructure.Interfaces.Security
{
    public interface IRoleRepository
    {
        Task<Role> CreateAsync(Role role);
        Task<bool> DeactivateAsync(string code);
        Task<IEnumerable<Role>> GetAllActiveAsync();
        Task<List<Role>> GetAllAsync(Expression<Func<Role, bool>> predicado);
        Task<List<Role>> GetAllAsync(List<Expression<Func<Role, bool>>> predicados);
        Task<Role> GetByCodeAsync(string code);
        Task<Role> UpdateAsync(Role role);
    }
}