using Integration.Core.Entities.Security;
using Integration.Infrastructure.Data.Contexts;
using Integration.Infrastructure.Interfaces.Security;
using Integration.Shared.DTO.Security;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Integration.Infrastructure.Repositories.Security
{
    public class UserPermissionRepository : IUserPermissionRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserPermissionRepository> _logger;
        public UserPermissionRepository(ApplicationDbContext context, ILogger<UserPermissionRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<IEnumerable<UserPermissionDTO>> GetAllActiveByUserIdAsync(string userCode, int applicationId)
        {
            try
            {
                var permissionsByModule = await (
                    from rmp in _context.RoleMenuPermissions
                    join ur in _context.UserRoles on rmp.RoleId equals ur.RoleId
                    join r in _context.Roles on rmp.RoleId equals r.Id
                    join m in _context.Modules on rmp.MenuId equals m.Id
                    join me in _context.Menus on rmp.MenuId equals me.Id
                    join p in _context.Permissions on rmp.PermissionId equals p.Id
                    join u in _context.Users on ur.UserId equals u.Id
                    where r.ApplicationId == applicationId
                          && m.ApplicationId == applicationId
                          && r.IsActive
                          && m.IsActive
                          && p.IsActive
                          && rmp.IsActive
                          && ur.IsActive
                          && u.Code == userCode
                    select new UserPermissionDTO
                    {
                        CodeUser = u.Code,
                        UserName = u.UserName,
                        CodeRole = r.Code,
                        Role = r.Name,
                        CodeModule = m.Code,
                        Module = m.Name,
                        CodeMenu = me.Code,
                        Menu = me.Name,
                        CodePermission = p.Code,
                        Permission = p.Name
                    }
                ).ToListAsync();

                _logger.LogInformation("Se obtuvieron permisos agrupados por módulo para el usuario {UserCode}.", userCode);
                return permissionsByModule;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener permisos por módulo para el usuario {UserCode}.", userCode);
                return Enumerable.Empty<UserPermissionDTO>();
            }
        }
    }
}