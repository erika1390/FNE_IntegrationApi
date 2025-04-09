using Integration.Shared.DTO.Header;
using Integration.Shared.DTO.Security;

using System.Linq.Expressions;

namespace Integration.Application.Interfaces.Security
{
    public interface IRoleMenuPermissionService
    {
        Task<RoleMenuPermissionDTO> GetByCodesAsync(RoleMenuPermissionDTO roleModulePermissionsDTO);
        Task<List<RoleMenuPermissionDTO>> GetByFilterAsync(Expression<Func<RoleMenuPermissionDTO, bool>> predicate);
        Task<List<RoleMenuPermissionDTO>> GetByMultipleFiltersAsync(List<Expression<Func<RoleMenuPermissionDTO, bool>>> predicates);
        Task<IEnumerable<RoleMenuPermissionDTO>> GetAllActiveAsync();
        Task<RoleMenuPermissionDTO> CreateAsync(HeaderDTO header, RoleMenuPermissionDTO roleModulePermissionsDTO);
        Task<RoleMenuPermissionDTO> UpdateAsync(HeaderDTO header, RoleMenuPermissionDTO roleModulePermissionsDTO);
        Task<bool> DeactivateAsync(HeaderDTO header, RoleMenuPermissionDTO roleModulePermissionsDTO);
    }
}