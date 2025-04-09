using Integration.Core.Entities.Security;

using System.Linq.Expressions;
namespace Integration.Infrastructure.Interfaces.Security
{
    public interface IRoleMenuPermissionRepository
    {
        Task<RoleMenuPermission> GetByRoleIdMenuIdPermissionsIdAsync(RoleMenuPermission roleMenuPermissions);
        Task<List<RoleMenuPermission>> GetByFilterAsync(Expression<Func<RoleMenuPermission, bool>> predicate);
        Task<List<RoleMenuPermission>> GetByMultipleFiltersAsync(List<Expression<Func<RoleMenuPermission, bool>>> predicates);
        Task<IEnumerable<RoleMenuPermission>> GetAllActiveAsync();
        Task<RoleMenuPermission> CreateAsync(RoleMenuPermission roleMenuPermissions);
        Task<RoleMenuPermission> UpdateAsync(RoleMenuPermission roleMenuPermissions);
        Task<bool> DeactivateAsync(RoleMenuPermission roleMenuPermissions);
    }
}