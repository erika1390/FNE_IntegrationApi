using Integration.Core.Entities.Security;
using Integration.Infrastructure.Interfaces.Base;
namespace Integration.Infrastructure.Interfaces.Security
{
    public interface IRoleModuleRepository : IRepositoryBase<RoleModule>
    {
        Task<IEnumerable<RoleModule>> GetAllActiveAsync();
    }
}