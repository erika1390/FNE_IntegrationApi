using Integration.Infrastructure.Interfaces.Audit;
using Integration.Infrastructure.Interfaces.Security;
namespace Integration.Infrastructure.Interfaces.UnitOfWork
{
    public interface ISecurityDbUOW : IDisposable
    {
        ILogRepository LogRepository { get; set; }
        IApplicationRepository ApplicationRepository { get; set; }
        IMenuRepository MenuRepository { get; set; }
        IModuleRepository ModuleRepository { get; set; }
        IPermissionRepository PermissionRepository { get; set; }
        IRoleRepository RoleRepository { get; set; }
        IRoleMenuPermissionRepository RoleMenuPermissionRepository { get; set; }
        IUserPermissionRepository UserPermissionRepository { get; set; }
        IUserRepository UserRepository { get; set; }
        IUserRoleRepository RoleUserRepository { get; set; }
        Task<int> SaveChangesAsync();
    }
}
