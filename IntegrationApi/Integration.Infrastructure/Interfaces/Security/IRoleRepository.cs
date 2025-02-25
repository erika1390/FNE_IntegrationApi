using Integration.Core.Entities.Security;
using System.Linq.Expressions;
namespace Integration.Infrastructure.Interfaces.Security
{
    public interface IRoleRepository
    {
        Task<Role> CreateAsync(Role role);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Role>> GetAllActiveAsync();
        Task<List<Role>> GetAllAsync(Expression<Func<Role, bool>> predicado);
        Task<List<Role>> GetAllAsync(List<Expression<Func<Role, bool>>> predicados);
        Task<Role> GetByIdAsync(int id);
        Task<Role> UpdateAsync(Role role);
    }
}