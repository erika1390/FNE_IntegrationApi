using Integration.Application.Interfaces.Security;
using Integration.Core.Entities.Security;
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
            _logger.LogInformation("Iniciando solicitud para obtener todos los usuarios.");

            try
            {
                var result = await _service.GetAllActiveAsync();

                if (result == null || !result.Any())
                {
                    _logger.LogWarning("No se encontraron usuarios.");
                    return NotFound(ResponseApi<IEnumerable<UserDTO>>.Error("No se encontraron usuarios."));
                }

                _logger.LogInformation("{Count} usuarios obtenidas correctamente.", result.Count());
                return Ok(ResponseApi<IEnumerable<UserDTO>>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas los usuarios.");
                return StatusCode(500, ResponseApi<IEnumerable<UserDTO>>.Error("Error interno del servidor."));
            }
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se recibió un ID no válido ({UserId}) en la solicitud de búsqueda.", id);
                return BadRequest(ResponseApi<UserDTO>.Error("El ID debe ser mayor a 0."));
            }
            _logger.LogInformation("Buscando usuario con ID: {UserId}", id);
            try
            {
                var result = await _service.GetByIdAsync(id);
                if (result == null)
                {
                    _logger.LogWarning("No se encontró el usuario con ID {UserId}.", id);
                    return NotFound(ResponseApi<UserDTO>.Error("Aplicación no encontrada."));
                }

                _logger.LogInformation("Usuario encontrada: UserName={UserName}", result.UserName);
                return Ok(ResponseApi<UserDTO>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el usuario con ID {UserId}.", id);
                return StatusCode(500, ResponseApi<UserDTO>.Error("Error interno del servidor."));
            }
        }
        [HttpGet("filter")]
        public async Task<IActionResult> GetRoles([FromQuery] string filterField, [FromQuery] string filterValue)
        {
            try
            {
                if (string.IsNullOrEmpty(filterField) || string.IsNullOrEmpty(filterValue))
                {
                    return BadRequest(ResponseApi<UserDTO>.Error("Debe proporcionar un campo y un valor para filtrar."));
                }
                var propertyInfo = typeof(UserDTO).GetProperty(filterField);
                if (propertyInfo == null)
                {
                    return BadRequest(ResponseApi<UserDTO>.Error($"El campo '{filterField}' no existe en UserDTO."));
                }
                object typedValue;
                try
                {
                    typedValue = Convert.ChangeType(filterValue, propertyInfo.PropertyType);
                }
                catch (Exception)
                {
                    return BadRequest(ResponseApi<UserDTO>.Error($"El valor '{filterValue}' no se puede convertir al tipo {propertyInfo.PropertyType.Name}."));
                }
                ParameterExpression param = Expression.Parameter(typeof(UserDTO), "dto");
                MemberExpression property = Expression.Property(param, filterField);
                ConstantExpression constant = Expression.Constant(typedValue, propertyInfo.PropertyType);
                BinaryExpression comparison = Expression.Equal(property, constant);
                Expression<Func<UserDTO, bool>> filter = Expression.Lambda<Func<UserDTO, bool>>(comparison, param);
                var result = await _service.GetAllAsync(new List<Expression<Func<UserDTO, bool>>> { filter });
                return Ok(ResponseApi<IEnumerable<UserDTO>>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener usuarios con filtro.");
                return StatusCode(500, ResponseApi<IEnumerable<UserDTO>>.Error("Error interno del servidor."));
            }
        }
        [HttpGet("filters")]
        public async Task<IActionResult> GetRoles([FromQuery] Dictionary<string, string> filters)
        {
            try
            {
                if (filters == null || filters.Count == 0)
                {
                    return BadRequest(ResponseApi<UserDTO>.Error("Debe proporcionar al menos un filtro."));
                }
                ParameterExpression param = Expression.Parameter(typeof(UserDTO), "dto");
                Expression finalExpression = null;
                foreach (var filter in filters)
                {
                    var propertyInfo = typeof(UserDTO).GetProperty(filter.Key);
                    if (propertyInfo == null)
                    {
                        return BadRequest(ResponseApi<UserDTO>.Error($"El campo '{filter.Key}' no existe en UserDTO."));
                    }
                    object typedValue;
                    try
                    {
                        typedValue = Convert.ChangeType(filter.Value, propertyInfo.PropertyType);
                    }
                    catch (Exception)
                    {
                        return BadRequest(ResponseApi<UserDTO>.Error($"El valor '{filter.Value}' no se puede convertir al tipo {propertyInfo.PropertyType.Name}."));
                    }
                    MemberExpression property = Expression.Property(param, propertyInfo);
                    ConstantExpression constant = Expression.Constant(typedValue, propertyInfo.PropertyType);
                    BinaryExpression comparison = Expression.Equal(property, constant);
                    finalExpression = finalExpression == null ? comparison : Expression.AndAlso(finalExpression, comparison);
                }
                var filterExpression = Expression.Lambda<Func<UserDTO, bool>>(finalExpression, param);
                var result = await _service.GetAllAsync(new List<Expression<Func<UserDTO, bool>>> { filterExpression });
                return Ok(ResponseApi<IEnumerable<UserDTO>>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener usuario con múltiples filtros.");
                return StatusCode(500, ResponseApi<IEnumerable<UserDTO>>.Error("Error interno del servidor."));
            }
        }
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create([FromBody] UserDTO permissionDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Se recibió una solicitud con datos inválidos para crear un usuario.");
                return BadRequest(ResponseApi<UserDTO>.Error("Datos de entrada inválidos."));
            }
            _logger.LogInformation("Creando nuevo usuario: {UserName}", permissionDTO.UserName);
            try
            {
                var result = await _service.CreateAsync(permissionDTO);
                if (result == null)
                {
                    _logger.LogWarning("No se pudo crear el usuario.");
                    return BadRequest(ResponseApi<UserDTO>.Error("No se pudo crear el usuario."));
                }
                _logger.LogInformation("Usuario creado con éxito: UserName={UserName}", result.UserName);
                return CreatedAtAction(nameof(GetById), new { id = result.UserId },
                    ResponseApi<UserDTO>.Success(result, "Rol creada con éxito."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la usuario.");
                return StatusCode(500, ResponseApi<UserDTO>.Error("Error interno del servidor."));
            }
        }

        [HttpPut]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Update([FromBody] UserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Se recibió una solicitud con datos inválidos para actualizar un usuario.");
                return BadRequest(ResponseApi<UserDTO>.Error("Datos de entrada inválidos."));
            }
            _logger.LogInformation("Actualizando usuario con ID: {UserId}, UserName: {UserName}", userDTO.UserId, userDTO.UserName);
            try
            {
                var result = await _service.UpdateAsync(userDTO);
                if (result == null)
                {
                    _logger.LogWarning("No se pudo actualizar el usuario con ID {UserId}.", userDTO.UserId);
                    return NotFound(ResponseApi<UserDTO>.Error("Usuario no encontrada."));
                }
                _logger.LogInformation("Usuario actualizada con éxito: ID={UserId}, UserName={UserName}", result.UserId, result.UserName);
                return Ok(ResponseApi<UserDTO>.Success(result, "Usuario actualizada correctamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el usuario con ID {UserId}.", userDTO.UserId);
                return StatusCode(500, ResponseApi<UserDTO>.Error("Error interno del servidor."));
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se recibió un ID no válido ({UserId}) en la solicitud de eliminación.", id);
                return BadRequest(ResponseApi<bool>.Error("El ID debe ser mayor a 0."));
            }
            _logger.LogInformation("Eliminando usuario con ID: {UserId}", id);
            try
            {
                var result = await _service.DeleteAsync(id);
                if (!result)
                {
                    _logger.LogWarning("No se encontró el usuario con ID {UserId} para eliminar.", id);
                    return NotFound(ResponseApi<bool>.Error("Usuario no encontrada."));
                }
                _logger.LogInformation("Usuario eliminada con éxito: ID={UserId}", id);
                return Ok(ResponseApi<bool>.Success(result, "Usuario eliminada correctamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el usuario con ID {UserId}.", id);
                return StatusCode(500, ResponseApi<bool>.Error("Error interno del servidor."));
            }
        }
    }
}