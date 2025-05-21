using Integration.Shared.DTO.Parametric;

namespace Integration.Application.Interfaces.Parametric
{
    public interface ICityService
    {
        Task<List<CityDTO>> GetByDepartmentIdAsync(int departmentId);
    }
}