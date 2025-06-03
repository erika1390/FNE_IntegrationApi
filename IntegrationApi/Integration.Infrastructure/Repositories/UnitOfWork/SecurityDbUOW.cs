using Integration.Infrastructure.Data.Contexts;
using Integration.Infrastructure.Interfaces.Audit;
using Integration.Infrastructure.Interfaces.Security;
using Integration.Infrastructure.Interfaces.UnitOfWork;

using Microsoft.Extensions.Logging;
namespace Integration.Infrastructure.Repositories.UnitOfWork
{
    public class SecurityDbUOW : ISecurityDbUOW, IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SecurityDbUOW> _logger;
        public ILogRepository LogRepository { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IApplicationRepository ApplicationRepository { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IMenuRepository MenuRepository { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IModuleRepository ModuleRepository { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IPermissionRepository PermissionRepository { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IRoleRepository RoleRepository { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IRoleMenuPermissionRepository RoleMenuPermissionRepository { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IUserPermissionRepository UserPermissionRepository { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IUserRepository UserRepository { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IUserRoleRepository RoleUserRepository { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public SecurityDbUOW(
            ApplicationDbContext context,
            ILogger<SecurityDbUOW> logger,
            ILogRepository logRepository,
            IApplicationRepository applicationRepository,
            IMenuRepository menuRepository,
            IModuleRepository moduleRepository,
            IPermissionRepository permissionRepository,
            IRoleMenuPermissionRepository roleMenuPermissionRepository,
            IRoleRepository roleRepository,            
            IUserPermissionRepository userPermissionRepository,
            IUserRepository userRepository,
            IUserRoleRepository roleUserRepository)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            LogRepository = logRepository ?? throw new ArgumentNullException(nameof(logRepository));
            ApplicationRepository = applicationRepository ?? throw new ArgumentNullException(nameof(applicationRepository));
            ModuleRepository = moduleRepository ?? throw new ArgumentNullException(nameof(moduleRepository));
            PermissionRepository = permissionRepository ?? throw new ArgumentNullException(nameof(permissionRepository));
            RoleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
            MenuRepository = menuRepository ?? throw new ArgumentNullException(nameof(menuRepository));
            RoleMenuPermissionRepository = roleMenuPermissionRepository ?? throw new ArgumentNullException(nameof(roleMenuPermissionRepository));
            UserPermissionRepository = userPermissionRepository ?? throw new ArgumentNullException(nameof(userPermissionRepository));
            UserRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            RoleUserRepository = roleUserRepository ?? throw new ArgumentNullException(nameof(roleUserRepository));
        }

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                _logger.LogInformation("Guardando cambios en la base de datos.");
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar cambios en la base de datos.");
                throw;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
