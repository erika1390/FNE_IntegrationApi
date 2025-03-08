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
    public class ModuleController : ControllerBase
    {
        private readonly IModuleService _service;
        private readonly ILogger<ModuleController> _logger;

        public ModuleController(IModuleService service, ILogger<ModuleController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los módulos activos.
        /// </summary>
        [HttpGet("active")]
        public async Task<IActionResult> GetAllActive()
        {
            _logger.LogInformation("Iniciando solicitud para obtener todos los módulos activos.");
            try
            {
                var result = await _service.GetAllActiveAsync();
                if (result == null || !result.Any())
                {
                    _logger.LogWarning("No se encontraron módulos.");
                    return NotFound(ResponseApi<IEnumerable<ModuleDTO>>.Error("No se encontraron módulos."));
                }
                _logger.LogInformation($"{result.Count()} módulos recuperados exitosamente.");
                return Ok(ResponseApi<IEnumerable<ModuleDTO>>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recuperar todos los módulos activos.");
                return StatusCode(500, ResponseApi<IEnumerable<ModuleDTO>>.Error("Error interno del servidor."));
            }
        }

        /// <summary>
        /// Obtiene un módulo por su ID.
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("ID no válido recibido ({ModuleId}) en la solicitud de búsqueda.", id);
                return BadRequest(ResponseApi<ModuleDTO>.Error("El ID debe ser mayor que 0."));
            }
            _logger.LogInformation("Buscando módulo con ID: {ModuleId}", id);
            try
            {
                var result = await _service.GetByIdAsync(id);
                if (result == null)
                {
                    _logger.LogWarning("Módulo con ID {ModuleId} no encontrado.", id);
                    return NotFound(ResponseApi<ModuleDTO>.Error("Módulo no encontrado."));
                }
                _logger.LogInformation("Módulo encontrado: ID={ModuleId}, Name={Name}", result.ModuleId, result.Name);
                return Ok(ResponseApi<ModuleDTO>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recuperar módulo con ID {ModuleId}.", id);
                return StatusCode(500, ResponseApi<ModuleDTO>.Error("Error interno del servidor."));
            }
        }

        /// <summary>
        /// Obtiene módulos basados en un solo filtro.
        /// </summary>
        [HttpGet("filter")]
        public async Task<IActionResult> GetModules([FromQuery] string filterField, [FromQuery] string filterValue)
        {
            try
            {
                if (string.IsNullOrEmpty(filterField) || string.IsNullOrEmpty(filterValue))
                {
                    return BadRequest(ResponseApi<IEnumerable<ModuleDTO>>.Error("Los campos y valores de filtro son obligatorios."));
                }
                var propertyInfo = typeof(ModuleDTO).GetProperty(filterField);
                if (propertyInfo == null)
                {
                    return BadRequest(ResponseApi<IEnumerable<ModuleDTO>>.Error($"El campo '{filterField}' no existe en ModuleDTO."));
                }
                object typedValue;
                try
                {
                    typedValue = Convert.ChangeType(filterValue, propertyInfo.PropertyType);
                }
                catch (Exception)
                {
                    return BadRequest(ResponseApi<IEnumerable<ModuleDTO>>.Error($"El valor '{filterValue}' no puede convertirse a tipo {propertyInfo.PropertyType.Name}."));
                }
                ParameterExpression param = Expression.Parameter(typeof(ModuleDTO), "dto");
                MemberExpression property = Expression.Property(param, filterField);
                ConstantExpression constant = Expression.Constant(typedValue, propertyInfo.PropertyType);
                BinaryExpression comparison = Expression.Equal(property, constant);
                Expression<Func<ModuleDTO, bool>> filter = Expression.Lambda<Func<ModuleDTO, bool>>(comparison, param);
                var result = await _service.GetAllAsync(new List<Expression<Func<ModuleDTO, bool>>> { filter });
                return Ok(ResponseApi<IEnumerable<ModuleDTO>>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recuperar módulos con filtro.");
                return StatusCode(500, ResponseApi<IEnumerable<ModuleDTO>>.Error("Error interno del servidor."));
            }
        }

        /// <summary>
        /// Obtiene módulos basados en múltiples filtros.
        /// </summary>
        [HttpGet("filters")]
        public async Task<IActionResult> GetModules([FromQuery] Dictionary<string, string> filters)
        {
            try
            {
                if (filters == null || filters.Count == 0)
                {
                    return BadRequest(ResponseApi<IEnumerable<ModuleDTO>>.Error("Se requiere al menos un filtro."));
                }
                ParameterExpression param = Expression.Parameter(typeof(ModuleDTO), "dto");
                Expression finalExpression = null;
                foreach (var filter in filters)
                {
                    var propertyInfo = typeof(ModuleDTO).GetProperty(filter.Key);
                    if (propertyInfo == null)
                    {
                        return BadRequest(ResponseApi<IEnumerable<ModuleDTO>>.Error($"El campo '{filter.Key}' no existe en ModuleDTO."));
                    }
                    object typedValue;
                    try
                    {
                        typedValue = Convert.ChangeType(filter.Value, propertyInfo.PropertyType);
                    }
                    catch (Exception)
                    {
                        return BadRequest(ResponseApi<IEnumerable<ModuleDTO>>.Error($"El valor '{filter.Value}' no puede convertirse a tipo {propertyInfo.PropertyType.Name}."));
                    }
                    MemberExpression property = Expression.Property(param, propertyInfo);
                    ConstantExpression constant = Expression.Constant(typedValue, propertyInfo.PropertyType);
                    BinaryExpression comparison = Expression.Equal(property, constant);
                    finalExpression = finalExpression == null ? comparison : Expression.AndAlso(finalExpression, comparison);
                }
                var filterExpression = Expression.Lambda<Func<ModuleDTO, bool>>(finalExpression, param);
                var result = await _service.GetAllAsync(new List<Expression<Func<ModuleDTO, bool>>> { filterExpression });
                return Ok(ResponseApi<IEnumerable<ModuleDTO>>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recuperar módulos con múltiples filtros.");
                return StatusCode(500, ResponseApi<IEnumerable<ModuleDTO>>.Error("Error interno del servidor."));
            }
        }

        /// <summary>
        /// Crea un nuevo módulo.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create([FromBody] ModuleDTO moduleDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Datos de entrada no válidos recibidos para crear un módulo.");
                return BadRequest(ResponseApi<ModuleDTO>.Error("Datos de entrada no válidos."));
            }
            _logger.LogInformation("Creando nuevo módulo: {Name}", moduleDTO.Name);
            try
            {
                var result = await _service.CreateAsync(moduleDTO);
                if (result == null)
                {
                    _logger.LogWarning("Fallo al crear el módulo.");
                    return BadRequest(ResponseApi<ModuleDTO>.Error("Fallo al crear el módulo."));
                }
                _logger.LogInformation("Módulo creado exitosamente: ID={ModuleId}, Name={Name}", result.ModuleId, result.Name);
                return CreatedAtAction(nameof(GetById), new { id = result.ModuleId },
                    ResponseApi<ModuleDTO>.Success(result, "Módulo creado exitosamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el módulo.");
                return StatusCode(500, ResponseApi<ModuleDTO>.Error("Error interno del servidor."));
            }
        }

        /// <summary>
        /// Actualiza un módulo existente.
        /// </summary>
        [HttpPut]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Update([FromBody] ModuleDTO moduleDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Datos de entrada no válidos recibidos para actualizar un módulo.");
                return BadRequest(ResponseApi<ModuleDTO>.Error("Datos de entrada no válidos."));
            }
            _logger.LogInformation("Actualizando módulo con ID: {ModuleId}, Name: {Name}", moduleDTO.ModuleId, moduleDTO.Name);
            try
            {
                var result = await _service.UpdateAsync(moduleDTO);
                if (result == null)
                {
                    _logger.LogWarning("Módulo con ID {ModuleId} no encontrado.", moduleDTO.ModuleId);
                    return NotFound(ResponseApi<ModuleDTO>.Error("Módulo no encontrado."));
                }
                _logger.LogInformation("Módulo actualizado exitosamente: ID={ModuleId}, Name={Name}", result.ModuleId, result.Name);
                return Ok(ResponseApi<ModuleDTO>.Success(result, "Módulo actualizado exitosamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar módulo con ID {ModuleId}.", moduleDTO.ModuleId);
                return StatusCode(500, ResponseApi<ModuleDTO>.Error("Error interno del servidor."));
            }
        }

        /// <summary>
        /// Elimina un módulo por su ID.
        /// </summary>
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("ID no válido recibido ({ModuleId}) en la solicitud de eliminación.", id);
                return BadRequest(ResponseApi<bool>.Error("El ID debe ser mayor que 0."));
            }
            _logger.LogInformation("Eliminando módulo con ID: {ModuleId}", id);
            try
            {
                var result = await _service.DeleteAsync(id);
                if (!result)
                {
                    _logger.LogWarning("Módulo con ID {ModuleId} no encontrado.", id);
                    return NotFound(ResponseApi<bool>.Error("Módulo no encontrado."));
                }
                _logger.LogInformation("Módulo eliminado exitosamente: ID={ModuleId}", id);
                return Ok(ResponseApi<bool>.Success(result, "Módulo eliminado exitosamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar módulo con ID {ModuleId}.", id);
                return StatusCode(500, ResponseApi<bool>.Error("Error interno del servidor."));
            }
        }
    }
}