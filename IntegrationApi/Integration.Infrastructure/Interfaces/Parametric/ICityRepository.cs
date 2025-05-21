using Integration.Core.Entities.Parametric;

namespace Integration.Infrastructure.Interfaces.Parametric
{
    public interface ICityRepository
    {
        Task<List<City>> GetByDepartmentIdAsync(int departmentId);
    }
}