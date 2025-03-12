using Integration.Shared.DTO.Security;
using System.Linq.Expressions;
namespace Integration.Application.Interfaces.Security
{
    public interface IUserService
    {
        Task<UserDTO> CreateAsync(UserDTO userDTO);
        Task<bool> DeactivateAsync(string code);
        Task<IEnumerable<UserDTO>> GetAllActiveAsync();
        Task<List<UserDTO>> GetAllAsync(Expression<Func<UserDTO, bool>> predicado);
        Task<List<UserDTO>> GetAllAsync(List<Expression<Func<UserDTO, bool>>> predicados);
        Task<UserDTO> GetByCodeAsync(string code);
        Task<UserDTO> UpdateAsync(UserDTO userDTO);
    }
}