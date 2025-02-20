using Integration.Shared.DTO.Security;

namespace Integration.Application.Interfaces.Security
{
    public interface IApplicationService
    {
        Task<IEnumerable<ApplicationDTO>> GetAllAsync();
        Task<ApplicationDTO> GetByIdAsync(int id);
        Task<ApplicationDTO> CreateAsync(ApplicationDTO applicationDTO);
        Task<ApplicationDTO> UpdateAsync(ApplicationDTO applicationDTO);
        Task<bool> DeleteAsync(int id);
    }
}