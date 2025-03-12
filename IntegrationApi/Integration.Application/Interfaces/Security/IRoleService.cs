using Integration.Shared.DTO.Security;
using System.Linq.Expressions;
namespace Integration.Application.Interfaces.Security
{
    public interface IRoleService
    {
        Task<RoleDTO> CreateAsync(RoleDTO roleDTO);
        Task<bool> DeactivateAsync(string code);
        Task<IEnumerable<RoleDTO>> GetAllActiveAsync();
        Task<List<RoleDTO>> GetAllAsync(Expression<Func<RoleDTO, bool>> predicado);
        Task<List<RoleDTO>> GetAllAsync(List<Expression<Func<RoleDTO, bool>>> predicados);
        Task<RoleDTO> GetByCodeAsync(string code);
        Task<RoleDTO> UpdateAsync(RoleDTO roleDTO);
    }
}