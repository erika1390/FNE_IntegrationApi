using FluentValidation;
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
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _service;
        private readonly ILogger<RoleController> _logger;
        private readonly IValidator<RoleDTO> _validator;
        public RoleController(IRoleService service, ILogger<RoleController> logger, IValidator<RoleDTO> validator)
        {
            _service = service;
            _logger = logger;
            _validator = validator;
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetAllActive([FromHeader] HeaderDTO header)
        {
            _logger.LogInformation("Iniciando solicitud para obtener todos los roles activos.");
            try
            {
                var result = await _service.GetAllActiveAsync();
                if (!result.Any())
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
        public async Task<IActionResult> GetByCode([FromHeader] HeaderDTO header, string code)
        {
            if (code == null)
            {
                _logger.LogWarning("Se recibió un RoleCode no válido ({RoleCode}) en la solicitud de búsqueda.", code);
                return BadRequest(ResponseApi<RoleDTO>.Error("El code node debe ser nulo o vacio."));
            }
            _logger.LogInformation("Buscando rol con RoleCode: {RoleCode}", code);
            try
            {
                var result = await _service.GetByCodeAsync(code);
                if (result == null)
                {
                    _logger.LogWarning("No se encontró el rol con RoleCode {RoleCode}.", code);
                    return NotFound(ResponseApi<RoleDTO>.Error("Rol no encontrada."));
                }
                _logger.LogInformation("Rol encontrada: RoleCode={RoleCode}, Nombre={Name}", result.Code, result.Name);
                return Ok(ResponseApi<RoleDTO>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el rol con RoleCode {RoleCode}.", code);
                return StatusCode(500, ResponseApi<RoleDTO>.Error("Error interno del servidor."));
            }
        }

        /// <summary>
        /// Obtiene roles basados en un solo filtro.
        /// </summary>
        [HttpGet("filter")]
        public async Task<IActionResult> GetRoles([FromHeader] HeaderDTO header, [FromQuery] string filterField, [FromQuery] string filterValue)
        {
            try
            {
                if (string.IsNullOrEmpty(filterField) || string.IsNullOrEmpty(filterValue))
                {
                    return BadRequest(ResponseApi<IEnumerable<RoleDTO>>.Error("Los campos y valores de filtro son obligatorios."));
                }
                var propertyInfo = typeof(RoleDTO).GetProperty(filterField);
                if (propertyInfo == null)
                {
                    return BadRequest(ResponseApi<IEnumerable<RoleDTO>>.Error($"El campo '{filterField}' no existe en RoleDTO."));
                }
                object typedValue;
                try
                {
                    typedValue = Convert.ChangeType(filterValue, propertyInfo.PropertyType);
                }
                catch (Exception)
                {
                    return BadRequest(ResponseApi<IEnumerable<RoleDTO>>.Error($"El valor '{filterValue}' no puede convertirse a tipo {propertyInfo.PropertyType.Name}."));
                }
                ParameterExpression param = Expression.Parameter(typeof(RoleDTO), "dto");
                MemberExpression property = Expression.Property(param, filterField);
                ConstantExpression constant = Expression.Constant(typedValue, propertyInfo.PropertyType);
                BinaryExpression comparison = Expression.Equal(property, constant);
                Expression<Func<RoleDTO, bool>> filter = Expression.Lambda<Func<RoleDTO, bool>>(comparison, param);
                var result = await _service.GetAllAsync(filter);
                return Ok(ResponseApi<IEnumerable<RoleDTO>>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recuperar roles con filtro.");
                return StatusCode(500, ResponseApi<IEnumerable<RoleDTO>>.Error("Error interno del servidor."));
            }
        }

        /// <summary>
        /// Obtiene roles basados en múltiples filtros.
        /// </summary>
        [HttpGet("filters")]
        public async Task<IActionResult> GetRoles([FromHeader] HeaderDTO header, [FromQuery] Dictionary<string, string> filters)
        {
            try
            {
                if (filters == null || !filters.Any())
                {
                    return BadRequest(ResponseApi<IEnumerable<RoleDTO>>.Error("Se requiere al menos un filtro."));
                }

                List<Expression<Func<RoleDTO, bool>>> filterExpressions = new();

                foreach (var filter in filters)
                {
                    string filterField = filter.Key;
                    string filterValue = filter.Value;

                    var propertyInfo = typeof(RoleDTO).GetProperty(filterField);
                    if (propertyInfo == null)
                    {
                        return BadRequest(ResponseApi<IEnumerable<RoleDTO>>.Error($"El campo '{filterField}' no existe en RoleDTO."));
                    }

                    object typedValue;
                    try
                    {
                        typedValue = Convert.ChangeType(filterValue, propertyInfo.PropertyType);
                    }
                    catch (Exception)
                    {
                        return BadRequest(ResponseApi<IEnumerable<RoleDTO>>.Error($"El valor '{filterValue}' no puede convertirse a tipo {propertyInfo.PropertyType.Name}."));
                    }

                    // Crear expresión de filtro
                    ParameterExpression param = Expression.Parameter(typeof(RoleDTO), "dto");
                    MemberExpression property = Expression.Property(param, filterField);
                    ConstantExpression constant = Expression.Constant(typedValue, propertyInfo.PropertyType);
                    BinaryExpression comparison = Expression.Equal(property, constant);
                    Expression<Func<RoleDTO, bool>> filterExpression = Expression.Lambda<Func<RoleDTO, bool>>(comparison, param);

                    filterExpressions.Add(filterExpression);
                }

                // Llamar al servicio con múltiples filtros
                var result = await _service.GetAllAsync(filterExpressions);

                return Ok(ResponseApi<IEnumerable<RoleDTO>>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recuperar roles con múltiples filtros.");
                return StatusCode(500, ResponseApi<IEnumerable<RoleDTO>>.Error("Error interno del servidor."));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromHeader] HeaderDTO header, [FromBody] RoleDTO roleDTO)
        {
            if (roleDTO == null)
            {
                _logger.LogWarning("Se recibió una solicitud con datos nulos para crear un rol.");
                return BadRequest(ResponseApi<RoleDTO>.Error("Los datos del rol no pueden ser nulos."));
            }
            var validationResult = await _validator.ValidateAsync(roleDTO);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(ResponseApi<RoleDTO>.Error(errors));
            }
            _logger.LogInformation("Creando nuevo rol: {Name}", roleDTO.Name);
            try
            {
                var result = await _service.CreateAsync(header, roleDTO);
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
        public async Task<IActionResult> Update([FromHeader] HeaderDTO header, [FromBody] RoleDTO roleDTO)
        {
            if (roleDTO == null)
            {
                _logger.LogWarning("Se recibió una solicitud con datos nulos para modificar un rol.");
                return BadRequest(ResponseApi<RoleDTO>.Error("Los datos del rol no pueden ser nulos."));
            }
            var validationResult = await _validator.ValidateAsync(roleDTO);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(ResponseApi<RoleDTO>.Error(errors));
            }
            _logger.LogInformation("Actualizando rol con RoleCode: {RoleCode}, Nombre: {Name}", roleDTO.Code, roleDTO.Name);
            try
            {
                var result = await _service.UpdateAsync(header, roleDTO);
                if (result == null)
                {
                    _logger.LogWarning("No se pudo actualizar el rol con RoleCode {RoleCode}.", roleDTO.Code);
                    return NotFound(ResponseApi<RoleDTO>.Error("Rol no encontrada."));
                }
                _logger.LogInformation("Rol actualizada con éxito: RoleCode={RoleCode}, Nombre={Name}", result.Code, result.Name);
                return Ok(ResponseApi<RoleDTO>.Success(result, "Rol actualizada correctamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el rol con RoleCode {RoleCode}.", roleDTO.Code);
                return StatusCode(500, ResponseApi<RoleDTO>.Error("Error interno del servidor."));
            }
        }

        [HttpDelete("{code}")]
        public async Task<IActionResult> Deactivate([FromHeader] HeaderDTO header, string code)
        {
            if (code == null)
            {
                _logger.LogWarning("Se recibió un RoleCode no válido ({RoleCode}) en la solicitud de eliminación.", code);
                return BadRequest(ResponseApi<bool>.Error("El RoleCode debe ser nulo o vacio."));
            }
            _logger.LogInformation("Eliminando rol con RoleCode: {RoleCode}", code);
            try
            {
                var result = await _service.DeactivateAsync(header, code);
                if (!result)
                {
                    _logger.LogWarning("No se encontró el rol con RoleCode {RoleCode} para eliminar.", code);
                    return NotFound(ResponseApi<bool>.Error("Rol no encontrada."));
                }
                _logger.LogInformation("Rol eliminada con éxito: RoleCode={RoleCode}", code);
                return Ok(ResponseApi<bool>.Success(result, "Rol eliminada correctamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el rol con RoleCode {RoleCode}.", code);
                return StatusCode(500, ResponseApi<bool>.Error("Error interno del servidor."));
            }
        }
    }
}