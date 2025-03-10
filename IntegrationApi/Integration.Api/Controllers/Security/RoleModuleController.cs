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
    public class RoleModuleController : ControllerBase
    {
        private readonly IRoleModuleService _service;
        private readonly ILogger<RoleModuleController> _logger;

        public RoleModuleController(IRoleModuleService service, ILogger<RoleModuleController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los roleModules activos.
        /// </summary>
        [HttpGet("active")]
        public async Task<IActionResult> GetAllActive()
        {
            _logger.LogInformation("Iniciando solicitud para obtener todos los roleModules activos.");
            try
            {
                var result = await _service.GetAllActiveAsync();
                if (!result.Any())
                {
                    _logger.LogWarning("No se encontraron roleModules activos.");
                    return NotFound(ResponseApi<IEnumerable<RoleModuleDTO>>.Error("No se encontraron roleModules activos."));
                }
                _logger.LogInformation($"{result.Count()} roleModules activos recuperados exitosamente.");
                return Ok(ResponseApi<IEnumerable<RoleModuleDTO>>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recuperar roleModules activos.");
                return StatusCode(500, ResponseApi<IEnumerable<RoleModuleDTO>>.Error("Error interno del servidor."));
            }
        }

        /// <summary>
        /// Obtiene un roleModule por su ID.
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("ID no válido recibido ({RoleModuleId}) en la solicitud de búsqueda.", id);
                return BadRequest(ResponseApi<RoleModuleDTO>.Error("El ID debe ser mayor que 0."));
            }
            _logger.LogInformation("Buscando roleModule con ID: {RoleModuleId}", id);
            try
            {
                var result = await _service.GetByIdAsync(id);
                if (result == null)
                {
                    _logger.LogWarning("roleModule con ID {RoleModuleId} no encontrado.", id);
                    return NotFound(ResponseApi<RoleModuleDTO>.Error("roleModule no encontrado."));
                }
                _logger.LogInformation("roleModule encontrado: ID={RoleModuleId}", result.RoleModuleId);
                return Ok(ResponseApi<RoleModuleDTO>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recuperar roleModule con ID {RoleModuleId}.", id);
                return StatusCode(500, ResponseApi<RoleModuleDTO>.Error("Error interno del servidor."));
            }
        }

        /// <summary>
        /// Obtiene roleModules basados en un solo filtro.
        /// </summary>
        [HttpGet("filter")]
        public async Task<IActionResult> GetRoleModules([FromQuery] string filterField, [FromQuery] string filterValue)
        {
            try
            {
                if (string.IsNullOrEmpty(filterField) || string.IsNullOrEmpty(filterValue))
                {
                    return BadRequest(ResponseApi<RoleModuleDTO>.Error("Los campos y valores de filtro son obligatorios."));
                }
                var propertyInfo = typeof(RoleModuleDTO).GetProperty(filterField);
                if (propertyInfo == null)
                {
                    return BadRequest(ResponseApi<RoleModuleDTO>.Error($"El campo '{filterField}' no existe en RoleModuleDTO."));
                }
                object typedValue;
                try
                {
                    typedValue = Convert.ChangeType(filterValue, propertyInfo.PropertyType);
                }
                catch (Exception)
                {
                    return BadRequest(ResponseApi<RoleModuleDTO>.Error($"El valor '{filterValue}' no puede convertirse a tipo {propertyInfo.PropertyType.Name}."));
                }
                ParameterExpression param = Expression.Parameter(typeof(RoleModuleDTO), "dto");
                MemberExpression property = Expression.Property(param, filterField);
                ConstantExpression constant = Expression.Constant(typedValue, propertyInfo.PropertyType);
                BinaryExpression comparison = Expression.Equal(property, constant);
                Expression<Func<RoleModuleDTO, bool>> filter = Expression.Lambda<Func<RoleModuleDTO, bool>>(comparison, param);
                var result = await _service.GetAllAsync(new List<Expression<Func<RoleModuleDTO, bool>>> { filter });
                return Ok(ResponseApi<IEnumerable<RoleModuleDTO>>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recuperar roleModules con filtro.");
                return StatusCode(500, ResponseApi<IEnumerable<RoleModuleDTO>>.Error("Error interno del servidor."));
            }
        }

        /// <summary>
        /// Obtiene roleModules basados en múltiples filtros.
        /// </summary>
        [HttpGet("filters")]
        public async Task<IActionResult> GetRoleModules([FromQuery] Dictionary<string, string> filters)
        {
            try
            {
                if (filters == null || filters.Count == 0)
                {
                    return BadRequest(ResponseApi<RoleModuleDTO>.Error("Se requiere al menos un filtro."));
                }
                ParameterExpression param = Expression.Parameter(typeof(RoleModuleDTO), "dto");
                Expression finalExpression = null;
                foreach (var filter in filters)
                {
                    var propertyInfo = typeof(RoleModuleDTO).GetProperty(filter.Key);
                    if (propertyInfo == null)
                    {
                        return BadRequest(ResponseApi<RoleModuleDTO>.Error($"El campo '{filter.Key}' no existe en RoleModuleDTO."));
                    }
                    object typedValue;
                    try
                    {
                        typedValue = Convert.ChangeType(filter.Value, propertyInfo.PropertyType);
                    }
                    catch (Exception)
                    {
                        return BadRequest(ResponseApi<RoleModuleDTO>.Error($"El valor '{filter.Value}' no puede convertirse a tipo {propertyInfo.PropertyType.Name}."));
                    }
                    MemberExpression property = Expression.Property(param, propertyInfo);
                    ConstantExpression constant = Expression.Constant(typedValue, propertyInfo.PropertyType);
                    BinaryExpression comparison = Expression.Equal(property, constant);
                    finalExpression = finalExpression == null ? comparison : Expression.AndAlso(finalExpression, comparison);
                }
                var filterExpression = Expression.Lambda<Func<RoleModuleDTO, bool>>(finalExpression, param);
                var result = await _service.GetAllAsync(new List<Expression<Func<RoleModuleDTO, bool>>> { filterExpression });
                return Ok(ResponseApi<IEnumerable<RoleModuleDTO>>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recuperar roleModules con múltiples filtros.");
                return StatusCode(500, ResponseApi<IEnumerable<RoleModuleDTO>>.Error("Error interno del servidor."));
            }
        }

        /// <summary>
        /// Crea un nuevo roleModule.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create([FromBody] RoleModuleDTO roleModuleDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Datos de entrada no válidos recibidos para crear un roleModule.");
                return BadRequest(ResponseApi<RoleModuleDTO>.Error("Datos de entrada no válidos."));
            }
            _logger.LogInformation("Creando nuevo roleModule: {RoleModuleId}", roleModuleDTO.RoleModuleId);
            try
            {
                var result = await _service.CreateAsync(roleModuleDTO);
                if (result == null)
                {
                    _logger.LogWarning("Fallo al crear el roleModule.");
                    return BadRequest(ResponseApi<RoleModuleDTO>.Error("Fallo al crear el roleModule."));
                }
                _logger.LogInformation("roleModule creado exitosamente: ID={RoleModuleId}", result.RoleModuleId);
                return CreatedAtAction(nameof(GetById), new { id = result.RoleModuleId },
                    ResponseApi<RoleModuleDTO>.Success(result, "roleModule creado exitosamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el roleModule.");
                return StatusCode(500, ResponseApi<RoleModuleDTO>.Error("Error interno del servidor."));
            }
        }

        /// <summary>
        /// Actualiza un roleModule existente.
        /// </summary>
        [HttpPut]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Update([FromBody] RoleModuleDTO roleModuleDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Datos de entrada no válidos recibidos para actualizar un roleModule.");
                return BadRequest(ResponseApi<RoleModuleDTO>.Error("Datos de entrada no válidos."));
            }
            _logger.LogInformation("Actualizando roleModule con ID: {RoleModuleId}", roleModuleDTO.RoleModuleId);
            try
            {
                var result = await _service.UpdateAsync(roleModuleDTO);
                if (result == null)
                {
                    _logger.LogWarning("roleModule con ID {RoleModuleId} no encontrado.", roleModuleDTO.RoleModuleId);
                    return NotFound(ResponseApi<RoleModuleDTO>.Error("roleModule no encontrado."));
                }
                _logger.LogInformation("roleModule actualizado exitosamente: ID={RoleModuleId}", result.RoleModuleId);
                return Ok(ResponseApi<RoleModuleDTO>.Success(result, "roleModule actualizado exitosamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar roleModule con ID {RoleModuleId}.", roleModuleDTO.RoleModuleId);
                return StatusCode(500, ResponseApi<RoleModuleDTO>.Error("Error interno del servidor."));
            }
        }

        /// <summary>
        /// Elimina un roleModule por su ID.
        /// </summary>
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("ID no válido recibido ({RoleModuleId}) en la solicitud de eliminación.", id);
                return BadRequest(ResponseApi<bool>.Error("El ID debe ser mayor que 0."));
            }
            _logger.LogInformation("Eliminando roleModule con ID: {RoleModuleId}", id);
            try
            {
                var result = await _service.DeleteAsync(id);
                if (!result)
                {
                    _logger.LogWarning("roleModule con ID {RoleModuleId} no encontrado.", id);
                    return NotFound(ResponseApi<bool>.Error("roleModule no encontrado."));
                }
                _logger.LogInformation("roleModule eliminado exitosamente: ID={RoleModuleId}", id);
                return Ok(ResponseApi<bool>.Success(result, "roleModule eliminado exitosamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar roleModule con ID {RoleModuleId}.", id);
                return StatusCode(500, ResponseApi<bool>.Error("Error interno del servidor."));
            }
        }
    }
}