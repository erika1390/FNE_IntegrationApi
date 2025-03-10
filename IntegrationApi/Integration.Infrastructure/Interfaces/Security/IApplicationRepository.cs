using Integration.Core.Entities.Security;
using Integration.Infrastructure.Interfaces.Base;
namespace Integration.Infrastructure.Interfaces.Security
{
    public interface IApplicationRepository : IRepositoryBase<Application>
    {
        Task<Application> GetByCodeAsync(string code);
        Task<IEnumerable<Application>> GetAllActiveAsync();
    }
}