using Integration.Shared.DTO.Header;
using Integration.Shared.DTO.Security;

using System.Linq.Expressions;

namespace Integration.Application.Interfaces.Security
{
    public interface IMenuService
    {
        Task<MenuDTO> CreateAsync(HeaderDTO header, MenuDTO moduleDTO);
        Task<bool> DeactivateAsync(HeaderDTO header, string code);
        Task<IEnumerable<MenuDTO>> GetAllActiveAsync();
        Task<List<MenuDTO>> GetByFilterAsync(Expression<Func<MenuDTO, bool>> predicate);
        Task<List<MenuDTO>> GetByMultipleFiltersAsync(List<Expression<Func<MenuDTO, bool>>> predicates);
        Task<MenuDTO> GetByCodeAsync(string code);
        Task<MenuDTO> UpdateAsync(HeaderDTO header, MenuDTO moduleDTO);
    }
}
