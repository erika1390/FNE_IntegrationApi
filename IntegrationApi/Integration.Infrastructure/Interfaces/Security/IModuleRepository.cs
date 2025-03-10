using Integration.Core.Entities.Security;
using Integration.Infrastructure.Interfaces.Base;
namespace Integration.Infrastructure.Interfaces.Security
{
    public interface IModuleRepository : IRepositoryBase<Module>
    {
        Task<Module> GetByCodeAsync(string code);
        Task<IEnumerable<Module>> GetAllActiveAsync();
    }
}