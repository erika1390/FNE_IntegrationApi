using Integration.Core.Entities.Security;
using System.Linq.Expressions;
namespace Integration.Infrastructure.Interfaces.Security
{
    public interface IRoleModulePermissionRepository
    {
        Task<RoleModulePermissions> GetByRoleIdModuleIdPermissionsIdAsync(RoleModulePermissions roleModulePermissions);
        Task<List<RoleModulePermissions>> GetByFilterAsync(Expression<Func<RoleModulePermissions, bool>> predicate);
        Task<List<RoleModulePermissions>> GetByMultipleFiltersAsync(List<Expression<Func<RoleModulePermissions, bool>>> predicates);
        Task<IEnumerable<RoleModulePermissions>> GetAllActiveAsync();
        Task<RoleModulePermissions> CreateAsync(RoleModulePermissions roleModulePermissions);
        Task<RoleModulePermissions> UpdateAsync(RoleModulePermissions roleModulePermissions);
        Task<bool> DeactivateAsync(RoleModulePermissions roleModulePermissions);
    }
}