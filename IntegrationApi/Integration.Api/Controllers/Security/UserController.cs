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
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService service, ILogger<UserController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetAllActive()
        {
            return await HandleRequest(async () =>
            {
                _logger.LogInformation("Iniciando solicitud para obtener todos los usuarios activos.");
                var result = await _service.GetAllActiveAsync();
                if (!result.Any())
                {
                    _logger.LogWarning("No se encontraron usuarios activos.");
                    return NotFound(ResponseApi<IEnumerable<UserDTO>>.Error("No se encontraron usuarios."));
                }
                _logger.LogInformation("Se encontraron {Count} usuarios activos.", result.Count());
                return Ok(ResponseApi<IEnumerable<UserDTO>>.Success(result));
            });
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            return await HandleRequest(async () =>
            {
                if (string.IsNullOrWhiteSpace(code))
                {
                    _logger.LogWarning("Código de usuario vacío en la solicitud.");
                    return BadRequest(ResponseApi<UserDTO>.Error("El código del usuario es requerido."));
                }

                _logger.LogInformation("Buscando usuario con código: {UserCode}", code);
                var result = await _service.GetByCodeAsync(code);
                if (result == null)
                {
                    _logger.LogWarning("No se encontró el usuario con código {UserCode}.", code);
                    return NotFound(ResponseApi<UserDTO>.Error("Usuario no encontrado."));
                }

                _logger.LogInformation("Usuario encontrado: Código={UserCode}, UserName={UserName}", result.Code, result.UserName);
                return Ok(ResponseApi<UserDTO>.Success(result));
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserDTO userDTO)
        {
            return await HandleRequest(async () =>
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Datos de entrada inválidos para crear un usuario.");
                    return BadRequest(ResponseApi<UserDTO>.Error("Datos de entrada inválidos."));
                }

                _logger.LogInformation("Creando nuevo usuario: {UserName}", userDTO.UserName);
                var result = await _service.CreateAsync(userDTO);
                if (result == null)
                {
                    _logger.LogWarning("No se pudo crear el usuario.");
                    return BadRequest(ResponseApi<UserDTO>.Error("No se pudo crear el usuario."));
                }

                _logger.LogInformation("Usuario creado con éxito: Código={Code}, UserName={UserName}", result.Code, result.UserName);
                return CreatedAtAction(nameof(GetByCode), new { code = result.Code },
                    ResponseApi<UserDTO>.Success(result, "Usuario creado con éxito."));
            });
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UserDTO userDTO)
        {
            return await HandleRequest(async () =>
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Datos de entrada inválidos para actualizar un usuario.");
                    return BadRequest(ResponseApi<UserDTO>.Error("Datos de entrada inválidos."));
                }

                _logger.LogInformation("Actualizando usuario con Código: {Code}, UserName: {Name}", userDTO.Code, userDTO.UserName);
                var result = await _service.UpdateAsync(userDTO);
                if (result == null)
                {
                    _logger.LogWarning("No se pudo actualizar el usuario con Código {Code}.", userDTO.Code);
                    return NotFound(ResponseApi<UserDTO>.Error("Usuario no encontrado."));
                }

                _logger.LogInformation("Usuario actualizado con éxito: Código={Code}, UserName={UserName}", result.Code, result.UserName);
                return Ok(ResponseApi<UserDTO>.Success(result, "Usuario actualizado correctamente."));
            });
        }

        [HttpDelete("{code}")]
        public async Task<IActionResult> Delete(string code)
        {
            return await HandleRequest(async () =>
            {
                if (string.IsNullOrWhiteSpace(code))
                {
                    _logger.LogWarning("Código de usuario vacío en la solicitud de eliminación.");
                    return BadRequest(ResponseApi<bool>.Error("El código del usuario es requerido."));
                }

                _logger.LogInformation("Eliminando usuario con Código: {UserCode}", code);
                var result = await _service.DeactivateAsync(code);
                if (!result)
                {
                    _logger.LogWarning("No se encontró el usuario con Código {UserCode} para eliminar.", code);
                    return NotFound(ResponseApi<bool>.Error("Usuario no encontrado."));
                }

                _logger.LogInformation("Usuario eliminado con éxito: Código={UserCode}", code);
                return Ok(ResponseApi<bool>.Success(result, "Usuario eliminado correctamente."));
            });
        }

        private async Task<IActionResult> HandleRequest(Func<Task<IActionResult>> action)
        {
            try
            {
                return await action();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error en la solicitud.");
                return StatusCode(500, ResponseApi<object>.Error("Error interno del servidor."));
            }
        }
    }
}