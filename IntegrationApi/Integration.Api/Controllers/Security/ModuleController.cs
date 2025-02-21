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
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Iniciando solicitud para obtener todas los modulos.");

            try
            {
                var result = await _service.GetAllAsync();

                if (result == null || !result.Any())
                {
                    _logger.LogWarning("No se encontraron modulos.");
                    return NotFound(ResponseApi<IEnumerable<ModuleDTO>>.Error("No se encontraron modulos."));
                }

                _logger.LogInformation("{Count} modulos obtenidas correctamente.", result.Count());
                return Ok(ResponseApi<IEnumerable<ModuleDTO>>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas los modulos.");
                return StatusCode(500, ResponseApi<IEnumerable<ModuleDTO>>.Error("Error interno del servidor."));
            }
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se recibió un ID no válido ({ModuleId}) en la solicitud de búsqueda.", id);
                return BadRequest(ResponseApi<ModuleDTO>.Error("El ID debe ser mayor a 0."));
            }
            _logger.LogInformation("Buscando modulo con ID: {ModuleId}", id);
            try
            {
                var result = await _service.GetByIdAsync(id);
                if (result == null)
                {
                    _logger.LogWarning("No se encontró la modulo con ID {ModuleId}.", id);
                    return NotFound(ResponseApi<ModuleDTO>.Error("Aplicación no encontrada."));
                }

                _logger.LogInformation("Modulo encontrada: ID={ModuleId}, Nombre={Name}", result.ModuleId, result.Name);
                return Ok(ResponseApi<ModuleDTO>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la modulo con ID {ModuleId}.", id);
                return StatusCode(500, ResponseApi<ModuleDTO>.Error("Error interno del servidor."));
            }
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] ModuleDTO moduleDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Se recibió una solicitud con datos inválidos para crear un modulo.");
                return BadRequest(ResponseApi<ModuleDTO>.Error("Datos de entrada inválidos."));
            }
            _logger.LogInformation("Creando nuevo modulo: {Name}", moduleDTO.Name);
            try
            {
                var result = await _service.CreateAsync(moduleDTO);
                if (result == null)
                {
                    _logger.LogWarning("No se pudo crear el modulo.");
                    return BadRequest(ResponseApi<ModuleDTO>.Error("No se pudo crear la modulo."));
                }
                _logger.LogInformation("Modulo creado con éxito: ID={ModuleId}, Nombre={Name}", result.ModuleId, result.Name);
                return CreatedAtAction(nameof(GetById), new { id = result.ModuleId },
                    ResponseApi<ModuleDTO>.Success(result, "Modulo creada con éxito."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la modulo.");
                return StatusCode(500, ResponseApi<ModuleDTO>.Error("Error interno del servidor."));
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ModuleDTO moduleDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Se recibió una solicitud con datos inválidos para actualizar un modulo.");
                return BadRequest(ResponseApi<ModuleDTO>.Error("Datos de entrada inválidos."));
            }
            _logger.LogInformation("Actualizando modulo con ID: {ModuleId}, Nombre: {Name}", moduleDTO.ModuleId, moduleDTO.Name);
            try
            {
                var result = await _service.UpdateAsync(moduleDTO);
                if (result == null)
                {
                    _logger.LogWarning("No se pudo actualizar el modulo con ID {ModuleId}.", moduleDTO.ModuleId);
                    return NotFound(ResponseApi<ModuleDTO>.Error("Modulo no encontrada."));
                }
                _logger.LogInformation("Modulo actualizada con éxito: ID={ModuleId}, Nombre={Name}", result.ModuleId, result.Name);
                return Ok(ResponseApi<ModuleDTO>.Success(result, "Modulo actualizada correctamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el modulo con ID {ModuleId}.", moduleDTO.ModuleId);
                return StatusCode(500, ResponseApi<ModuleDTO>.Error("Error interno del servidor."));
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se recibió un ID no válido ({ModuleId}) en la solicitud de eliminación.", id);
                return BadRequest(ResponseApi<bool>.Error("El ID debe ser mayor a 0."));
            }
            _logger.LogInformation("Eliminando modulo con ID: {ModuleId}", id);
            try
            {
                var result = await _service.DeleteAsync(id);
                if (!result)
                {
                    _logger.LogWarning("No se encontró el modulo con ID {ModuleId} para eliminar.", id);
                    return NotFound(ResponseApi<bool>.Error("Modulo no encontrada."));
                }
                _logger.LogInformation("Modulo eliminada con éxito: ID={ModuleId}", id);
                return Ok(ResponseApi<bool>.Success(result, "Modulo eliminada correctamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la modulo con ID {ModuleId}.", id);
                return StatusCode(500, ResponseApi<bool>.Error("Error interno del servidor."));
            }
        }
    }
}