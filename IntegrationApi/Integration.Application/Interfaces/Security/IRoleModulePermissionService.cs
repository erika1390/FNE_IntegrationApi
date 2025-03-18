using Integration.Shared.DTO.Header;
using Integration.Shared.DTO.Security;

using System.Linq.Expressions;

namespace Integration.Application.Interfaces.Security
{
    public interface IRoleModulePermissionService
    {
        Task<RoleModulePermissionDTO> GetByRoleCodeModuleCodePermissionsCodeAsync(RoleModulePermissionDTO roleModulePermissionsDTO);
        Task<List<RoleModulePermissionDTO>> GetAllAsync(Expression<Func<RoleModulePermissionDTO, bool>> predicate);
        Task<List<RoleModulePermissionDTO>> GetAllAsync(List<Expression<Func<RoleModulePermissionDTO, bool>>> predicates);
        Task<IEnumerable<RoleModulePermissionDTO>> GetAllActiveAsync();
        Task<RoleModulePermissionDTO> CreateAsync(HeaderDTO header, RoleModulePermissionDTO roleModulePermissionsDTO);
        Task<RoleModulePermissionDTO> UpdateAsync(HeaderDTO header, RoleModulePermissionDTO roleModulePermissionsDTO);
        Task<bool> DeactivateAsync(HeaderDTO header, RoleModulePermissionDTO roleModulePermissionsDTO);
    }
}