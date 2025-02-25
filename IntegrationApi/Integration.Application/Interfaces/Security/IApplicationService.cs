using Integration.Shared.DTO.Security;

using System.Linq.Expressions;

namespace Integration.Application.Interfaces.Security
{
    public interface IApplicationService
    {
        Task<ApplicationDTO> CreateAsync(ApplicationDTO applicationDTO);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<ApplicationDTO>> GetAllActiveAsync();
        Task<List<ApplicationDTO>> GetAllAsync(Expression<Func<ApplicationDTO, bool>> predicado);
        Task<List<ApplicationDTO>> GetAllAsync(List<Expression<Func<ApplicationDTO, bool>>> predicados);
        Task<ApplicationDTO> GetByIdAsync(int id);
        Task<ApplicationDTO> UpdateAsync(ApplicationDTO applicationDTO);
    }
}