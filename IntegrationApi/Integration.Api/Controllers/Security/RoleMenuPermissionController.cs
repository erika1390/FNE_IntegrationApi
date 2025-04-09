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
    public class RoleMenuPermissionController : ControllerBase
    {
        private readonly IRoleMenuPermissionService _service;
        private readonly ILogger<RoleMenuPermissionController> _logger;
        private readonly IValidator<RoleMenuPermissionDTO> _validator;
        public RoleMenuPermissionController(IRoleMenuPermissionService service, ILogger<RoleMenuPermissionController> logger, IValidator<RoleMenuPermissionDTO> validator)
        {
            _service = service;
            _logger = logger;
            _validator = validator;
        }
        [HttpGet("active")]
        public async Task<IActionResult> GetAllActive([FromHeader] HeaderDTO header)
        {
            _logger.LogInformation("Iniciando solicitud para obtener todos los  RoleMenuPermission activos.");
            try
            {
                var result = await _service.GetAllActiveAsync();
                if (!result.Any())
                {
                    _logger.LogWarning("No se encontraron RoleMenuPermission activos.");
                    return NotFound(ResponseApi<IEnumerable<RoleMenuPermissionDTO>>.Error("No se encontraron RoleMenuPermission activos."));
                }
                _logger.LogInformation("{Count} RoleMenuPermission activos obtenidos correctamente.", result.Count());
                return Ok(ResponseApi<IEnumerable<RoleMenuPermissionDTO>>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener RoleMenuPermission activos.");
                return StatusCode(500, ResponseApi<IEnumerable<RoleMenuPermissionDTO>>.Error("Error interno del servidor."));
            }
        }

        [HttpPost("ByCodes")]
        public async Task<IActionResult> GetByCodes([FromHeader] HeaderDTO header, RoleMenuPermissionDTO RoleMenuPermissionDTO)
        {
            if (RoleMenuPermissionDTO == null)
            {
                _logger.LogWarning("Se recibió un RoleMenuPermission no válido RoleCode: {RoleCode}, MenuCode: {MenuCode}, PermissionCode: {PermissionCode} en la solicitud de búsqueda.", RoleMenuPermissionDTO.RoleCode, RoleMenuPermissionDTO.MenuCode, RoleMenuPermissionDTO.PermissionCode);

                return BadRequest(ResponseApi<RoleMenuPermissionDTO>.Error("El code node debe ser nulo o vacio."));
            }
            _logger.LogInformation("Buscando RoleMenuPermission con RoleCode: {RoleCode}, MenuCode: {MenuCode}, PermissionCode: {PermissionCode}", RoleMenuPermissionDTO.RoleCode, RoleMenuPermissionDTO.MenuCode, RoleMenuPermissionDTO.PermissionCode);
            try
            {
                var result = await _service.GetByCodesAsync(RoleMenuPermissionDTO);
                if (result == null)
                {
                    _logger.LogWarning("No se encontró el RoleMenuPermission con RoleCode: {RoleCode}, MenuCode: {MenuCode}, PermissionCode: {PermissionCode}", RoleMenuPermissionDTO.RoleCode, RoleMenuPermissionDTO.MenuCode, RoleMenuPermissionDTO.PermissionCode);
                    return NotFound(ResponseApi<RoleMenuPermissionDTO>.Error("RoleMenuPermission no encontrada."));
                }
                _logger.LogInformation("RoleMenuPermission encontrada con RoleCode: {RoleCode}, MenuCode: {MenuCode}, PermissionCode: {PermissionCode}", RoleMenuPermissionDTO.RoleCode, RoleMenuPermissionDTO.MenuCode, RoleMenuPermissionDTO.PermissionCode);
                return Ok(ResponseApi<RoleMenuPermissionDTO>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el RoleMenuPermission con RoleCode: {RoleCode}, MenuCode: {MenuCode}, PermissionCode: {PermissionCode}", RoleMenuPermissionDTO.RoleCode, RoleMenuPermissionDTO.MenuCode, RoleMenuPermissionDTO.PermissionCode);
                return StatusCode(500, ResponseApi<RoleMenuPermissionDTO>.Error("Error interno del servidor."));
            }
        }

        /// <summary>
        /// Obtiene roles basados en un solo filtro.
        /// </summary>
        [HttpGet("filter")]
        public async Task<IActionResult> GetByFilter([FromHeader] HeaderDTO header, [FromQuery] string filterField, [FromQuery] string filterValue)
        {
            try
            {
                if (string.IsNullOrEmpty(filterField) || string.IsNullOrEmpty(filterValue))
                {
                    return BadRequest(ResponseApi<IEnumerable<RoleMenuPermissionDTO>>.Error("Los campos y valores de filtro son obligatorios."));
                }
                var propertyInfo = typeof(RoleMenuPermissionDTO).GetProperty(filterField);
                if (propertyInfo == null)
                {
                    return BadRequest(ResponseApi<IEnumerable<RoleMenuPermissionDTO>>.Error($"El campo '{filterField}' no existe en RoleMenuPermissionDTO."));
                }
                object typedValue;
                try
                {
                    typedValue = Convert.ChangeType(filterValue, propertyInfo.PropertyType);
                }
                catch (Exception)
                {
                    return BadRequest(ResponseApi<IEnumerable<RoleMenuPermissionDTO>>.Error($"El valor '{filterValue}' no puede convertirse a tipo {propertyInfo.PropertyType.Name}."));
                }
                ParameterExpression param = Expression.Parameter(typeof(RoleMenuPermissionDTO), "dto");
                MemberExpression property = Expression.Property(param, filterField);
                ConstantExpression constant = Expression.Constant(typedValue, propertyInfo.PropertyType);
                BinaryExpression comparison = Expression.Equal(property, constant);
                Expression<Func<RoleMenuPermissionDTO, bool>> filter = Expression.Lambda<Func<RoleMenuPermissionDTO, bool>>(comparison, param);
                var result = await _service.GetByFilterAsync(filter);
                return Ok(ResponseApi<IEnumerable<RoleMenuPermissionDTO>>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recuperar roles con filtro.");
                return StatusCode(500, ResponseApi<IEnumerable<RoleMenuPermissionDTO>>.Error("Error interno del servidor."));
            }
        }

        /// <summary>
        /// Obtiene roles basados en múltiples filtros.
        /// </summary>
        [HttpGet("filters")]
        public async Task<IActionResult> GetByMultipleFilters([FromHeader] HeaderDTO header, [FromQuery] Dictionary<string, string> filters)
        {
            try
            {
                if (filters == null || !filters.Any())
                {
                    return BadRequest(ResponseApi<IEnumerable<RoleMenuPermissionDTO>>.Error("Se requiere al menos un filtro."));
                }

                List<Expression<Func<RoleMenuPermissionDTO, bool>>> filterExpressions = new();

                foreach (var filter in filters)
                {
                    string filterField = filter.Key;
                    string filterValue = filter.Value;

                    var propertyInfo = typeof(RoleMenuPermissionDTO).GetProperty(filterField);
                    if (propertyInfo == null)
                    {
                        return BadRequest(ResponseApi<IEnumerable<RoleMenuPermissionDTO>>.Error($"El campo '{filterField}' no existe en RoleMenuPermissionDTO."));
                    }

                    object typedValue;
                    try
                    {
                        typedValue = Convert.ChangeType(filterValue, propertyInfo.PropertyType);
                    }
                    catch (Exception)
                    {
                        return BadRequest(ResponseApi<IEnumerable<RoleMenuPermissionDTO>>.Error($"El valor '{filterValue}' no puede convertirse a tipo {propertyInfo.PropertyType.Name}."));
                    }

                    // Crear expresión de filtro
                    ParameterExpression param = Expression.Parameter(typeof(RoleMenuPermissionDTO), "dto");
                    MemberExpression property = Expression.Property(param, filterField);
                    ConstantExpression constant = Expression.Constant(typedValue, propertyInfo.PropertyType);
                    BinaryExpression comparison = Expression.Equal(property, constant);
                    Expression<Func<RoleMenuPermissionDTO, bool>> filterExpression = Expression.Lambda<Func<RoleMenuPermissionDTO, bool>>(comparison, param);

                    filterExpressions.Add(filterExpression);
                }

                // Llamar al servicio con múltiples filtros
                var result = await _service.GetByMultipleFiltersAsync(filterExpressions);

                return Ok(ResponseApi<IEnumerable<RoleMenuPermissionDTO>>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recuperar RoleMenuPermission con múltiples filtros.");
                return StatusCode(500, ResponseApi<IEnumerable<RoleMenuPermissionDTO>>.Error("Error interno del servidor."));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromHeader] HeaderDTO header, [FromBody] RoleMenuPermissionDTO RoleMenuPermissionDTO)
        {
            if (RoleMenuPermissionDTO == null)
            {
                _logger.LogWarning("Se recibió una solicitud con datos nulos para crear un RoleMenuPermission.");
                return BadRequest(ResponseApi<RoleMenuPermissionDTO>.Error("Los datos del RoleMenuPermission no pueden ser nulos."));
            }
            var validationResult = await _validator.ValidateAsync(RoleMenuPermissionDTO);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(ResponseApi<RoleMenuPermissionDTO>.Error(errors));
            }
            _logger.LogInformation("Creando nuevo RoleMenuPermission: RoleCode: {RoleCode}, MenuCode: {MenuCode}, PermissionCode: {PermissionCode}", RoleMenuPermissionDTO.RoleCode, RoleMenuPermissionDTO.MenuCode, RoleMenuPermissionDTO.PermissionCode);
            try
            {
                var result = await _service.CreateAsync(header, RoleMenuPermissionDTO);
                if (result == null)
                {
                    _logger.LogWarning("No se pudo crear el RoleMenuPermissionDTO.");
                    return BadRequest(ResponseApi<RoleMenuPermissionDTO>.Error("No se pudo crear el rol."));
                }

                _logger.LogInformation("RoleMenuPermission creado con éxito: RoleCode: {RoleCode}, MenuCode: {MenuCode}, PermissionCode: {PermissionCode}", RoleMenuPermissionDTO.RoleCode, RoleMenuPermissionDTO.MenuCode, RoleMenuPermissionDTO.PermissionCode);
                return CreatedAtAction(nameof(GetByCodes), new { RoleMenuPermission = result },
                    ResponseApi<RoleMenuPermissionDTO>.Success(result, "RoleMenuPermissionDTO creado con éxito."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el RoleMenuPermissionDTO.");
                return StatusCode(500, ResponseApi<RoleMenuPermissionDTO>.Error("Error interno del servidor."));
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromHeader] HeaderDTO header, [FromBody] RoleMenuPermissionDTO RoleMenuPermissionDTO)
        {
            if (RoleMenuPermissionDTO == null)
            {
                _logger.LogWarning("Se recibió una solicitud con datos nulos para modificar un RoleMenuPermission.");
                return BadRequest(ResponseApi<RoleMenuPermissionDTO>.Error("Los datos del RoleMenuPermission no pueden ser nulos."));
            }
            var validationResult = await _validator.ValidateAsync(RoleMenuPermissionDTO);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(ResponseApi<RoleMenuPermissionDTO>.Error(errors));
            }
            _logger.LogInformation("Actualizando RoleMenuPermission con RoleCode: {RoleCode}, MenuCode: {MenuCode}, PermissionCode: {PermissionCode}", RoleMenuPermissionDTO.RoleCode, RoleMenuPermissionDTO.MenuCode, RoleMenuPermissionDTO.PermissionCode);
            try
            {
                var result = await _service.UpdateAsync(header, RoleMenuPermissionDTO);
                if (result == null)
                {
                    _logger.LogWarning("No se pudo actualizar el RoleMenuPermission RoleCode: {RoleCode}, MenuCode: {MenuCode}, PermissionCode: {PermissionCode}", RoleMenuPermissionDTO.RoleCode, RoleMenuPermissionDTO.MenuCode, RoleMenuPermissionDTO.PermissionCode);
                    return NotFound(ResponseApi<RoleMenuPermissionDTO>.Error("Rol no encontrada."));
                }
                _logger.LogInformation("RoleMenuPermission actualizada con éxito: RoleCode: {RoleCode}, MenuCode: {MenuCode}, PermissionCode: {PermissionCode}", RoleMenuPermissionDTO.RoleCode, RoleMenuPermissionDTO.MenuCode, RoleMenuPermissionDTO.PermissionCode);
                return Ok(ResponseApi<RoleMenuPermissionDTO>.Success(result, "RoleMenuPermission actualizada correctamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el RoleMenuPermission con RoleCode: {RoleCode}, MenuCode: {MenuCode}, PermissionCode: {PermissionCode}", RoleMenuPermissionDTO.RoleCode, RoleMenuPermissionDTO.MenuCode, RoleMenuPermissionDTO.PermissionCode);
                return StatusCode(500, ResponseApi<RoleMenuPermissionDTO>.Error("Error interno del servidor."));
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Deactivate([FromHeader] HeaderDTO header, [FromBody] RoleMenuPermissionDTO RoleMenuPermissionDTO)
        {
            if (RoleMenuPermissionDTO == null)
            {
                _logger.LogWarning("Se recibió un RoleMenuPermission no válido (RoleCode: {RoleCode}, MenuCode: {MenuCode}, PermissionCode: {PermissionCode}) en la solicitud de eliminación.", RoleMenuPermissionDTO.RoleCode, RoleMenuPermissionDTO.MenuCode, RoleMenuPermissionDTO.PermissionCode);
                return BadRequest(ResponseApi<bool>.Error("El RoleMenuPermission debe ser nulo o vacio."));
            }
            _logger.LogInformation("Desactivar RoleMenuPermission con RoleCode: {RoleCode}, MenuCode: {MenuCode}, PermissionCode: {PermissionCode}", RoleMenuPermissionDTO.RoleCode, RoleMenuPermissionDTO.MenuCode, RoleMenuPermissionDTO.PermissionCode);
            try
            {
                var result = await _service.DeactivateAsync(header, RoleMenuPermissionDTO);
                if (!result)
                {
                    _logger.LogWarning("No se encontró el rol con RoleCode: {RoleCode}, MenuCode: {MenuCode}, PermissionCode: {PermissionCode} para desactivar.", RoleMenuPermissionDTO.RoleCode, RoleMenuPermissionDTO.MenuCode, RoleMenuPermissionDTO.PermissionCode);
                    return NotFound(ResponseApi<bool>.Error("Rol no encontrada."));
                }
                _logger.LogInformation("RoleMenuPermission desactivar con éxito:  RoleCode: {RoleCode}, MenuCode: {MenuCode}, PermissionCode: {PermissionCode} ", RoleMenuPermissionDTO.RoleCode, RoleMenuPermissionDTO.MenuCode, RoleMenuPermissionDTO.PermissionCode);
                return Ok(ResponseApi<bool>.Success(result, "RoleMenuPermission desactivado correctamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al desactivar el RoleMenuPermission con RoleCode: {RoleCode}, MenuCode: {MenuCode}, PermissionCode: {PermissionCode} para desactivar.", RoleMenuPermissionDTO.RoleCode, RoleMenuPermissionDTO.MenuCode, RoleMenuPermissionDTO.PermissionCode);
                return StatusCode(500, ResponseApi<bool>.Error("Error interno del servidor."));
            }
        }
    }
}