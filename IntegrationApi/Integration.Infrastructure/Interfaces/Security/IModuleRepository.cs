using Integration.Core.Entities.Security;

namespace Integration.Infrastructure.Interfaces.Security
{
    public interface IModuleRepository
    {
        Task<IEnumerable<Module>> GetAllAsync();
        Task<Module> GetByIdAsync(int id);
        Task<Module> CreateAsync(Module module);
        Task<Module> UpdateAsync(Module module);
        Task<bool> DeleteAsync(int id);
    }
}