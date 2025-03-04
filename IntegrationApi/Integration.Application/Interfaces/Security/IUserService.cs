using Integration.Shared.DTO.Security;
using System.Linq.Expressions;
namespace Integration.Application.Interfaces.Security
{
    public interface IUserService
    {
        Task<UserDTO> CreateAsync(UserDTO userDTO);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<UserDTO>> GetAllActiveAsync();
        Task<List<UserDTO>> GetAllAsync(Expression<Func<UserDTO, bool>> predicado);
        Task<List<UserDTO>> GetAllAsync(List<Expression<Func<UserDTO, bool>>> predicados);
        Task<UserDTO> GetByIdAsync(int id);
        Task<UserDTO> UpdateAsync(UserDTO userDTO);
    }
}