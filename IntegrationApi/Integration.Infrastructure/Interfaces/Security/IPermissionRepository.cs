using Integration.Core.Entities.Security;
using Integration.Infrastructure.Interfaces.Base;
namespace Integration.Infrastructure.Interfaces.Security
{
    public interface IPermissionRepository : IRepositoryBase<Permission>
    {
        Task<IEnumerable<Permission>> GetAllActiveAsync();
    }
}