using FluentValidation;

using Integration.Api.Filters;
using Integration.Application.Interfaces.Security;
using Integration.Shared.DTO.Header;
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
    [ServiceFilter(typeof(ValidateHeadersFilter))]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly ILogger<UserController> _logger;
        private readonly IValidator<UserDTO> _validator;
        public UserController(IUserService service, ILogger<UserController> logger, IValidator<UserDTO> validator)
        {
            _service = service;
            _logger = logger;
            _validator = validator;
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetAllActive([FromHeader] HeaderDTO header)
        {
            var result = await _service.GetAllActiveAsync();
            if (!result.Any())
            {
                return NotFound(ResponseApi<IEnumerable<UserDTO>>.Error("No se encontraron usuarios."));
            }
            return Ok(ResponseApi<IEnumerable<UserDTO>>.Success(result));
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> GetByCode([FromHeader] HeaderDTO header, string code)
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
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetByFilter([FromHeader] HeaderDTO header, [FromQuery] string filterField, [FromQuery] string filterValue)
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

        [HttpGet("filters")]
        public async Task<IActionResult> GetByFilters([FromHeader] HeaderDTO header, [FromQuery] Dictionary<string, string> filters)
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

        [HttpPost]
        public async Task<IActionResult> Create([FromHeader] HeaderDTO header, [FromBody] UserDTO userDTO)
        {
            if (userDTO == null)
            {
                _logger.LogWarning("Se recibió una solicitud con datos nulos para crear un usuario.");
                return BadRequest(ResponseApi<UserDTO>.Error("Los datos del usuario no pueden ser nulos."));
            }
            var validationResult = await _validator.ValidateAsync(userDTO);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(ResponseApi<UserDTO>.Error(errors));
            }
            var result = await _service.CreateAsync(header, userDTO);
            if (result == null)
            {
                return BadRequest(ResponseApi<UserDTO>.Error("No se pudo crear el usuario."));
            }
            return CreatedAtAction(nameof(GetByCode), new { code = result.Code },
                ResponseApi<UserDTO>.Success(result, "Usuario creado con éxito."));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromHeader] HeaderDTO header, [FromBody] UserDTO userDTO)
        {
            if (userDTO == null)
            {
                _logger.LogWarning("Se recibió una solicitud con datos nulos para modificar un usuario.");
                return BadRequest(ResponseApi<UserDTO>.Error("Los datos del usuario no pueden ser nulos."));
            }
            var validationResult = await _validator.ValidateAsync(userDTO);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(ResponseApi<UserDTO>.Error(errors));
            }
            var result = await _service.UpdateAsync(header, userDTO);
            if (result == null)
            {
                return NotFound(ResponseApi<UserDTO>.Error("Usuario no encontrado."));
            }
            return Ok(ResponseApi<UserDTO>.Success(result, "Usuario actualizado correctamente."));
        }

        [HttpDelete("{code}")]
        public async Task<IActionResult> Deactivate([FromHeader] HeaderDTO header, string code)
        {
            if (code == null)
            {
                return BadRequest(ResponseApi<bool>.Error("El code debe ser nulo o vacio."));
            }
            var result = await _service.DeactivateAsync(header, code);
            if (!result)
            {
                return NotFound(ResponseApi<bool>.Error("Usuario no encontrado."));
            }
            return Ok(ResponseApi<bool>.Success(result, "Usuario eliminado correctamente."));
        }
    }
}