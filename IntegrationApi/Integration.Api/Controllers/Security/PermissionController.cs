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
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _service;
        private readonly ILogger<PermissionController> _logger;
        private readonly IValidator<PermissionDTO> _validator;

        public PermissionController(IPermissionService service, ILogger<PermissionController> logger, IValidator<PermissionDTO> validator)
        {
            _service = service;
            _logger = logger;
            _validator = validator;
        }

        /// <summary>
        /// Obtiene todas las permisos activos.
        /// </summary>
        [HttpGet("active")]
        public async Task<IActionResult> GetAllActive([FromHeader] HeaderDTO header)
        {
            _logger.LogInformation("Iniciando solicitud para obtener todas las permisos activos.");
            try
            {
                var result = await _service.GetAllActiveAsync();
                if (result == null || !result.Any())
                {
                    _logger.LogWarning("No se encontraron permisos.");
                    return NotFound(ResponseApi<IEnumerable<PermissionDTO>>.Error("No se encontraron permisos."));
                }
                _logger.LogInformation($"{result.Count()} permisos recuperados exitosamente.");
                return Ok(ResponseApi<IEnumerable<PermissionDTO>>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recuperar todos los permisos activos.");
                return StatusCode(500, ResponseApi<IEnumerable<PermissionDTO>>.Error("Error interno del servidor."));
            }
        }



        /// <summary>
        /// Obtiene permisos basados en un solo filtro.
        /// </summary>
        [HttpGet("filter")]
        public async Task<IActionResult> GetPermissions([FromHeader] HeaderDTO header, [FromQuery] string filterField, [FromQuery] string filterValue)
        {
            try
            {
                if (string.IsNullOrEmpty(filterField) || string.IsNullOrEmpty(filterValue))
                {
                    return BadRequest(ResponseApi<IEnumerable<PermissionDTO>>.Error("Los campos y valores de filtro son obligatorios."));
                }
                var propertyInfo = typeof(PermissionDTO).GetProperty(filterField);
                if (propertyInfo == null)
                {
                    return BadRequest(ResponseApi<IEnumerable<PermissionDTO>>.Error($"El campo '{filterField}' no existe en PermissionDTO."));
                }
                object typedValue;
                try
                {
                    typedValue = Convert.ChangeType(filterValue, propertyInfo.PropertyType);
                }
                catch (Exception)
                {
                    return BadRequest(ResponseApi<IEnumerable<PermissionDTO>>.Error($"El valor '{filterValue}' no puede convertirse a tipo {propertyInfo.PropertyType.Name}."));
                }
                ParameterExpression param = Expression.Parameter(typeof(PermissionDTO), "dto");
                MemberExpression property = Expression.Property(param, filterField);
                ConstantExpression constant = Expression.Constant(typedValue, propertyInfo.PropertyType);
                BinaryExpression comparison = Expression.Equal(property, constant);
                Expression<Func<PermissionDTO, bool>> filter = Expression.Lambda<Func<PermissionDTO, bool>>(comparison, param);
                var result = await _service.GetAllAsync(new List<Expression<Func<PermissionDTO, bool>>> { filter });
                return Ok(ResponseApi<IEnumerable<PermissionDTO>>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recuperar permisos con filtro.");
                return StatusCode(500, ResponseApi<IEnumerable<PermissionDTO>>.Error("Error interno del servidor."));
            }
        }

        /// <summary>
        /// Obtiene permisos basados en múltiples filtros.
        /// </summary>
        [HttpGet("filters")]
        public async Task<IActionResult> GetPermissions([FromHeader] HeaderDTO header, [FromQuery] Dictionary<string, string> filters)
        {
            try
            {
                if (filters == null || filters.Count == 0)
                {
                    return BadRequest(ResponseApi<IEnumerable<PermissionDTO>>.Error("Se requiere al menos un filtro."));
                }
                ParameterExpression param = Expression.Parameter(typeof(PermissionDTO), "dto");
                Expression finalExpression = null;
                foreach (var filter in filters)
                {
                    var propertyInfo = typeof(PermissionDTO).GetProperty(filter.Key);
                    if (propertyInfo == null)
                    {
                        return BadRequest(ResponseApi<IEnumerable<PermissionDTO>>.Error($"El campo '{filter.Key}' no existe en PermissionDTO."));
                    }
                    object typedValue;
                    try
                    {
                        typedValue = Convert.ChangeType(filter.Value, propertyInfo.PropertyType);
                    }
                    catch (Exception)
                    {
                        return BadRequest(ResponseApi<IEnumerable<PermissionDTO>>.Error($"El valor '{filter.Value}' no puede convertirse a tipo {propertyInfo.PropertyType.Name}."));
                    }
                    MemberExpression property = Expression.Property(param, propertyInfo);
                    ConstantExpression constant = Expression.Constant(typedValue, propertyInfo.PropertyType);
                    BinaryExpression comparison = Expression.Equal(property, constant);
                    finalExpression = finalExpression == null ? comparison : Expression.AndAlso(finalExpression, comparison);
                }
                var filterExpression = Expression.Lambda<Func<PermissionDTO, bool>>(finalExpression, param);
                var result = await _service.GetAllAsync(new List<Expression<Func<PermissionDTO, bool>>> { filterExpression });
                return Ok(ResponseApi<IEnumerable<PermissionDTO>>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recuperar permisos con múltiples filtros.");
                return StatusCode(500, ResponseApi<IEnumerable<PermissionDTO>>.Error("Error interno del servidor."));
            }
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> GetByCode([FromHeader] HeaderDTO header, string code)
        {
            if (code == null)
            {
                _logger.LogWarning("Se recibió un PermissionCode no válido ({PermissionCode}) en la solicitud de búsqueda.", code);
                return BadRequest(ResponseApi<PermissionDTO>.Error("El PermissionCode no debe ser nulo o vacio"));
            }
            _logger.LogInformation("Buscando permiso con PermissionCode: {PermissionCode}", code);
            try
            {
                var result = await _service.GetByCodeAsync(code);
                if (result == null)
                {
                    _logger.LogWarning("No se encontró el permiso con PermissionCode {PermissionCode}.", code);
                    return NotFound(ResponseApi<PermissionDTO>.Error("Modulo no encontrada."));
                }
                _logger.LogInformation("|Modulo encontrada: PermissionCode={PermissionCode}, Nombre={Name}", result.Code, result.Name);
                return Ok(ResponseApi<PermissionDTO>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el permiso con PermissionCode {PermissionCode}.", code);
                return StatusCode(500, ResponseApi<PermissionDTO>.Error("Error interno del servidor."));
            }
        }

        /// <summary>
        /// Crea un nuevo permiso.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromHeader] HeaderDTO header, [FromBody] PermissionDTO permissionDTO)
        {
            if (permissionDTO == null)
            {
                _logger.LogWarning("Se recibió una solicitud con datos nulos para crear un permiso.");
                return BadRequest(ResponseApi<PermissionDTO>.Error("Los datos del permiso no pueden ser nulos."));
            }
            var validationResult = await _validator.ValidateAsync(permissionDTO);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(ResponseApi<PermissionDTO>.Error(errors));
            }
            _logger.LogInformation("Creando nuevo permiso: {Name}", permissionDTO.Name);
            try
            {
                var result = await _service.CreateAsync(header, permissionDTO);
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
        public async Task<IActionResult> Update([FromHeader] HeaderDTO header, [FromBody] PermissionDTO permissionDTO)
        {
            if (permissionDTO == null)
            {
                _logger.LogWarning("Se recibió una solicitud con datos nulos para modificar un permiso.");
                return BadRequest(ResponseApi<PermissionDTO>.Error("Los datos de un permiso no pueden ser nulos."));
            }
            var validationResult = await _validator.ValidateAsync(permissionDTO);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(ResponseApi<PermissionDTO>.Error(errors));
            }
            _logger.LogInformation("Permiso creado exitosamente: PermissionCode={PermissionCode}, Name={Name}", permissionDTO.Code, permissionDTO.Name);
            try
            {
                var result = await _service.UpdateAsync(header, permissionDTO);
                if (result == null)
                {
                    _logger.LogWarning("Permiso con PermissionCode {PermissionCode} no encontrado.", permissionDTO.Code);
                    return NotFound(ResponseApi<PermissionDTO>.Error("Permiso no encontrado."));
                }
                _logger.LogInformation("Permiso actualizado exitosamente: PermissionCode={PermissionCode}, Name={Name}", result.Code, result.Name);
                return Ok(ResponseApi<PermissionDTO>.Success(result, "Permiso actualizado exitosamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar permiso con PermissionCode {PermissionCode}.", permissionDTO.Code);
                return StatusCode(500, ResponseApi<PermissionDTO>.Error("Error interno del servidor."));
            }
        }

        /// <summary>
        /// Elimina un permiso por su ID.
        /// </summary>
        [HttpDelete("{code}")]
        public async Task<IActionResult> Deactivate([FromHeader] HeaderDTO header, string code)
        {
            if (code == null)
            {
                _logger.LogWarning("ID no válido recibido ({PermissionCode}) en la solicitud de eliminación.", code);
                return BadRequest(ResponseApi<bool>.Error("El PermissionCode no debe ser nulo o vacío."));
            }

            _logger.LogInformation("Eliminando permiso con PermissionCode: {PermissionCode}", code);
            try
            {
                var result = await _service.DeactivateAsync(header, code);
                if (!result)
                {
                    _logger.LogWarning("Permiso con PermissionCode {PermissionCode} no encontrado.", code);
                    return NotFound(ResponseApi<bool>.Error("Permiso no encontrado."));
                }
                _logger.LogInformation("Permiso eliminado exitosamente: PermissionCode={PermissionCode}", code);
                return Ok(ResponseApi<bool>.Success(result, "Permiso eliminado exitosamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar permiso con PermissionCode {PermissionCode}.", code);
                return StatusCode(500, ResponseApi<bool>.Error("Error interno del servidor."));
            }
        }
    }
}