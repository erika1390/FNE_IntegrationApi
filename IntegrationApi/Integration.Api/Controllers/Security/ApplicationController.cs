﻿using Integration.Application.Interfaces.Security;
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
            _logger.LogInformation("Iniciando solicitud para obtener todas las aplicaciones.");

            try
            {
                var result = await _service.GetAllActiveAsync();

                if (result == null || !result.Any())
                {
                    _logger.LogWarning("No se encontraron aplicaciones.");
                    return NotFound(ResponseApi<IEnumerable<ApplicationDTO>>.Error("No se encontraron aplicaciones."));
                }

                _logger.LogInformation("{Count} aplicaciones obtenidas correctamente.", result.Count());
                return Ok(ResponseApi<IEnumerable<ApplicationDTO>>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las aplicaciones.");
                return StatusCode(500, ResponseApi<IEnumerable<ApplicationDTO>>.Error("Error interno del servidor."));
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se recibió un ID no válido ({ApplicationId}) en la solicitud de búsqueda.", id);
                return BadRequest(ResponseApi<ApplicationDTO>.Error("El ID debe ser mayor a 0."));
            }

            _logger.LogInformation("Buscando aplicación con ID: {ApplicationId}", id);

            try
            {
                var result = await _service.GetByIdAsync(id);

                if (result == null)
                {
                    _logger.LogWarning("No se encontró la aplicación con ID {ApplicationId}.", id);
                    return NotFound(ResponseApi<ApplicationDTO>.Error("Aplicación no encontrada."));
                }

                _logger.LogInformation("Aplicación encontrada: ID={ApplicationId}, Nombre={Name}", result.ApplicationId, result.Name);
                return Ok(ResponseApi<ApplicationDTO>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la aplicación con ID {ApplicationId}.", id);
                return StatusCode(500, ResponseApi<ApplicationDTO>.Error("Error interno del servidor."));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
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

                _logger.LogInformation("Aplicación creada con éxito: ID={ApplicationId}, Nombre={Name}", result.ApplicationId, result.Name);
                return CreatedAtAction(nameof(GetById), new { id = result.ApplicationId },
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

            _logger.LogInformation("Actualizando aplicación con ID: {ApplicationId}, Nombre: {Name}", applicationDTO.ApplicationId, applicationDTO.Name);

            try
            {
                var result = await _service.UpdateAsync(applicationDTO);

                if (result == null)
                {
                    _logger.LogWarning("No se pudo actualizar la aplicación con ID {ApplicationId}.", applicationDTO.ApplicationId);
                    return NotFound(ResponseApi<ApplicationDTO>.Error("Aplicación no encontrada."));
                }

                _logger.LogInformation("Aplicación actualizada con éxito: ID={ApplicationId}, Nombre={Name}", result.ApplicationId, result.Name);
                return Ok(ResponseApi<ApplicationDTO>.Success(result, "Aplicación actualizada correctamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la aplicación con ID {ApplicationId}.", applicationDTO.ApplicationId);
                return StatusCode(500, ResponseApi<ApplicationDTO>.Error("Error interno del servidor."));
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se recibió un ID no válido ({ApplicationId}) en la solicitud de eliminación.", id);
                return BadRequest(ResponseApi<bool>.Error("El ID debe ser mayor a 0."));
            }

            _logger.LogInformation("Eliminando aplicación con ID: {ApplicationId}", id);

            try
            {
                var result = await _service.DeleteAsync(id);

                if (!result)
                {
                    _logger.LogWarning("No se encontró la aplicación con ID {ApplicationId} para eliminar.", id);
                    return NotFound(ResponseApi<bool>.Error("Aplicación no encontrada."));
                }

                _logger.LogInformation("Aplicación eliminada con éxito: ID={ApplicationId}", id);
                return Ok(ResponseApi<bool>.Success(result, "Aplicación eliminada correctamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la aplicación con ID {ApplicationId}.", id);
                return StatusCode(500, ResponseApi<bool>.Error("Error interno del servidor."));
            }
        }
    }
}