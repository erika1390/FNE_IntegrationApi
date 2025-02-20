using Integration.Shared.DTO.Security;

namespace Integration.Application.Interfaces.Security
{
    public interface IPermissionService
    {
        Task<IEnumerable<PermissionDTO>> GetAllAsync();
        Task<PermissionDTO> GetByIdAsync(int id);
        Task<PermissionDTO> CreateAsync(PermissionDTO permissionDTO);
        Task<PermissionDTO> UpdateAsync(PermissionDTO permissionDTO);
        Task<bool> DeleteAsync(int id);
    }
}