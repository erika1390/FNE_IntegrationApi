using Integration.Core.Entities.Parametric;
using Integration.Infrastructure.Data.Contexts;
using Integration.Infrastructure.Interfaces.Base;
using Integration.Infrastructure.Interfaces.Parametric;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Integration.Infrastructure.Repositories.Parametric
{
    public class IdentificationDocumentTypeRepository : IIdentificationDocumentTypeRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<IdentificationDocumentTypeRepository> _logger;

        public IdentificationDocumentTypeRepository(ApplicationDbContext context, ILogger<IdentificationDocumentTypeRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<IEnumerable<IdentificationDocumentType>> GetAllActiveAsync()
        {
            try
            {
                var identificationDocumentType = await _context.IdentificationDocumentType.Where(a => a.IsActive == true).ToListAsync();
                _logger.LogInformation("Se obtuvieron {Count} tipos de documentos de la base de datos.", identificationDocumentType.Count);
                return identificationDocumentType;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los tipos de documento.");
                return Enumerable.Empty<IdentificationDocumentType>();
            }
        }
    }
}