using Integration.Infrastructure.Data.Contexts;
using Integration.Infrastructure.Interfaces.Security;
using Integration.Infrastructure.Interfaces.UnitOfWork;
using Integration.Infrastructure.Repositories.Security;
using Microsoft.Extensions.Logging;
namespace Integration.Infrastructure.Repositories.UnitOfWork
{
    public class ApplicationDbUOW : IApplicationDbUOW
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        private readonly IApplicationRepository _applicationRepository;
        private readonly IModuleRepository _moduleRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IRoleRepository _roleRepository;
        public ApplicationDbUOW(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;

        }
        public IApplicationRepository ApplicationRepository => _applicationRepository ?? new ApplicationRepository(_context, _logger);

        public IModuleRepository ModuleRepository => _moduleRepository??new ModuleRepository(_context, _logger);

        public IPermissionRepository PermissionRepository => _permissionRepository??new PermissionRepository(_context, _logger);

        public IRoleRepository RoleRepository => _roleRepository ??new RoleRepository(_context, _logger);

        public async void Dispose()
        {
            throw new NotImplementedException();
        }

        public async void SaveChanges()
        {
            throw new NotImplementedException();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
