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
        [HttpGet("active")]
        public async Task<IActionResult> GetAllActive()
        {
            _logger.LogInformation("Iniciando solicitud para obtener todas los modulos.");

            try
            {
                var result = await _service.GetAllActiveAsync();

                if (result == null || !result.Any())
                {
                    _logger.LogWarning("No se encontraron modulos.");
                    return NotFound(ResponseApi<IEnumerable<ModuleDTO>>.Error("No se encontraron modulos."));
                }

                _logger.LogInformation("{Count} modulos obtenidas correctamente.", result.Count());
                return Ok(ResponseApi<IEnumerable<ModuleDTO>>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas los modulos.");
                return StatusCode(500, ResponseApi<IEnumerable<ModuleDTO>>.Error("Error interno del servidor."));
            }
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se recibió un ID no válido ({ModuleId}) en la solicitud de búsqueda.", id);
                return BadRequest(ResponseApi<ModuleDTO>.Error("El ID debe ser mayor a 0."));
            }
            _logger.LogInformation("Buscando modulo con ID: {ModuleId}", id);
            try
            {
                var result = await _service.GetByIdAsync(id);
                if (result == null)
                {
                    _logger.LogWarning("No se encontró la modulo con ID {ModuleId}.", id);
                    return NotFound(ResponseApi<ModuleDTO>.Error("Aplicación no encontrada."));
                }

                _logger.LogInformation("Modulo encontrada: ID={ModuleId}, Nombre={Name}", result.ModuleId, result.Name);
                return Ok(ResponseApi<ModuleDTO>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la modulo con ID {ModuleId}.", id);
                return StatusCode(500, ResponseApi<ModuleDTO>.Error("Error interno del servidor."));
            }
        }
        [HttpGet("filter")]
        public async Task<IActionResult> GetModules([FromQuery] string filterField, [FromQuery] string filterValue)
        {
            try
            {
                if (string.IsNullOrEmpty(filterField) || string.IsNullOrEmpty(filterValue))
                {
                    return BadRequest("Debe proporcionar un campo y un valor para filtrar.");
                }
                var propertyInfo = typeof(ModuleDTO).GetProperty(filterField);
                if (propertyInfo == null)
                {
                    return BadRequest($"El campo '{filterField}' no existe en ModuleDTO.");
                }
                object typedValue;
                try
                {
                    typedValue = Convert.ChangeType(filterValue, propertyInfo.PropertyType);
                }
                catch (Exception)
                {
                    return BadRequest($"El valor '{filterValue}' no se puede convertir al tipo {propertyInfo.PropertyType.Name}.");
                }
                ParameterExpression param = Expression.Parameter(typeof(ModuleDTO), "dto");
                MemberExpression property = Expression.Property(param, filterField);
                ConstantExpression constant = Expression.Constant(typedValue, propertyInfo.PropertyType);
                BinaryExpression comparison = Expression.Equal(property, constant);
                Expression<Func<ModuleDTO, bool>> filter = Expression.Lambda<Func<ModuleDTO, bool>>(comparison, param);
                var applications = await _service.GetAllAsync(new List<Expression<Func<ModuleDTO, bool>>> { filter });
                return Ok(applications);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener modulos con filtro.");
                return StatusCode(500, "Ocurrió un error interno.");
            }
        }
        [HttpGet("filters")]
        public async Task<IActionResult> GetModules([FromQuery] Dictionary<string, string> filters)
        {
            try
            {
                if (filters == null || filters.Count == 0)
                {
                    return BadRequest("Debe proporcionar al menos un filtro.");
                }
                ParameterExpression param = Expression.Parameter(typeof(ModuleDTO), "dto");
                Expression finalExpression = null;
                foreach (var filter in filters)
                {
                    var propertyInfo = typeof(ModuleDTO).GetProperty(filter.Key);
                    if (propertyInfo == null)
                    {
                        return BadRequest($"El campo '{filter.Key}' no existe en ModuleDTO.");
                    }
                    object typedValue;
                    try
                    {
                        typedValue = Convert.ChangeType(filter.Value, propertyInfo.PropertyType);
                    }
                    catch (Exception)
                    {
                        return BadRequest($"El valor '{filter.Value}' no se puede convertir al tipo {propertyInfo.PropertyType.Name}.");
                    }
                    MemberExpression property = Expression.Property(param, propertyInfo);
                    ConstantExpression constant = Expression.Constant(typedValue, propertyInfo.PropertyType);
                    BinaryExpression comparison = Expression.Equal(property, constant);
                    finalExpression = finalExpression == null ? comparison : Expression.AndAlso(finalExpression, comparison);
                }
                var filterExpression = Expression.Lambda<Func<ModuleDTO, bool>>(finalExpression, param);
                var applications = await _service.GetAllAsync(new List<Expression<Func<ModuleDTO, bool>>> { filterExpression });
                return Ok(applications);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener modulos con múltiples filtros.");
                return StatusCode(500, "Ocurrió un error interno.");
            }
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] ModuleDTO moduleDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Se recibió una solicitud con datos inválidos para crear un modulo.");
                return BadRequest(ResponseApi<ModuleDTO>.Error("Datos de entrada inválidos."));
            }
            _logger.LogInformation("Creando nuevo modulo: {Name}", moduleDTO.Name);
            try
            {
                var result = await _service.CreateAsync(moduleDTO);
                if (result == null)
                {
                    _logger.LogWarning("No se pudo crear el modulo.");
                    return BadRequest(ResponseApi<ModuleDTO>.Error("No se pudo crear la modulo."));
                }
                _logger.LogInformation("Modulo creado con éxito: ID={ModuleId}, Nombre={Name}", result.ModuleId, result.Name);
                return CreatedAtAction(nameof(GetById), new { id = result.ModuleId },
                    ResponseApi<ModuleDTO>.Success(result, "Modulo creada con éxito."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la modulo.");
                return StatusCode(500, ResponseApi<ModuleDTO>.Error("Error interno del servidor."));
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ModuleDTO moduleDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Se recibió una solicitud con datos inválidos para actualizar un modulo.");
                return BadRequest(ResponseApi<ModuleDTO>.Error("Datos de entrada inválidos."));
            }
            _logger.LogInformation("Actualizando modulo con ID: {ModuleId}, Nombre: {Name}", moduleDTO.ModuleId, moduleDTO.Name);
            try
            {
                var result = await _service.UpdateAsync(moduleDTO);
                if (result == null)
                {
                    _logger.LogWarning("No se pudo actualizar el modulo con ID {ModuleId}.", moduleDTO.ModuleId);
                    return NotFound(ResponseApi<ModuleDTO>.Error("Modulo no encontrada."));
                }
                _logger.LogInformation("Modulo actualizada con éxito: ID={ModuleId}, Nombre={Name}", result.ModuleId, result.Name);
                return Ok(ResponseApi<ModuleDTO>.Success(result, "Modulo actualizada correctamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el modulo con ID {ModuleId}.", moduleDTO.ModuleId);
                return StatusCode(500, ResponseApi<ModuleDTO>.Error("Error interno del servidor."));
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se recibió un ID no válido ({ModuleId}) en la solicitud de eliminación.", id);
                return BadRequest(ResponseApi<bool>.Error("El ID debe ser mayor a 0."));
            }
            _logger.LogInformation("Eliminando modulo con ID: {ModuleId}", id);
            try
            {
                var result = await _service.DeleteAsync(id);
                if (!result)
                {
                    _logger.LogWarning("No se encontró el modulo con ID {ModuleId} para eliminar.", id);
                    return NotFound(ResponseApi<bool>.Error("Modulo no encontrada."));
                }
                _logger.LogInformation("Modulo eliminada con éxito: ID={ModuleId}", id);
                return Ok(ResponseApi<bool>.Success(result, "Modulo eliminada correctamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la modulo con ID {ModuleId}.", id);
                return StatusCode(500, ResponseApi<bool>.Error("Error interno del servidor."));
            }
        }
    }
}