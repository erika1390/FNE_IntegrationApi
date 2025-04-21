using Integration.Shared.DTO.Security;

namespace Integration.Application.Interfaces.Security
{
    public interface IUserPermissionService
    {
        Task<UserPermissionDTO> GetAllPermissionsByUserCodeAsync(string userCode, string applicationCode);
    }
}