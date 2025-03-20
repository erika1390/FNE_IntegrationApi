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
        Task<List<RoleModulePermissionDTO>> GetByFilterAsync(Expression<Func<RoleModulePermissionDTO, bool>> predicado);
        Task<List<RoleModulePermissionDTO>> GetByMultipleFiltersAsync(List<Expression<Func<RoleModulePermissionDTO, bool>>> predicados);
        Task<RoleModulePermissionDTO> UpdateAsync(HeaderDTO header, RoleModulePermissionDTO roleModuleDTO);
    }
}