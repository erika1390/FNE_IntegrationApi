using Integration.Core.Entities.Base;

namespace Integration.Infrastructure.Interfaces.Base
{
    public interface IRepositoryBaseSelect<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllActiveAsync();
    }
}