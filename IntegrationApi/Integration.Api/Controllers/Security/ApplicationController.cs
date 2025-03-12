using Integration.Application.Interfaces.Security;
using Integration.Shared.DTO.Security;
using Integration.Shared.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Integration.Api.Controllers.Security
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationService _service;
        private readonly ILogger<ApplicationController> _logger;

        public ApplicationController(IApplicationService service, ILogger<ApplicationController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetAllActive()
        {
            _logger.LogInformation("Iniciando solicitud para obtener todas las aplicaciones activas.");
            try
            {
                var result = await _service.GetAllActiveAsync();
                if (!result.Any())
                {
                    _logger.LogWarning("No se encontraron aplicaciones activas.");
                    return NotFound(ResponseApi<IEnumerable<ApplicationDTO>>.Error("No se encontraron aplicaciones activas."));
                }

                _logger.LogInformation("{Count} aplicaciones activas obtenidas correctamente.", result.Count());
                return Ok(ResponseApi<IEnumerable<ApplicationDTO>>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener aplicaciones activas.");
                return StatusCode(500, ResponseApi<IEnumerable<ApplicationDTO>>.Error("Error interno del servidor."));
            }
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                _logger.LogWarning("Código de aplicación vacío en la solicitud.");
                return BadRequest(ResponseApi<ApplicationDTO>.Error("El código de la aplicación es requerido."));
            }

            _logger.LogInformation("Buscando aplicación con código: {ApplicationCode}", code);
            try
            {
                var result = await _service.GetByCodeAsync(code);
                if (result == null)
                {
                    _logger.LogWarning("No se encontró la aplicación con código {ApplicationCode}.", code);
                    return NotFound(ResponseApi<ApplicationDTO>.Error("Aplicación no encontrada."));
                }

                _logger.LogInformation("Aplicación encontrada: Código={ApplicationCode}, Nombre={Name}", result.Code, result.Name);
                return Ok(ResponseApi<ApplicationDTO>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la aplicación con código {ApplicationCode}.", code);
                return StatusCode(500, ResponseApi<ApplicationDTO>.Error("Error interno del servidor."));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ApplicationDTO applicationDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Se recibió una solicitud con datos inválidos para crear una aplicación.");
                return BadRequest(ResponseApi<ApplicationDTO>.Error("Datos de entrada inválidos."));
            }

            _logger.LogInformation("Creando nueva aplicación: {Name}", applicationDTO.Name);
            try
            {
                var result = await _service.CreateAsync(applicationDTO);
                if (result == null)
                {
                    _logger.LogWarning("No se pudo crear la aplicación.");
                    return BadRequest(ResponseApi<ApplicationDTO>.Error("No se pudo crear la aplicación."));
                }

                _logger.LogInformation("Aplicación creada con éxito: Código={Code}, Nombre={Name}", result.Code, result.Name);
                return CreatedAtAction(nameof(GetByCode), new { code = result.Code },
                    ResponseApi<ApplicationDTO>.Success(result, "Aplicación creada con éxito."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la aplicación.");
                return StatusCode(500, ResponseApi<ApplicationDTO>.Error("Error interno del servidor."));
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ApplicationDTO applicationDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Se recibió una solicitud con datos inválidos para actualizar una aplicación.");
                return BadRequest(ResponseApi<ApplicationDTO>.Error("Datos de entrada inválidos."));
            }

            _logger.LogInformation("Actualizando aplicación con Código: {Code}, Nombre: {Name}", applicationDTO.Code, applicationDTO.Name);
            try
            {
                var result = await _service.UpdateAsync(applicationDTO);
                if (result == null)
                {
                    _logger.LogWarning("No se pudo actualizar la aplicación con Código {Code}.", applicationDTO.Code);
                    return NotFound(ResponseApi<ApplicationDTO>.Error("Aplicación no encontrada."));
                }

                _logger.LogInformation("Aplicación actualizada con éxito: Código={Code}, Nombre={Name}", result.Code, result.Name);
                return Ok(ResponseApi<ApplicationDTO>.Success(result, "Aplicación actualizada correctamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la aplicación con Código {Code}.", applicationDTO.Code);
                return StatusCode(500, ResponseApi<ApplicationDTO>.Error("Error interno del servidor."));
            }
        }

        [HttpDelete("{code}")]
        public async Task<IActionResult> Deactivate(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                _logger.LogWarning("Se recibió un código vacío en la solicitud de eliminación.");
                return BadRequest(ResponseApi<bool>.Error("El código de la aplicación es requerido."));
            }

            _logger.LogInformation("Eliminando aplicación con Código: {ApplicationCode}", code);
            try
            {
                var result = await _service.DeactivateAsync(code);
                if (!result)
                {
                    _logger.LogWarning("No se encontró la aplicación con Código {ApplicationCode} para eliminar.", code);
                    return NotFound(ResponseApi<bool>.Error("Aplicación no encontrada."));
                }

                _logger.LogInformation("Aplicación eliminada con éxito: Código={ApplicationCode}", code);
                return Ok(ResponseApi<bool>.Success(result, "Aplicación eliminada correctamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la aplicación con Código {ApplicationCode}.", code);
                return StatusCode(500, ResponseApi<bool>.Error("Error interno del servidor."));
            }
        }
    }
}