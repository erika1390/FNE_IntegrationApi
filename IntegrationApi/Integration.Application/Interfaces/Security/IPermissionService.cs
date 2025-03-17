using Integration.Shared.DTO.Header;
using Integration.Shared.DTO.Security;

using System.Linq.Expressions;

namespace Integration.Application.Interfaces.Security
{
    public interface IPermissionService
    {
        Task<PermissionDTO> CreateAsync(HeaderDTO header, PermissionDTO permissionDTO);
        Task<bool> DeactivateAsync(HeaderDTO header, string code);
        Task<IEnumerable<PermissionDTO>> GetAllActiveAsync();
        Task<List<PermissionDTO>> GetAllAsync(Expression<Func<PermissionDTO, bool>> predicado);
        Task<List<PermissionDTO>> GetAllAsync(List<Expression<Func<PermissionDTO, bool>>> predicados);
        Task<PermissionDTO> GetByCodeAsync(string code);
        Task<PermissionDTO> UpdateAsync(HeaderDTO header, PermissionDTO permissionDTO);
    }
}