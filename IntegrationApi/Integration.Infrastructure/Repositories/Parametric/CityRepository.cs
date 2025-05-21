using Integration.Core.Entities.Parametric;
using Integration.Infrastructure.Data.Contexts;
using Integration.Infrastructure.Interfaces.Parametric;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Integration.Infrastructure.Repositories.Parametric
{
    public class CityRepository : ICityRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CityRepository> _logger;

        public CityRepository(ApplicationDbContext context, ILogger<CityRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<City>> GetByDepartmentIdAsync(int departmentId)
        {
            try
            {
                var cities = await _context.Cities
                    .Where(c => c.IsActive && c.DepartmentId == departmentId)
                    .ToListAsync();

                _logger.LogInformation("Se obtuvieron {Count} ciudades del departamento {DepartmentId}.", cities.Count, departmentId);
                return cities;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las ciudades del departamento {DepartmentId}.", departmentId);
                return new List<City>();
            }
        }
    }
}