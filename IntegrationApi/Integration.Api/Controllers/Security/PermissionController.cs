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
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _service;
        private readonly ILogger<PermissionController> _logger;

        public PermissionController(IPermissionService service, ILogger<PermissionController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los permisos activos.
        /// </summary>
        [HttpGet("active")]
        public async Task<IActionResult> GetAllActive()
        {
            _logger.LogInformation("Iniciando solicitud para obtener todos los permisos activos.");
            try
            {
                var result = await _service.GetAllActiveAsync();
                if (result == null || !result.Any())
                {
                    _logger.LogWarning("No se encontraron permisos activos.");
                    return NotFound(ResponseApi<IEnumerable<PermissionDTO>>.Error("No se encontraron permisos activos."));
                }

                _logger.LogInformation("{Count} permisos recuperados exitosamente.", result.Count());
                return Ok(ResponseApi<IEnumerable<PermissionDTO>>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recuperar todos los permisos activos.");
                return StatusCode(500, ResponseApi<IEnumerable<PermissionDTO>>.Error("Error interno del servidor."));
            }
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                _logger.LogWarning("Código de permiso vacío en la solicitud.");
                return BadRequest(ResponseApi<PermissionDTO>.Error("El código del permiso es requerido."));
            }

            _logger.LogInformation("Buscando permiso con código: {PermissionCode}", code);
            try
            {
                var result = await _service.GetByCodeAsync(code);
                if (result == null)
                {
                    _logger.LogWarning("No se encontró el permiso con código {PermissionCode}.", code);
                    return NotFound(ResponseApi<PermissionDTO>.Error("Permiso no encontrado."));
                }

                _logger.LogInformation("Permiso encontrado: Código={PermissionCode}, Nombre={Name}", result.Code, result.Name);
                return Ok(ResponseApi<PermissionDTO>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el permiso con código {PermissionCode}.", code);
                return StatusCode(500, ResponseApi<PermissionDTO>.Error("Error interno del servidor."));
            }
        }

        /// <summary>
        /// Crea un nuevo permiso.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PermissionDTO permissionDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Datos de entrada inválidos para crear un permiso.");
                return BadRequest(ResponseApi<PermissionDTO>.Error("Datos de entrada inválidos."));
            }

            _logger.LogInformation("Creando nuevo permiso: {Name}", permissionDTO.Name);
            try
            {
                var result = await _service.CreateAsync(permissionDTO);
                if (result == null)
                {
                    _logger.LogWarning("No se pudo crear el permiso.");
                    return BadRequest(ResponseApi<PermissionDTO>.Error("No se pudo crear el permiso."));
                }

                _logger.LogInformation("Permiso creado con éxito: Código={Code}, Nombre={Name}", result.Code, result.Name);
                return CreatedAtAction(nameof(GetByCode), new { code = result.Code },
                    ResponseApi<PermissionDTO>.Success(result, "Permiso creado con éxito."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el permiso.");
                return StatusCode(500, ResponseApi<PermissionDTO>.Error("Error interno del servidor."));
            }
        }

        /// <summary>
        /// Actualiza un permiso existente.
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] PermissionDTO permissionDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Datos de entrada inválidos para actualizar un permiso.");
                return BadRequest(ResponseApi<PermissionDTO>.Error("Datos de entrada inválidos."));
            }

            _logger.LogInformation("Actualizando permiso con Código: {Code}, Nombre: {Name}", permissionDTO.Code, permissionDTO.Name);
            try
            {
                var result = await _service.UpdateAsync(permissionDTO);
                if (result == null)
                {
                    _logger.LogWarning("No se pudo actualizar el permiso con Código {Code}.", permissionDTO.Code);
                    return NotFound(ResponseApi<PermissionDTO>.Error("Permiso no encontrado."));
                }

                _logger.LogInformation("Permiso actualizado con éxito: Código={Code}, Nombre={Name}", result.Code, result.Name);
                return Ok(ResponseApi<PermissionDTO>.Success(result, "Permiso actualizado correctamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el permiso con Código {Code}.", permissionDTO.Code);
                return StatusCode(500, ResponseApi<PermissionDTO>.Error("Error interno del servidor."));
            }
        }

        /// <summary>
        /// Elimina un permiso por su código.
        /// </summary>
        [HttpDelete("{code}")]
        public async Task<IActionResult> Delete(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                _logger.LogWarning("Código de permiso vacío en la solicitud de eliminación.");
                return BadRequest(ResponseApi<bool>.Error("El código del permiso es requerido."));
            }

            _logger.LogInformation("Eliminando permiso con Código: {PermissionCode}", code);
            try
            {
                var result = await _service.DeactivateAsync(code);
                if (!result)
                {
                    _logger.LogWarning("No se encontró el permiso con Código {PermissionCode} para eliminar.", code);
                    return NotFound(ResponseApi<bool>.Error("Permiso no encontrado."));
                }

                _logger.LogInformation("Permiso eliminado con éxito: Código={PermissionCode}", code);
                return Ok(ResponseApi<bool>.Success(result, "Permiso eliminado correctamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el permiso con Código {PermissionCode}.", code);
                return StatusCode(500, ResponseApi<bool>.Error("Error interno del servidor."));
            }
        }
    }
}