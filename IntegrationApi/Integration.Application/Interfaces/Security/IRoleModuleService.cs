using Integration.Shared.DTO.Header;
using Integration.Shared.DTO.Security;
using System.Linq.Expressions;

namespace Integration.Application.Interfaces.Security
{
    public interface IRoleModuleService
    {
        Task<RoleModulePermissionsDTO> CreateAsync(HeaderDTO header, RoleModulePermissionsDTO roleModuleDTO);
        Task<bool> DeactivateAsync(HeaderDTO header, RoleModulePermissionsDTO roleModulePermissionsDTO);
        Task<IEnumerable<RoleModulePermissionsDTO>> GetAllActiveAsync();
        Task<List<RoleModulePermissionsDTO>> GetAllAsync(Expression<Func<RoleModulePermissionsDTO, bool>> predicado);
        Task<List<RoleModulePermissionsDTO>> GetAllAsync(List<Expression<Func<RoleModulePermissionsDTO, bool>>> predicados);
        Task<RoleModulePermissionsDTO> UpdateAsync(HeaderDTO header, RoleModulePermissionsDTO roleModuleDTO);
    }
}