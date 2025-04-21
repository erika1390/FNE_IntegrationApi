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
        public async Task<UserPermissionDTO> GetAllPermissionsByUserCodeAsync(string userCode, int applicationId)
        {
            try
            {
                var flatPermissions = await (
                    from rmp in _context.RoleMenuPermissions
                    join ur in _context.UserRoles on rmp.RoleId equals ur.RoleId
                    join r in _context.Roles on rmp.RoleId equals r.Id
                    join me in _context.Menus on rmp.MenuId equals me.Id
                    join p in _context.Permissions on rmp.PermissionId equals p.Id
                    join u in _context.Users on ur.UserId equals u.Id
                    join m in _context.Modules on me.ModuleId equals m.Id
                    where r.ApplicationId == applicationId
                          && m.ApplicationId == applicationId
                          && r.IsActive
                          && m.IsActive
                          && p.IsActive
                          && rmp.IsActive
                          && ur.IsActive
                          && u.Code == userCode
                    select new 
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

                var userPermissionDto = new UserPermissionDTO
                {
                    CodeUser = flatPermissions.FirstOrDefault()?.CodeUser,
                    UserName = flatPermissions.FirstOrDefault()?.UserName,
                    Roles = flatPermissions
                    .GroupBy(x => new { x.CodeRole, x.Role })
                    .Select(roleGroup => new RoleDto
                    {
                        Code = roleGroup.Key.CodeRole,
                        Name = roleGroup.Key.Role,
                        Modules = roleGroup
                            .GroupBy(x => new { x.CodeModule, x.Module })
                            .Select(moduleGroup => new ModuleDto
                            {
                                Code = moduleGroup.Key.CodeModule,
                                Name = moduleGroup.Key.Module,
                                Menus = moduleGroup
                                    .GroupBy(x => new { x.CodeMenu, x.Menu })
                                    .Select(menuGroup => new MenuDto
                                    {
                                        Code = menuGroup.Key.CodeMenu,
                                        Name = menuGroup.Key.Menu,
                                        Permissions = menuGroup
                                            .Select(p => new PermissionDto
                                            {
                                                Code = p.CodePermission,
                                                Name = p.Permission
                                            })
                                            .Distinct()
                                            .ToList()
                                    })
                                    .ToList()
                            })
                            .ToList()
                    })
                    .ToList()
                };


                _logger.LogInformation("Se obtuvieron permisos agrupados por módulo para el usuario {UserCode}.", userCode);
                return userPermissionDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener permisos por módulo para el usuario {UserCode}.", userCode);
                return null;
            }
        }
    }
}