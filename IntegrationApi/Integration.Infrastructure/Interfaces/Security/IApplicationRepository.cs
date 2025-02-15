using Integration.Core.Entities.Security;

namespace Integration.Infrastructure.Interfaces.Security
{
    public interface IApplicationRepository
    {
        Task<IEnumerable<Application>> GetAllAsync();
        Task<Application> GetByIdAsync(int id);
        Task<Application> CreateAsync(Application entity);
        Task<Application> UpdateAsync(Application entity);
        Task<bool> DeleteAsync(int id);
    }
}