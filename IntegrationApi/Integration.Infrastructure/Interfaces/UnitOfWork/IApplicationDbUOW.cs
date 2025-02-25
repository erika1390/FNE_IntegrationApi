using Integration.Infrastructure.Interfaces.Audit;
using Integration.Infrastructure.Interfaces.Security;
namespace Integration.Infrastructure.Interfaces.UnitOfWork
{
    public interface IApplicationDbUOW : IDisposable
    {
        IApplicationRepository ApplicationRepository { get; }
        IModuleRepository ModuleRepository { get; }
        IPermissionRepository PermissionRepository { get; }
        IRoleRepository RoleRepository { get; }
        ILogRepository LogRepository { get; }
        Task<int> SaveChangesAsync();
    }
}
