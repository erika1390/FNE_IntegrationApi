using Integration.Infrastructure.Data.Contexts;
using Integration.Infrastructure.Interfaces.Audit;
using Integration.Infrastructure.Interfaces.Security;
using Integration.Infrastructure.Interfaces.UnitOfWork;
using Microsoft.Extensions.Logging;
namespace Integration.Infrastructure.Repositories.UnitOfWork
{
    public class ApplicationDbUOW : IApplicationDbUOW, IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ApplicationDbUOW> _logger;

        public ILogRepository LogRepository { get; }
        public IApplicationRepository ApplicationRepository { get; }
        public IModuleRepository ModuleRepository { get; }
        public IPermissionRepository PermissionRepository { get; }
        public IRoleRepository RoleRepository { get; }
        public IMenuRepository MenuRepository { get; }
        public IRoleMenuPermissionRepository RoleMenuPermissionRepository { get; }
        public IUserPermissionRepository UserPermissionRepository { get; set; } // Assuming you have this repository as well
        public IUserRepository UserRepository { get; set; } // Assuming you have this repository as well
        public IUserRoleRepository RoleUserRepository { get; set; } // Assuming you have this repository as well

        public ApplicationDbUOW(
            ApplicationDbContext context,
            ILogger<ApplicationDbUOW> logger,
            ILogRepository logRepository,
            IApplicationRepository applicationRepository,
            IModuleRepository moduleRepository,
            IPermissionRepository permissionRepository,
            IRoleRepository roleRepository,
            IMenuRepository menuRepository,
            IRoleMenuPermissionRepository roleMenuPermissionRepository,
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
