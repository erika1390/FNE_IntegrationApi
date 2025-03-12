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
    public class ModuleController : ControllerBase
    {
        private readonly IModuleService _service;
        private readonly ILogger<ModuleController> _logger;

        public ModuleController(IModuleService service, ILogger<ModuleController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los módulos activos.
        /// </summary>
        [HttpGet("active")]
        public async Task<IActionResult> GetAllActive()
        {
            _logger.LogInformation("Iniciando solicitud para obtener todos los módulos activos.");
            try
            {
                var result = await _service.GetAllActiveAsync();
                if (result == null || !result.Any())
                {
                    _logger.LogWarning("No se encontraron módulos activos.");
                    return NotFound(ResponseApi<IEnumerable<ModuleDTO>>.Error("No se encontraron módulos activos."));
                }

                _logger.LogInformation("{Count} módulos recuperados exitosamente.", result.Count());
                return Ok(ResponseApi<IEnumerable<ModuleDTO>>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recuperar todos los módulos activos.");
                return StatusCode(500, ResponseApi<IEnumerable<ModuleDTO>>.Error("Error interno del servidor."));
            }
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                _logger.LogWarning("Código de módulo vacío en la solicitud.");
                return BadRequest(ResponseApi<ModuleDTO>.Error("El código del módulo es requerido."));
            }

            _logger.LogInformation("Buscando módulo con código: {ModuleCode}", code);
            try
            {
                var result = await _service.GetByCodeAsync(code);
                if (result == null)
                {
                    _logger.LogWarning("No se encontró el módulo con código {ModuleCode}.", code);
                    return NotFound(ResponseApi<ModuleDTO>.Error("Módulo no encontrado."));
                }

                _logger.LogInformation("Módulo encontrado: Código={ModuleCode}, Nombre={Name}", result.Code, result.Name);
                return Ok(ResponseApi<ModuleDTO>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el módulo con código {ModuleCode}.", code);
                return StatusCode(500, ResponseApi<ModuleDTO>.Error("Error interno del servidor."));
            }
        }

        /// <summary>
        /// Crea un nuevo módulo.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ModuleDTO moduleDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Datos de entrada inválidos para crear un módulo.");
                return BadRequest(ResponseApi<ModuleDTO>.Error("Datos de entrada inválidos."));
            }

            _logger.LogInformation("Creando nuevo módulo: {Name}", moduleDTO.Name);
            try
            {
                var result = await _service.CreateAsync(moduleDTO);
                if (result == null)
                {
                    _logger.LogWarning("No se pudo crear el módulo.");
                    return BadRequest(ResponseApi<ModuleDTO>.Error("No se pudo crear el módulo."));
                }

                _logger.LogInformation("Módulo creado con éxito: Código={Code}, Nombre={Name}", result.Code, result.Name);
                return CreatedAtAction(nameof(GetByCode), new { code = result.Code },
                    ResponseApi<ModuleDTO>.Success(result, "Módulo creado con éxito."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el módulo.");
                return StatusCode(500, ResponseApi<ModuleDTO>.Error("Error interno del servidor."));
            }
        }

        /// <summary>
        /// Actualiza un módulo existente.
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ModuleDTO moduleDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Datos de entrada inválidos para actualizar un módulo.");
                return BadRequest(ResponseApi<ModuleDTO>.Error("Datos de entrada inválidos."));
            }

            _logger.LogInformation("Actualizando módulo con Código: {Code}, Nombre: {Name}", moduleDTO.Code, moduleDTO.Name);
            try
            {
                var result = await _service.UpdateAsync(moduleDTO);
                if (result == null)
                {
                    _logger.LogWarning("No se pudo actualizar el módulo con Código {Code}.", moduleDTO.Code);
                    return NotFound(ResponseApi<ModuleDTO>.Error("Módulo no encontrado."));
                }

                _logger.LogInformation("Módulo actualizado con éxito: Código={Code}, Nombre={Name}", result.Code, result.Name);
                return Ok(ResponseApi<ModuleDTO>.Success(result, "Módulo actualizado correctamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el módulo con Código {Code}.", moduleDTO.Code);
                return StatusCode(500, ResponseApi<ModuleDTO>.Error("Error interno del servidor."));
            }
        }

        /// <summary>
        /// Elimina un módulo por su código.
        /// </summary>
        [HttpDelete("{code}")]
        public async Task<IActionResult> Delete(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                _logger.LogWarning("Código de módulo vacío en la solicitud de eliminación.");
                return BadRequest(ResponseApi<bool>.Error("El código del módulo es requerido."));
            }

            _logger.LogInformation("Eliminando módulo con Código: {ModuleCode}", code);
            try
            {
                var result = await _service.DeactivateAsync(code);
                if (!result)
                {
                    _logger.LogWarning("No se encontró el módulo con Código {ModuleCode} para eliminar.", code);
                    return NotFound(ResponseApi<bool>.Error("Módulo no encontrado."));
                }

                _logger.LogInformation("Módulo eliminado con éxito: Código={ModuleCode}", code);
                return Ok(ResponseApi<bool>.Success(result, "Módulo eliminado correctamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el módulo con Código {ModuleCode}.", code);
                return StatusCode(500, ResponseApi<bool>.Error("Error interno del servidor."));
            }
        }
    }
}