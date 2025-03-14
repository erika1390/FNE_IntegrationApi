using Integration.Shared.DTO.Security;
using System.Linq.Expressions;

namespace Integration.Application.Interfaces.Security
{
    public interface IRoleModuleService
    {
        Task<RoleModulePermissionsDTO> CreateAsync(RoleModulePermissionsDTO roleModuleDTO);
        //Task<bool> DeactivateAsync(int id);
        Task<IEnumerable<RoleModulePermissionsDTO>> GetAllActiveAsync();
        Task<List<RoleModulePermissionsDTO>> GetAllAsync(Expression<Func<RoleModulePermissionsDTO, bool>> predicado);
        Task<List<RoleModulePermissionsDTO>> GetAllAsync(List<Expression<Func<RoleModulePermissionsDTO, bool>>> predicados);
        Task<RoleModulePermissionsDTO> UpdateAsync(RoleModulePermissionsDTO roleModuleDTO);
    }
}