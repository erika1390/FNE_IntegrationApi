using Integration.Shared.DTO.Header;
using Integration.Shared.DTO.Security;

using System.Linq.Expressions;

namespace Integration.Application.Interfaces.Security
{
    public interface IRoleModulePermissionsService
    {
        Task<RoleModulePermissionsDTO> GetByRoleCodeModuleCodePermissionsCodeAsync(RoleModulePermissionsDTO roleModulePermissionsDTO);
        Task<List<RoleModulePermissionsDTO>> GetAllAsync(Expression<Func<RoleModulePermissionsDTO, bool>> predicate);
        Task<List<RoleModulePermissionsDTO>> GetAllAsync(List<Expression<Func<RoleModulePermissionsDTO, bool>>> predicates);
        Task<IEnumerable<RoleModulePermissionsDTO>> GetAllActiveAsync();
        Task<RoleModulePermissionsDTO> CreateAsync(HeaderDTO header, RoleModulePermissionsDTO roleModulePermissionsDTO);
        Task<RoleModulePermissionsDTO> UpdateAsync(HeaderDTO header, RoleModulePermissionsDTO roleModulePermissionsDTO);
        Task<bool> DeactivateAsync(HeaderDTO header, RoleModulePermissionsDTO roleModulePermissionsDTO);
    }
}