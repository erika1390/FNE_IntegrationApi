using FluentValidation;

using Integration.Api.Filters;
using Integration.Application.Interfaces.Parametric;
using Integration.Shared.DTO.Header;
using Integration.Shared.DTO.Parametric;
using Integration.Shared.Response;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Integration.Api.Controllers.Parametric
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ServiceFilter(typeof(ValidateHeadersFilter))]
    public class IdentificationDocumentTypeController : ControllerBase
    {
        private readonly IIdentificationDocumentTypeService _service;
        private readonly ILogger<IdentificationDocumentTypeController> _logger;
        private readonly IValidator<IdentificationDocumentTypeDTO> _validator;

        public IdentificationDocumentTypeController(IIdentificationDocumentTypeService service, ILogger<IdentificationDocumentTypeController> logger, IValidator<IdentificationDocumentTypeDTO> validator)
        {
            _service = service;
            _logger = logger;
            _validator = validator;
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetAllActive([FromHeader] HeaderDTO header)
        {
            _logger.LogInformation("Iniciando solicitud para obtener todos los tipos de documentos de identificación activos.");
            try
            {
                var result = await _service.GetAllActiveAsync();
                if (!result.Any())
                {
                    _logger.LogWarning("No se encontraron los tipos de documentos de identificación activos.");
                    return NotFound(ResponseApi<IEnumerable<IdentificationDocumentTypeDTO>>.Error("No se encontraron los tipos de documentos de identificación activos."));
                }
                _logger.LogInformation("{Count} tipos de documentos de identificación activos obtenidas correctamente.", result.Count());
                return Ok(ResponseApi<IEnumerable<IdentificationDocumentTypeDTO>>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los tipos de documentos de identificación activos.");
                return StatusCode(500, ResponseApi<IEnumerable<IdentificationDocumentTypeDTO>>.Error("Error interno del servidor."));
            }
        }
    }
}