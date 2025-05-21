using Integration.Core.Entities.Parametric;
using Integration.Infrastructure.Data.Contexts;
using Integration.Infrastructure.Interfaces.Parametric;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Integration.Infrastructure.Repositories.Parametric
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DepartmentRepository> _logger;

        public DepartmentRepository(ApplicationDbContext context, ILogger<DepartmentRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<IEnumerable<Department>> GetAllActiveAsync()
        {
            try
            {
                var departments = await _context.Departments.Where(a => a.IsActive == true).ToListAsync();
                _logger.LogInformation("Se obtuvieron {Count} departamentos de la base de datos.", departments.Count);
                return departments;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los departamentos.");
                return Enumerable.Empty<Department>();
            }
        }
    }
}