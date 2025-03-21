using Integration.Shared.DTO.Header;
using Integration.Shared.DTO.Security;
using System.Linq.Expressions;
namespace Integration.Application.Interfaces.Security
{
    public interface IUserService
    {
        Task<UserDTO> CreateAsync(HeaderDTO header, UserDTO userDTO);
        Task<bool> DeactivateAsync(HeaderDTO header, string code);
        Task<IEnumerable<UserDTO>> GetAllActiveAsync();
        Task<List<UserDTO>> GetByFilterAsync(Expression<Func<UserDTO, bool>> predicate);
        Task<List<UserDTO>> GetByMultipleFiltersAsync(List<Expression<Func<UserDTO, bool>>> predicates);
        Task<UserDTO> GetByCodeAsync(string code);
        Task<UserDTO> UpdateAsync(HeaderDTO header, UserDTO userDTO);
    }
}