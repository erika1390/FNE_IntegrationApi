using Integration.Core.Entities.Security;
using Integration.Shared.DTO.Security;

namespace Integration.Infrastructure.Interfaces.Security
{
    public interface IUserPermissionRepository
    {
        Task<IEnumerable<UserPermissionDTOResponse>> GetAllActiveByUserIdAsync(string userCode, int applicationId);
    }
}