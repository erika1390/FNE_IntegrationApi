using Integration.Core.Entities.Base;

using System.Linq.Expressions;

namespace Integration.Infrastructure.Interfaces.Base
{
    public interface IRepositoryBase<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int id);
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicado);
        Task<List<T>> GetAllAsync(List<Expression<Func<T, bool>>> predicados);
        Task<T> CreateAsync(T entidad);
        Task<T> UpdateAsync(T entidad);
        Task<bool> DeleteAsync(int id);
    }
}