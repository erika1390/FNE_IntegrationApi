using Integration.Shared.DTO.Security;

namespace Integration.Application.Interfaces.Security
{
    public interface IModuleService
    {
        Task<IEnumerable<ModuleDTO>> GetAllAsync();
        Task<ModuleDTO> GetByIdAsync(int id);
        Task<ModuleDTO> CreateAsync(ModuleDTO moduleDTO);
        Task<ModuleDTO> UpdateAsync(ModuleDTO moduleDTO);
        Task<bool> DeleteAsync(int id);
    }
}