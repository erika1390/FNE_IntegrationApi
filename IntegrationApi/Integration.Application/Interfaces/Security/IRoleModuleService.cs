using Integration.Shared.DTO.Security;
using System.Linq.Expressions;

namespace Integration.Application.Interfaces.Security
{
    public interface IRoleModuleService
    {
        Task<RoleModuleDTO> CreateAsync(RoleModuleDTO roleModuleDTO);
        //Task<bool> DeactivateAsync(int id);
        Task<IEnumerable<RoleModuleDTO>> GetAllActiveAsync();
        Task<List<RoleModuleDTO>> GetAllAsync(Expression<Func<RoleModuleDTO, bool>> predicado);
        Task<List<RoleModuleDTO>> GetAllAsync(List<Expression<Func<RoleModuleDTO, bool>>> predicados);
        Task<RoleModuleDTO> UpdateAsync(RoleModuleDTO roleModuleDTO);
    }
}