using Integration.Shared.DTO.Header;
using Integration.Shared.DTO.Security;
using System.Linq.Expressions;

namespace Integration.Application.Interfaces.Security
{
    public interface IRoleModuleService
    {
        Task<RoleModulePermissionDTO> CreateAsync(HeaderDTO header, RoleModulePermissionDTO roleModuleDTO);
        Task<bool> DeactivateAsync(HeaderDTO header, RoleModulePermissionDTO roleModulePermissionsDTO);
        Task<IEnumerable<RoleModulePermissionDTO>> GetAllActiveAsync();
        Task<List<RoleModulePermissionDTO>> GetAllAsync(Expression<Func<RoleModulePermissionDTO, bool>> predicate);
        Task<List<RoleModulePermissionDTO>> GetAllAsync(List<Expression<Func<RoleModulePermissionDTO, bool>>> predicates);
        Task<RoleModulePermissionDTO> UpdateAsync(HeaderDTO header, RoleModulePermissionDTO roleModuleDTO);
    }
}