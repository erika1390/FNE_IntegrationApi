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
        Task<List<PermissionDTO>> GetByFilterAsync(Expression<Func<PermissionDTO, bool>> predicate);
        Task<List<PermissionDTO>> GetByMultipleFiltersAsync(List<Expression<Func<PermissionDTO, bool>>> predicates);
        Task<PermissionDTO> GetByCodeAsync(string code);
        Task<PermissionDTO> UpdateAsync(HeaderDTO header, PermissionDTO permissionDTO);
    }
}