using Integration.Application.Interfaces.Audit;
using Integration.Shared.DTO.Audit;
using Integration.Shared.Response;

using Microsoft.AspNetCore.Mvc;

namespace Integration.Api.Controllers.Audit
{
    [Route("api/[controller]")]
    [ApiController]
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
        public async Task<IActionResult> Search(
             [FromQuery] string? codeApplication,
             [FromQuery] string? codeUser,
             [FromQuery] DateTime? timestamp,
             [FromQuery] string? level,
             [FromQuery] string? source,
             [FromQuery] string? method)
        {
            _logger.LogInformation("Filtrando logs con parámetros: CodeApplication={CodeApplication}, CodeUser={CodeUser}, Timestamp={Timestamp}, Level={Level}, Source={Source}, Method={Method}",
                codeApplication, codeUser, timestamp, level, source, method);

            try
            {
                var logs = await _service.SearchAsync(codeApplication, codeUser, timestamp, level, source, method);

                if (logs == null || !logs.Any())
                {
                    _logger.LogWarning("No se encontraron logs con los filtros aplicados.");
                }
                _logger.LogInformation("{Count} logs obtenidos con éxito usando filtros.", logs.Count());
                return Ok(ResponseApi<IEnumerable<LogDTO>>.Success(logs));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al filtrar logs.");
                return StatusCode(500, ResponseApi<IEnumerable<LogDTO>>.Error("Error interno del servidor."));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LogDTO logDTO)
        {
            _logger.LogInformation("Datos recibidos para crear log: {@logDTO}", logDTO);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Se recibió una solicitud con datos inválidos.");
                return BadRequest(ResponseApi<LogDTO>.Error("Datos de entrada inválidos."));
            }

            try
            {
                var result = await _service.CreateAsync(logDTO);

                if (result == null)
                {
                    _logger.LogWarning("No se pudo crear el log: {@logDTO}", logDTO);
                    return BadRequest(ResponseApi<LogDTO>.Error("No se pudo crear el log."));
                }

                _logger.LogInformation("Log creado con éxito: ID={LogId}", result.LogId);
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