using Integration.Shared.DTO.Header;
using Integration.Shared.DTO.Security;
using System.Linq.Expressions;
namespace Integration.Application.Interfaces.Security
{
    public interface IRoleService
    {
        Task<RoleDTO> CreateAsync(HeaderDTO header, RoleDTO roleDTO);
        Task<bool> DeactivateAsync(HeaderDTO header, string code);
        Task<IEnumerable<RoleDTO>> GetAllActiveAsync();
        Task<List<RoleDTO>> GetByFilterAsync(Expression<Func<RoleDTO, bool>> predicate);
        Task<List<RoleDTO>> GetByMultipleFiltersAsync(List<Expression<Func<RoleDTO, bool>>> predicates);
        Task<RoleDTO> GetByCodeAsync(string code);
        Task<RoleDTO> UpdateAsync(HeaderDTO header, RoleDTO roleDTO);
    }
}