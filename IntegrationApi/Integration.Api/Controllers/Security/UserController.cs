using Integration.Application.Interfaces.Security;
using Integration.Shared.DTO.Security;
using Integration.Shared.Response;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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
                var result = await _service.GetAllActiveAsync();
                if (!result.Any())
                {
                    return NotFound(ResponseApi<IEnumerable<UserDTO>>.Error("No se encontraron usuarios."));
                }
                return Ok(ResponseApi<IEnumerable<UserDTO>>.Success(result));
            });
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            return await HandleRequest(async () =>
            {
                if (code == null)
                {
                    return BadRequest(ResponseApi<UserDTO>.Error("El code debe ser nulo o vacio."));
                }
                var result = await _service.GetByCodeAsync(code);
                if (result == null)
                {
                    return NotFound(ResponseApi<UserDTO>.Error("Usuario no encontrado."));
                }
                return Ok(ResponseApi<UserDTO>.Success(result));
            });
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetByFilter([FromQuery] string filterField, [FromQuery] string filterValue)
        {
            return await HandleRequest(async () =>
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
            });
        }

        [HttpGet("filters")]
        public async Task<IActionResult> GetByFilters([FromQuery] Dictionary<string, string> filters)
        {
            return await HandleRequest(async () =>
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
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserDTO userDTO)
        {
            return await HandleRequest(async () =>
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ResponseApi<UserDTO>.Error("Datos de entrada inválidos."));
                }
                var result = await _service.CreateAsync(userDTO);
                if (result == null)
                {
                    return BadRequest(ResponseApi<UserDTO>.Error("No se pudo crear el usuario."));
                }
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
                    return BadRequest(ResponseApi<UserDTO>.Error("Datos de entrada inválidos."));
                }
                var result = await _service.UpdateAsync(userDTO);
                if (result == null)
                {
                    return NotFound(ResponseApi<UserDTO>.Error("Usuario no encontrado."));
                }
                return Ok(ResponseApi<UserDTO>.Success(result, "Usuario actualizado correctamente."));
            });
        }

        [HttpDelete("{code}")]
        public async Task<IActionResult> Delete(string code)
        {
            return await HandleRequest(async () =>
            {
                if (code == null)
                {
                    return BadRequest(ResponseApi<bool>.Error("El code debe ser nulo o vacio."));
                }
                var result = await _service.DeactivateAsync(code);
                if (!result)
                {
                    return NotFound(ResponseApi<bool>.Error("Usuario no encontrado."));
                }
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
                _logger.LogError(ex, "Ocurrió un error.");
                return StatusCode(500, ResponseApi<object>.Error("Error interno del servidor."));
            }
        }
    }
}