using AutoMapper;

using Integration.Application.Interfaces.Parametric;
using Integration.Infrastructure.Interfaces.Parametric;
using Integration.Shared.DTO.Parametric;

using Microsoft.Extensions.Logging;

namespace Integration.Application.Services.Parametric
{
    public class IdentificationDocumentTypeService : IIdentificationDocumentTypeService
    {
        private readonly IIdentificationDocumentTypeRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<IdentificationDocumentTypeService> _logger;

        public IdentificationDocumentTypeService(IIdentificationDocumentTypeRepository repository, IMapper mapper, ILogger<IdentificationDocumentTypeService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<IEnumerable<IdentificationDocumentTypeDTO>> GetAllActiveAsync()
        {
            _logger.LogInformation("Obteniendo todas los tipos de documentos activos.");
            try
            {
                var identificationDocumentTypes = await _repository.GetAllActiveAsync();
                var identificationDocumentTypeDTO = _mapper.Map<IEnumerable<IdentificationDocumentTypeDTO>>(identificationDocumentTypes);
                _logger.LogInformation("{Count} tipos de documentos de identidad activos obtenidos con éxito.", identificationDocumentTypeDTO.Count());
                return identificationDocumentTypeDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los tipos de documentos de identidad activos.");
                throw new Exception("Error al obtener todos los tipos de documentos de identidad activos", ex);
            }
        }
    }
}