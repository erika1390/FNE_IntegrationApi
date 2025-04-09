using Integration.Shared.DTO.Header;
using Integration.Shared.DTO.Security;
using System.Linq.Expressions;

namespace Integration.Application.Interfaces.Security
{
    public interface IRoleModuleService
    {
        Task<RoleMenuPermissionDTO> CreateAsync(HeaderDTO header, RoleMenuPermissionDTO roleModuleDTO);
        Task<bool> DeactivateAsync(HeaderDTO header, RoleMenuPermissionDTO roleModulePermissionsDTO);
        Task<IEnumerable<RoleMenuPermissionDTO>> GetAllActiveAsync();
        Task<List<RoleMenuPermissionDTO>> GetByFilterAsync(Expression<Func<RoleMenuPermissionDTO, bool>> predicado);
        Task<List<RoleMenuPermissionDTO>> GetByMultipleFiltersAsync(List<Expression<Func<RoleMenuPermissionDTO, bool>>> predicados);
        Task<RoleMenuPermissionDTO> UpdateAsync(HeaderDTO header, RoleMenuPermissionDTO roleModuleDTO);
    }
}