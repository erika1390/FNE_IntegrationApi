using Integration.Api.Filters;
using Integration.Application.Interfaces.Audit;
using Integration.Shared.DTO.Audit;
using Integration.Shared.DTO.Header;
using Integration.Shared.Response;

using Microsoft.AspNetCore.Mvc;

namespace Integration.Api.Controllers.Audit
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(ValidateHeadersFilter))]
    public class LogController : ControllerBase
    {
        private readonly ILogService _service;
        private readonly ILogger<LogController> _logger;

        public LogController(ILogService service, ILogger<LogController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromHeader] HeaderDTO header,
             [FromQuery] DateTime? timestamp,
             [FromQuery] string? level,
             [FromQuery] string? source,
             [FromQuery] string? method)
        {
            try
            {
                var logs = await _service.SearchAsync(header, timestamp, level, source, method);

                if (logs == null || !logs.Any())
                {
                    _logger.LogWarning("No se encontraron logs con los filtros aplicados.");
                }
                return Ok(ResponseApi<IEnumerable<LogDTO>>.Success(logs));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al filtrar logs.");
                return StatusCode(500, ResponseApi<IEnumerable<LogDTO>>.Error("Error interno del servidor."));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromHeader] HeaderDTO header,[FromBody] LogDTO logDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Se recibió una solicitud con datos inválidos.");
                return BadRequest(ResponseApi<LogDTO>.Error("Datos de entrada inválidos."));
            }

            try
            {
                var result = await _service.CreateAsync(header, logDTO);

                if (result == null)
                {
                    _logger.LogWarning("No se pudo crear el log: {@logDTO}", logDTO);
                    return BadRequest(ResponseApi<LogDTO>.Error("No se pudo crear el log."));
                }
                return Ok(ResponseApi<LogDTO>.Success(result, "Log creado con éxito."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el Log.");
                return StatusCode(500, ResponseApi<LogDTO>.Error("Error interno del servidor."));
            }
        }

    }
}