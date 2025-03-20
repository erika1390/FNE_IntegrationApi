using Integration.Shared.DTO.Header;
using Integration.Shared.DTO.Security;

using System.Linq.Expressions;

namespace Integration.Application.Interfaces.Security
{
    public interface IUserRoleService
    {
        Task<UserRoleDTO> CreateAsync(HeaderDTO header, UserRoleDTO userRole);
        Task<bool> DeactivateAsync(HeaderDTO header, string userCode, string roleCode);
        Task<IEnumerable<UserRoleDTO>> GetAllActiveAsync();
        Task<List<UserRoleDTO>> GetAllAsync(Expression<Func<UserRoleDTO, bool>> predicate);
        Task<List<UserRoleDTO>> GetAllAsync(List<Expression<Func<UserRoleDTO, bool>>> predicates);
        Task<UserRoleDTO> GetByUserCodeRoleCodeAsync(string userCode, string roleCode);
        Task<UserRoleDTO> UpdateAsync(HeaderDTO header, UserRoleDTO userRole);
    }
}
