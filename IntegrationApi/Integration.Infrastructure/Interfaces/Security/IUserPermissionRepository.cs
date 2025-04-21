using Integration.Shared.DTO.Security;

namespace Integration.Infrastructure.Interfaces.Security
{
    public interface IUserPermissionRepository
    {
        Task<UserPermissionDTO> GetAllPermissionsByUserCodeAsync(string userCode, int applicationId);
    }
}