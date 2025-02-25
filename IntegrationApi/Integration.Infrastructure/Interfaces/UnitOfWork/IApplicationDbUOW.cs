using Integration.Infrastructure.Interfaces.Security;
namespace Integration.Infrastructure.Interfaces.UnitOfWork
{
    public interface IApplicationDbUOW : IDisposable
    {
        IApplicationRepository ApplicationRepository { get; }
        IModuleRepository ModuleRepository { get; }
        IPermissionRepository PermissionRepository { get; }
        IRoleRepository RoleRepository { get; }
        void SaveChanges();
        Task SaveChangesAsync();
    }
}