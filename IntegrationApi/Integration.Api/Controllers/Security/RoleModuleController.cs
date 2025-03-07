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
                _logger.LogInformation("{Count} roleModules activos obtenidos correctamente.", result.Count());
                return Ok(ResponseApi<IEnumerable<RoleModuleDTO>>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener roleModules activos.");
                return StatusCode(500, ResponseApi<IEnumerable<RoleModuleDTO>>.Error("Error interno del servidor."));
            }
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se recibió un ID no válido ({RoleModuleId}) en la solicitud de búsqueda.", id);
                return BadRequest(ResponseApi<RoleModuleDTO>.Error("El ID debe ser mayor a 0."));
            }
            _logger.LogInformation("Buscando roleModule con ID: {RoleModuleId}", id);
            try
            {
                var result = await _service.GetByIdAsync(id);
                if (result == null)
                {
                    _logger.LogWarning("No se encontró el roleModule con ID {RoleModuleId}.", id);
                    return NotFound(ResponseApi<RoleModuleDTO>.Error("RoleModule no encontrada."));
                }
                _logger.LogInformation("RoleModule encontrada: ID={RoleModuleId}", result.Id);
                return Ok(ResponseApi<RoleModuleDTO>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el RoleModule con ID {RoleModuleId}.", id);
                return StatusCode(500, ResponseApi<RoleModuleDTO>.Error("Error interno del servidor."));
            }
        }
        [HttpGet("filter")]
        public async Task<IActionResult> GetRoleModules([FromQuery] string filterField, [FromQuery] string filterValue)
        {
            try
            {
                if (string.IsNullOrEmpty(filterField) || string.IsNullOrEmpty(filterValue))
                {
                    return BadRequest(ResponseApi<RoleModuleDTO>.Error("Debe proporcionar un campo y un valor para filtrar."));
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
                    return BadRequest(ResponseApi<RoleModuleDTO>.Error($"El valor '{filterValue}' no se puede convertir al tipo {propertyInfo.PropertyType.Name}."));
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
                _logger.LogError(ex, "Error al obtener RoleModules con filtro.");
                return StatusCode(500, ResponseApi<IEnumerable<RoleModuleDTO>>.Error("Error interno del servidor."));
            }
        }
        [HttpGet("filters")]
        public async Task<IActionResult> GetRoleModules([FromQuery] Dictionary<string, string> filters)
        {
            try
            {
                if (filters == null || filters.Count == 0)
                {
                    return BadRequest(ResponseApi<RoleModuleDTO>.Error("Debe proporcionar al menos un filtro."));
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
                        return BadRequest(ResponseApi<RoleModuleDTO>.Error($"El valor '{filter.Value}' no se puede convertir al tipo {propertyInfo.PropertyType.Name}."));
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
                _logger.LogError(ex, "Error al obtener RoleModules con múltiples filtros.");
                return StatusCode(500, ResponseApi<IEnumerable<RoleModuleDTO>>.Error("Error interno del servidor."));
            }
        }
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create([FromBody] RoleModuleDTO roleModuleDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Se recibió una solicitud con datos inválidos para crear un RoleModule.");
                return BadRequest(ResponseApi<RoleModuleDTO>.Error("Datos de entrada inválidos."));
            }
            _logger.LogInformation("Creando nuevo RoleModule: {RoleModuleId}", roleModuleDTO.Id);
            try
            {
                var result = await _service.CreateAsync(roleModuleDTO);
                if (result == null)
                {
                    _logger.LogWarning("No se pudo crear el RoleModule.");
                    return BadRequest(ResponseApi<RoleModuleDTO>.Error("No se pudo crear el RoleModule."));
                }
                _logger.LogInformation("RoleModule creado con éxito: ID={RoleModuleId}", result.Id);
                return CreatedAtAction(nameof(GetById), new { id = result.Id },
                    ResponseApi<RoleModuleDTO>.Success(result, "RoleModule creada con éxito."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la RoleModule.");
                return StatusCode(500, ResponseApi<RoleModuleDTO>.Error("Error interno del servidor."));
            }
        }
        [HttpPut]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Update([FromBody] RoleModuleDTO roleModuleDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Se recibió una solicitud con datos inválidos para actualizar un RoleModule.");
                return BadRequest(ResponseApi<RoleModuleDTO>.Error("Datos de entrada inválidos."));
            }
            _logger.LogInformation("Actualizando RoleModule con ID: {RoleModuleId}", roleModuleDTO.Id);
            try
            {
                var result = await _service.UpdateAsync(roleModuleDTO);
                if (result == null)
                {
                    _logger.LogWarning("No se pudo actualizar el RoleModule con ID {RoleModuleId}.", roleModuleDTO.Id);
                    return NotFound(ResponseApi<RoleModuleDTO>.Error("RoleModule no encontrada."));
                }
                _logger.LogInformation("RoleModule actualizada con éxito: ID={RoleModuleId}", result.Id);
                return Ok(ResponseApi<RoleModuleDTO>.Success(result, "RoleModule actualizada correctamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el RoleModule con ID {RoleModuleId}.", roleModuleDTO.Id);
                return StatusCode(500, ResponseApi<RoleModuleDTO>.Error("Error interno del servidor."));
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se recibió un ID no válido ({RoleModuleId}) en la solicitud de eliminación.", id);
                return BadRequest(ResponseApi<bool>.Error("El ID debe ser mayor a 0."));
            }
            _logger.LogInformation("Eliminando roleModule con ID: {RoleModuleId}", id);
            try
            {
                var result = await _service.DeleteAsync(id);
                if (!result)
                {
                    _logger.LogWarning("No se encontró el roleModule con ID {RoleModuleId} para eliminar.", id);
                    return NotFound(ResponseApi<bool>.Error("RoleModule no encontrada."));
                }
                _logger.LogInformation("RoleModule eliminada con éxito: ID={RoleModuleId}", id);
                return Ok(ResponseApi<bool>.Success(result, "RoleModule eliminada correctamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el RoleModule con ID {RoleModuleId}.", id);
                return StatusCode(500, ResponseApi<bool>.Error("Error interno del servidor."));
            }
        }
    }
}