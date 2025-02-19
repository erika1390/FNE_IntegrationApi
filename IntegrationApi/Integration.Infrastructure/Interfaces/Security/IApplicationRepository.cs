using Integration.Core.Entities.Security;

namespace Integration.Infrastructure.Interfaces.Security
{
    public interface IApplicationRepository
    {
        Task<IEnumerable<Application>> GetAllAsync();
        Task<Application> GetByIdAsync(int id);
        Task<Application> CreateAsync(Application application);
        Task<Application> UpdateAsync(Application application);
        Task<bool> DeleteAsync(int id);
    }
}