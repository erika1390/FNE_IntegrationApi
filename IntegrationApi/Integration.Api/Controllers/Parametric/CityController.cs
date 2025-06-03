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
    public class CityController : Controller
    {
        private readonly ICityService _service;
        private readonly ILogger<CityController> _logger;
        private readonly IValidator<CityDTO> _validator;

        public CityController(ICityService service, ILogger<CityController> logger, IValidator<CityDTO> validator)
        {
            _service = service;
            _logger = logger;
            _validator = validator;
        }

        [HttpGet("department/{departmentId}")]
        public async Task<IActionResult> GetAllActive([FromHeader] HeaderDTO header, int departmentId)
        {
            _logger.LogInformation("Iniciando solicitud para obtener las ciudades.");
            try
            {
                var result = await _service.GetByDepartmentIdAsync(departmentId);
                if (!result.Any())
                {
                    _logger.LogWarning("No se encontraron las ciudades.");
                    return NotFound(ResponseApi<IEnumerable<CityDTO>>.Error("No se encontraron las ciudades."));
                }
                _logger.LogInformation("{Count} ciudades obtenidas correctamente.", result.Count());
                return Ok(ResponseApi<IEnumerable<CityDTO>>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las ciudades.");
                return StatusCode(500, ResponseApi<IEnumerable<CityDTO>>.Error("Error interno del servidor."));
            }
        }
    }
}