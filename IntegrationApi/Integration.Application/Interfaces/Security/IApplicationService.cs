using Integration.Shared.DTO.Header;
using Integration.Shared.DTO.Security;
using System.Linq.Expressions;
namespace Integration.Application.Interfaces.Security
{
    public interface IApplicationService
    {
        Task<ApplicationDTO> CreateAsync(HeaderDTO header, ApplicationDTO applicationDTO);
        Task<bool> DeactivateAsync(HeaderDTO header, string code);
        Task<IEnumerable<ApplicationDTO>> GetAllActiveAsync();
        Task<List<ApplicationDTO>> GetAllAsync(Expression<Func<ApplicationDTO, bool>> predicado);
        Task<List<ApplicationDTO>> GetAllAsync(List<Expression<Func<ApplicationDTO, bool>>> predicados);
        Task<ApplicationDTO> GetByCodeAsync(string code);
        Task<ApplicationDTO> UpdateAsync(HeaderDTO header, ApplicationDTO applicationDTO);
    }
}