using Integration.Core.Entities.Security;
using System.Linq.Expressions;
namespace Integration.Infrastructure.Interfaces.Security
{
    public interface IUserRepository
    {
        Task<User> CreateAsync(User user);
        Task<bool> DeactivateAsync(string code);
        Task<IEnumerable<User>> GetAllActiveAsync();
        Task<List<User>> GetAllAsync(Expression<Func<User, bool>> predicado);
        Task<List<User>> GetAllAsync(List<Expression<Func<User, bool>>> predicados);
        Task<User> GetByCodeAsync(string code);
        Task<User> UpdateAsync(User user);
    }
}