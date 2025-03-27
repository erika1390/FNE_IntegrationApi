using Integration.Shared.DTO.Security;

namespace Integration.Application.Interfaces.Security
{
    public interface IUserPermissionService
    {
        Task<IEnumerable<UserPermissionDTO>> GetAllActiveByUserCodeAsync(string userCode, string applicationCode);
    }
}