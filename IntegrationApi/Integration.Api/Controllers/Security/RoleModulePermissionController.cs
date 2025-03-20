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
    public class RoleModulePermissionController : ControllerBase
    {
        private readonly IRoleModulePermissionService _service;
        private readonly ILogger<RoleModulePermissionController> _logger;
        private readonly IValidator<RoleModulePermissionDTO> _validator;
        public RoleModulePermissionController(IRoleModulePermissionService service, ILogger<RoleModulePermissionController> logger, IValidator<RoleModulePermissionDTO> validator)
        {
            _service = service;
            _logger = logger;
            _validator = validator;
        }
        [HttpGet("active")]
        public async Task<IActionResult> GetAllActive([FromHeader] HeaderDTO header)
        {
            _logger.LogInformation("Iniciando solicitud para obtener todos los  RoleModulePermission activos.");
            try
            {
                var result = await _service.GetAllActiveAsync();
                if (!result.Any())
                {
                    _logger.LogWarning("No se encontraron RoleModulePermission activos.");
                    return NotFound(ResponseApi<IEnumerable<RoleModulePermissionDTO>>.Error("No se encontraron RoleModulePermission activos."));
                }
                _logger.LogInformation("{Count} RoleModulePermission activos obtenidos correctamente.", result.Count());
                return Ok(ResponseApi<IEnumerable<RoleModulePermissionDTO>>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener roles activos.");
                return StatusCode(500, ResponseApi<IEnumerable<RoleModulePermissionDTO>>.Error("Error interno del servidor."));
            }
        }

        [HttpPost("ByCodes")]
        public async Task<IActionResult> GetByCodes([FromHeader] HeaderDTO header, RoleModulePermissionDTO roleModulePermissionDTO)
        {
            if (roleModulePermissionDTO == null)
            {
                _logger.LogWarning("Se recibió un RoleModulePermission no válido (RoleCode: {RoleCode}, ModuleCode: {ModuleCode}, PermissionCode: {PermissionCode}) en la solicitud de búsqueda.", roleModulePermissionDTO.RoleCode, roleModulePermissionDTO.ModuleCode, roleModulePermissionDTO.PermissionCode);

                return BadRequest(ResponseApi<RoleModulePermissionDTO>.Error("El code node debe ser nulo o vacio."));
            }
            _logger.LogInformation("Buscando RoleModulePermission con {RoleCode}, ModuleCode: {ModuleCode}, PermissionCode: {PermissionCode}", roleModulePermissionDTO.RoleCode, roleModulePermissionDTO.ModuleCode, roleModulePermissionDTO.PermissionCode);
            try
            {
                var result = await _service.GetByRoleCodeModuleCodePermissionsCodeAsync(roleModulePermissionDTO);
                if (result == null)
                {
                    _logger.LogWarning("No se encontró el RoleModulePermission con RoleCode: {RoleCode}, ModuleCode: {ModuleCode}, PermissionCode: {PermissionCode}", roleModulePermissionDTO.RoleCode, roleModulePermissionDTO.ModuleCode, roleModulePermissionDTO.PermissionCode);
                    return NotFound(ResponseApi<RoleModulePermissionDTO>.Error("RoleModulePermission no encontrada."));
                }
                _logger.LogInformation("RoleModulePermission encontrada con RoleCode: {RoleCode}, ModuleCode: {ModuleCode}, PermissionCode: {PermissionCode}", roleModulePermissionDTO.RoleCode, roleModulePermissionDTO.ModuleCode, roleModulePermissionDTO.PermissionCode);
                return Ok(ResponseApi<RoleModulePermissionDTO>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el RoleModulePermission con RoleCode: {RoleCode}, ModuleCode: {ModuleCode}, PermissionCode: {PermissionCode}", roleModulePermissionDTO.RoleCode, roleModulePermissionDTO.ModuleCode, roleModulePermissionDTO.PermissionCode);
                return StatusCode(500, ResponseApi<RoleModulePermissionDTO>.Error("Error interno del servidor."));
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
                    return BadRequest(ResponseApi<IEnumerable<RoleModulePermissionDTO>>.Error("Los campos y valores de filtro son obligatorios."));
                }
                var propertyInfo = typeof(RoleModulePermissionDTO).GetProperty(filterField);
                if (propertyInfo == null)
                {
                    return BadRequest(ResponseApi<IEnumerable<RoleModulePermissionDTO>>.Error($"El campo '{filterField}' no existe en RoleModulePermissionDTO."));
                }
                object typedValue;
                try
                {
                    typedValue = Convert.ChangeType(filterValue, propertyInfo.PropertyType);
                }
                catch (Exception)
                {
                    return BadRequest(ResponseApi<IEnumerable<RoleModulePermissionDTO>>.Error($"El valor '{filterValue}' no puede convertirse a tipo {propertyInfo.PropertyType.Name}."));
                }
                ParameterExpression param = Expression.Parameter(typeof(RoleModulePermissionDTO), "dto");
                MemberExpression property = Expression.Property(param, filterField);
                ConstantExpression constant = Expression.Constant(typedValue, propertyInfo.PropertyType);
                BinaryExpression comparison = Expression.Equal(property, constant);
                Expression<Func<RoleModulePermissionDTO, bool>> filter = Expression.Lambda<Func<RoleModulePermissionDTO, bool>>(comparison, param);
                var result = await _service.GetAllAsync(filter);
                return Ok(ResponseApi<IEnumerable<RoleModulePermissionDTO>>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recuperar roles con filtro.");
                return StatusCode(500, ResponseApi<IEnumerable<RoleModulePermissionDTO>>.Error("Error interno del servidor."));
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
                    return BadRequest(ResponseApi<IEnumerable<RoleModulePermissionDTO>>.Error("Se requiere al menos un filtro."));
                }

                List<Expression<Func<RoleModulePermissionDTO, bool>>> filterExpressions = new();

                foreach (var filter in filters)
                {
                    string filterField = filter.Key;
                    string filterValue = filter.Value;

                    var propertyInfo = typeof(RoleModulePermissionDTO).GetProperty(filterField);
                    if (propertyInfo == null)
                    {
                        return BadRequest(ResponseApi<IEnumerable<RoleModulePermissionDTO>>.Error($"El campo '{filterField}' no existe en RoleModulePermissionDTO."));
                    }

                    object typedValue;
                    try
                    {
                        typedValue = Convert.ChangeType(filterValue, propertyInfo.PropertyType);
                    }
                    catch (Exception)
                    {
                        return BadRequest(ResponseApi<IEnumerable<RoleModulePermissionDTO>>.Error($"El valor '{filterValue}' no puede convertirse a tipo {propertyInfo.PropertyType.Name}."));
                    }

                    // Crear expresión de filtro
                    ParameterExpression param = Expression.Parameter(typeof(RoleModulePermissionDTO), "dto");
                    MemberExpression property = Expression.Property(param, filterField);
                    ConstantExpression constant = Expression.Constant(typedValue, propertyInfo.PropertyType);
                    BinaryExpression comparison = Expression.Equal(property, constant);
                    Expression<Func<RoleModulePermissionDTO, bool>> filterExpression = Expression.Lambda<Func<RoleModulePermissionDTO, bool>>(comparison, param);

                    filterExpressions.Add(filterExpression);
                }

                // Llamar al servicio con múltiples filtros
                var result = await _service.GetAllAsync(filterExpressions);

                return Ok(ResponseApi<IEnumerable<RoleModulePermissionDTO>>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recuperar RoleModulePermission con múltiples filtros.");
                return StatusCode(500, ResponseApi<IEnumerable<RoleModulePermissionDTO>>.Error("Error interno del servidor."));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromHeader] HeaderDTO header, [FromBody] RoleModulePermissionDTO roleModulePermissionDTO)
        {
            if (roleModulePermissionDTO == null)
            {
                _logger.LogWarning("Se recibió una solicitud con datos nulos para crear un RoleModulePermission.");
                return BadRequest(ResponseApi<RoleModulePermissionDTO>.Error("Los datos del RoleModulePermission no pueden ser nulos."));
            }
            var validationResult = await _validator.ValidateAsync(roleModulePermissionDTO);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(ResponseApi<RoleModulePermissionDTO>.Error(errors));
            }
            _logger.LogInformation("Creando nuevo RoleModulePermission: RoleCode: {RoleCode}, ModuleCode: {ModuleCode}, PermissionCode: {PermissionCode}: {RoleCode}", roleModulePermissionDTO.RoleCode, roleModulePermissionDTO.ModuleCode, roleModulePermissionDTO.PermissionCode);
            try
            {
                var result = await _service.CreateAsync(header, roleModulePermissionDTO);
                if (result == null)
                {
                    _logger.LogWarning("No se pudo crear el RoleModulePermissionDTO.");
                    return BadRequest(ResponseApi<RoleModulePermissionDTO>.Error("No se pudo crear el rol."));
                }

                _logger.LogInformation("RoleModulePermission creado con éxito: RoleCode: {RoleCode}, ModuleCode: {ModuleCode}, PermissionCode: {PermissionCode}: {RoleCode}", roleModulePermissionDTO.RoleCode, roleModulePermissionDTO.ModuleCode, roleModulePermissionDTO.PermissionCode);
                return CreatedAtAction(nameof(GetByRoleCodeModuleCodePermissionsCode), new { roleModulePermission = result },
                    ResponseApi<RoleModulePermissionDTO>.Success(result, "RoleModulePermissionDTO creado con éxito."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el RoleModulePermissionDTO.");
                return StatusCode(500, ResponseApi<RoleModulePermissionDTO>.Error("Error interno del servidor."));
            }
        }


        [HttpPut]
        public async Task<IActionResult> Update([FromHeader] HeaderDTO header, [FromBody] RoleModulePermissionDTO roleModulePermissionDTO)
        {
            if (roleModulePermissionDTO == null)
            {
                _logger.LogWarning("Se recibió una solicitud con datos nulos para modificar un RoleModulePermission.");
                return BadRequest(ResponseApi<RoleModulePermissionDTO>.Error("Los datos del RoleModulePermission no pueden ser nulos."));
            }
            var validationResult = await _validator.ValidateAsync(roleModulePermissionDTO);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(ResponseApi<RoleModulePermissionDTO>.Error(errors));
            }
            _logger.LogInformation("Actualizando RoleModulePermission con RoleCode: {RoleCode}, ModuleCode: {ModuleCode}, PermissionCode: {PermissionCode}", roleModulePermissionDTO.RoleCode, roleModulePermissionDTO.ModuleCode, roleModulePermissionDTO.PermissionCode);
            try
            {
                var result = await _service.UpdateAsync(header, roleModulePermissionDTO);
                if (result == null)
                {
                    _logger.LogWarning("No se pudo actualizar el RoleModulePermission RoleCode: {RoleCode}, ModuleCode: {ModuleCode}, PermissionCode: {PermissionCode}", roleModulePermissionDTO.RoleCode, roleModulePermissionDTO.ModuleCode, roleModulePermissionDTO.PermissionCode);
                    return NotFound(ResponseApi<RoleModulePermissionDTO>.Error("Rol no encontrada."));
                }
                _logger.LogInformation("RoleModulePermission actualizada con éxito: RoleCode: {RoleCode}, ModuleCode: {ModuleCode}, PermissionCode: {PermissionCode}", roleModulePermissionDTO.RoleCode, roleModulePermissionDTO.ModuleCode, roleModulePermissionDTO.PermissionCode);
                return Ok(ResponseApi<RoleModulePermissionDTO>.Success(result, "RoleModulePermission actualizada correctamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el RoleModulePermission con RoleCode: {RoleCode}, ModuleCode: {ModuleCode}, PermissionCode: {PermissionCode}", roleModulePermissionDTO.RoleCode, roleModulePermissionDTO.ModuleCode, roleModulePermissionDTO.PermissionCode);
                return StatusCode(500, ResponseApi<RoleModulePermissionDTO>.Error("Error interno del servidor."));
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Deactivate([FromHeader] HeaderDTO header, [FromBody] RoleModulePermissionDTO roleModulePermissionDTO)
        {
            if (roleModulePermissionDTO == null)
            {
                _logger.LogWarning("Se recibió un RoleModulePermission no válido (RoleCode: {RoleCode}, ModuleCode: {ModuleCode}, PermissionCode: {PermissionCode}) en la solicitud de eliminación.", roleModulePermissionDTO.RoleCode, roleModulePermissionDTO.ModuleCode, roleModulePermissionDTO.PermissionCode);
                return BadRequest(ResponseApi<bool>.Error("El RoleModulePermission debe ser nulo o vacio."));
            }
            _logger.LogInformation("Desactivar RoleModulePermission con RoleCode: {RoleCode}, ModuleCode: {ModuleCode}, PermissionCode: {PermissionCode}", roleModulePermissionDTO.RoleCode, roleModulePermissionDTO.ModuleCode, roleModulePermissionDTO.PermissionCode);
            try
            {
                var result = await _service.DeactivateAsync(header, roleModulePermissionDTO);
                if (!result)
                {
                    _logger.LogWarning("No se encontró el rol con RoleCode: {RoleCode}, ModuleCode: {ModuleCode}, PermissionCode: {PermissionCode} para desactivar.", roleModulePermissionDTO.RoleCode, roleModulePermissionDTO.ModuleCode, roleModulePermissionDTO.PermissionCode);
                    return NotFound(ResponseApi<bool>.Error("Rol no encontrada."));
                }
                _logger.LogInformation("RoleModulePermission desactivar con éxito:  RoleCode: {RoleCode}, ModuleCode: {ModuleCode}, PermissionCode: {PermissionCode} ", roleModulePermissionDTO.RoleCode, roleModulePermissionDTO.ModuleCode, roleModulePermissionDTO.PermissionCode);
                return Ok(ResponseApi<bool>.Success(result, "RoleModulePermission desactivado correctamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al desactivar el RoleModulePermission con RoleCode: {RoleCode}, ModuleCode: {ModuleCode}, PermissionCode: {PermissionCode} para desactivar.", roleModulePermissionDTO.RoleCode, roleModulePermissionDTO.ModuleCode, roleModulePermissionDTO.PermissionCode);
                return StatusCode(500, ResponseApi<bool>.Error("Error interno del servidor."));
            }
        }
    }
}