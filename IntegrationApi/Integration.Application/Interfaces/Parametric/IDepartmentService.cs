using Integration.Shared.DTO.Parametric;

namespace Integration.Application.Interfaces.Parametric
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentDTO>> GetAllActiveAsync();
    }
}