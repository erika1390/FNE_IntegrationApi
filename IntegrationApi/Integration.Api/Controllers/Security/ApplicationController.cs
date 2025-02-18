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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _service.GetAllAsync();
                if (result == null || !result.Any())
                {
                    return NotFound(ResponseApi<IEnumerable<ApplicationDTO>>.Error("No se encontraron aplicaciones."));
                }
                return Ok(ResponseApi<IEnumerable<ApplicationDTO>>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las aplicaciones");
                return StatusCode(500, ResponseApi<IEnumerable<ApplicationDTO>>.Error("Error interno del servidor."));
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
                return BadRequest(ResponseApi<ApplicationDTO>.Error("El ID debe ser mayor a 0."));

            try
            {
                var result = await _service.GetByIdAsync(id);
                if (result == null)
                    return NotFound(ResponseApi<ApplicationDTO>.Error("Aplicación no encontrada."));

                return Ok(ResponseApi<ApplicationDTO>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener la aplicación con ID {id}");
                return StatusCode(500, ResponseApi<ApplicationDTO>.Error("Error interno del servidor."));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] ApplicationDTO entity)
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseApi<ApplicationDTO>.Error("Datos de entrada inválidos."));

            try
            {
                var result = await _service.CreateAsync(entity);
                if (result == null)
                    return BadRequest(ResponseApi<ApplicationDTO>.Error("No se pudo crear la aplicación."));

                return CreatedAtAction(nameof(GetById), new { id = result.ApplicationId },
                    ResponseApi<ApplicationDTO>.Success(result, "Aplicación creada con éxito."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la aplicación");
                return StatusCode(500, ResponseApi<ApplicationDTO>.Error("Error interno del servidor."));
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ApplicationDTO entity)
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseApi<ApplicationDTO>.Error("Datos de entrada inválidos."));

            try
            {
                var result = await _service.UpdateAsync(entity);
                if (result == null)
                    return NotFound(ResponseApi<ApplicationDTO>.Error("Aplicación no encontrada."));

                return Ok(ResponseApi<ApplicationDTO>.Success(result, "Aplicación actualizada correctamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la aplicación");
                return StatusCode(500, ResponseApi<ApplicationDTO>.Error("Error interno del servidor."));
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest(ResponseApi<bool>.Error("El ID debe ser mayor a 0."));

            try
            {
                var result = await _service.DeleteAsync(id);
                if (!result)
                    return NotFound(ResponseApi<bool>.Error("Aplicación no encontrada."));

                return Ok(ResponseApi<bool>.Success(result, "Aplicación eliminada correctamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar la aplicación con ID {id}");
                return StatusCode(500, ResponseApi<bool>.Error("Error interno del servidor."));
            }
        }
    }
}