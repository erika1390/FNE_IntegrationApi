using Integration.Core.Entities.Security;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Infrastructure.Interfaces.Security
{
    public interface IUserRepository
    {
        Task<User> CreateAsync(User user);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<User>> GetAllActiveAsync();
        Task<List<User>> GetAllAsync(Expression<Func<User, bool>> predicado);
        Task<List<User>> GetAllAsync(List<Expression<Func<User, bool>>> predicados);
        Task<User> GetByIdAsync(int id);
        Task<User> UpdateAsync(User user);
    }
}