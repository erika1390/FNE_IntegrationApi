using Integration.Shared.DTO.Header;
using Integration.Shared.DTO.Security;

using System.Linq.Expressions;

namespace Integration.Application.Interfaces.Security
{
    public interface IModuleService
    {
        Task<ModuleDTO> CreateAsync(HeaderDTO header, ModuleDTO moduleDTO);
        Task<bool> DeactivateAsync(HeaderDTO header, string code);
        Task<IEnumerable<ModuleDTO>> GetAllActiveAsync();
        Task<List<ModuleDTO>> GetByFilterAsync(Expression<Func<ModuleDTO, bool>> predicate);
        Task<List<ModuleDTO>> GetByMultipleFiltersAsync(List<Expression<Func<ModuleDTO, bool>>> predicates);
        Task<ModuleDTO> GetByCodeAsync(string code);
        Task<ModuleDTO> UpdateAsync(HeaderDTO header, ModuleDTO moduleDTO);
    }
}