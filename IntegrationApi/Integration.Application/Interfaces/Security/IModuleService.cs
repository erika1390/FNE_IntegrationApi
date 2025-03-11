using Integration.Shared.DTO.Security;

using System.Linq.Expressions;

namespace Integration.Application.Interfaces.Security
{
    public interface IModuleService
    {
        Task<ModuleDTO> CreateAsync(ModuleDTO moduleDTO);
        Task<bool> DeleteAsync(string code);
        Task<IEnumerable<ModuleDTO>> GetAllActiveAsync();
        Task<List<ModuleDTO>> GetAllAsync(Expression<Func<ModuleDTO, bool>> predicado);
        Task<List<ModuleDTO>> GetAllAsync(List<Expression<Func<ModuleDTO, bool>>> predicados);
        Task<ModuleDTO> GetByCodeAsync(string code);
        Task<ModuleDTO> UpdateAsync(ModuleDTO moduleDTO);
    }
}