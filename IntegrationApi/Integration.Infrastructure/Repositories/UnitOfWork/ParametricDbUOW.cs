using Integration.Infrastructure.Data.Contexts;
using Integration.Infrastructure.Interfaces.Parametric;
using Integration.Infrastructure.Interfaces.UnitOfWork;

using Microsoft.Extensions.Logging;

namespace Integration.Infrastructure.Repositories.UnitOfWork
{
    public class ParametricDbUOW : IParametricDbUOW, IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ParametricDbUOW> _logger;
        public ICityRepository CityRepository { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IDepartmentRepository DepartmentRepository { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IIdentificationDocumentTypeRepository IdentificationDocumentTypeRepository { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ParametricDbUOW(
            ApplicationDbContext context,
            ILogger<ParametricDbUOW> logger,
            ICityRepository cityRepository,
            IDepartmentRepository departmentRepository,
            IIdentificationDocumentTypeRepository identificationDocumentTypeRepository)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            CityRepository = cityRepository ?? throw new ArgumentNullException(nameof(cityRepository));
            DepartmentRepository = departmentRepository ?? throw new ArgumentNullException(nameof(departmentRepository));
            IdentificationDocumentTypeRepository = identificationDocumentTypeRepository ?? throw new ArgumentNullException(nameof(identificationDocumentTypeRepository));
        }
        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                _logger.LogInformation("Guardando cambios en la base de datos.");
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar cambios en la base de datos.");
                throw;
            }
        }
    }
}