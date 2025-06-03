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
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _service;
        private readonly ILogger<DepartmentController> _logger;
        private readonly IValidator<DepartmentDTO> _validator;

        public DepartmentController(IDepartmentService service, ILogger<DepartmentController> logger, IValidator<DepartmentDTO> validator)
        {
            _service = service;
            _logger = logger;
            _validator = validator;
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetAllActive([FromHeader] HeaderDTO header)
        {
            _logger.LogInformation("Iniciando solicitud para obtener todos los departamentos activos.");
            try
            {
                var result = await _service.GetAllActiveAsync();
                if (!result.Any())
                {
                    _logger.LogWarning("No se encontraron los departamentos activos.");
                    return NotFound(ResponseApi<IEnumerable<DepartmentDTO>>.Error("No se encontraron los departamentos activos."));
                }
                _logger.LogInformation("{Count} departamentos activos obtenidas correctamente.", result.Count());
                return Ok(ResponseApi<IEnumerable<DepartmentDTO>>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los departamentos activos.");
                return StatusCode(500, ResponseApi<IEnumerable<DepartmentDTO>>.Error("Error interno del servidor."));
            }
        }
    }
}