using Integration.Application.Interfaces.Security;
using Integration.Shared.DTO.Security;
using Integration.Shared.Response;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Linq.Expressions;

namespace Integration.Api.Controllers.Security
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _service;
        private readonly ILogger<RoleController> _logger;

        public RoleController(IRoleService service, ILogger<RoleController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetAllActive()
        {
            _logger.LogInformation("Iniciando solicitud para obtener todos los roles activos.");
            try
            {
                var result = await _service.GetAllActiveAsync();
                if (result == null || !result.Any())
                {
                    _logger.LogWarning("No se encontraron roles activos.");
                    return NotFound(ResponseApi<IEnumerable<RoleDTO>>.Error("No se encontraron roles activos."));
                }

                _logger.LogInformation("{Count} roles activos obtenidos correctamente.", result.Count());
                return Ok(ResponseApi<IEnumerable<RoleDTO>>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener roles activos.");
                return StatusCode(500, ResponseApi<IEnumerable<RoleDTO>>.Error("Error interno del servidor."));
            }
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                _logger.LogWarning("Código de rol vacío en la solicitud.");
                return BadRequest(ResponseApi<RoleDTO>.Error("El código del rol es requerido."));
            }

            _logger.LogInformation("Buscando rol con código: {RoleCode}", code);
            try
            {
                var result = await _service.GetByCodeAsync(code);
                if (result == null)
                {
                    _logger.LogWarning("No se encontró el rol con código {RoleCode}.", code);
                    return NotFound(ResponseApi<RoleDTO>.Error("Rol no encontrado."));
                }

                _logger.LogInformation("Rol encontrado: Código={RoleCode}, Nombre={Name}", result.Code, result.Name);
                return Ok(ResponseApi<RoleDTO>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el rol con código {RoleCode}.", code);
                return StatusCode(500, ResponseApi<RoleDTO>.Error("Error interno del servidor."));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoleDTO roleDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Datos de entrada inválidos para crear un rol.");
                return BadRequest(ResponseApi<RoleDTO>.Error("Datos de entrada inválidos."));
            }

            _logger.LogInformation("Creando nuevo rol: {Name}", roleDTO.Name);
            try
            {
                var result = await _service.CreateAsync(roleDTO);
                if (result == null)
                {
                    _logger.LogWarning("No se pudo crear el rol.");
                    return BadRequest(ResponseApi<RoleDTO>.Error("No se pudo crear el rol."));
                }

                _logger.LogInformation("Rol creado con éxito: Código={Code}, Nombre={Name}", result.Code, result.Name);
                return CreatedAtAction(nameof(GetByCode), new { code = result.Code },
                    ResponseApi<RoleDTO>.Success(result, "Rol creado con éxito."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el rol.");
                return StatusCode(500, ResponseApi<RoleDTO>.Error("Error interno del servidor."));
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] RoleDTO roleDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Datos de entrada inválidos para actualizar un rol.");
                return BadRequest(ResponseApi<RoleDTO>.Error("Datos de entrada inválidos."));
            }

            _logger.LogInformation("Actualizando rol con Código: {Code}, Nombre: {Name}", roleDTO.Code, roleDTO.Name);
            try
            {
                var result = await _service.UpdateAsync(roleDTO);
                if (result == null)
                {
                    _logger.LogWarning("No se pudo actualizar el rol con Código {Code}.", roleDTO.Code);
                    return NotFound(ResponseApi<RoleDTO>.Error("Rol no encontrado."));
                }

                _logger.LogInformation("Rol actualizado con éxito: Código={Code}, Nombre={Name}", result.Code, result.Name);
                return Ok(ResponseApi<RoleDTO>.Success(result, "Rol actualizado correctamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el rol con Código {Code}.", roleDTO.Code);
                return StatusCode(500, ResponseApi<RoleDTO>.Error("Error interno del servidor."));
            }
        }

        [HttpDelete("{code}")]
        public async Task<IActionResult> Delete(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                _logger.LogWarning("Código de rol vacío en la solicitud de eliminación.");
                return BadRequest(ResponseApi<bool>.Error("El código del rol es requerido."));
            }

            _logger.LogInformation("Eliminando rol con Código: {RoleCode}", code);
            try
            {
                var result = await _service.DeactivateAsync(code);
                if (!result)
                {
                    _logger.LogWarning("No se encontró el rol con Código {RoleCode} para eliminar.", code);
                    return NotFound(ResponseApi<bool>.Error("Rol no encontrado."));
                }

                _logger.LogInformation("Rol eliminado con éxito: Código={RoleCode}", code);
                return Ok(ResponseApi<bool>.Success(result, "Rol eliminado correctamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el rol con Código {RoleCode}.", code);
                return StatusCode(500, ResponseApi<bool>.Error("Error interno del servidor."));
            }
        }
    }
}