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
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationService _service;
        private readonly ILogger<ApplicationController> _logger;

        public ApplicationController(IApplicationService service, ILogger<ApplicationController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetAllActive()
        {
            _logger.LogInformation("Iniciando solicitud para obtener todas las aplicaciones activas.");
            try
            {
                var result = await _service.GetAllActiveAsync();
                if (!result.Any())
                {
                    _logger.LogWarning("No se encontraron aplicaciones activas.");
                    return NotFound(ResponseApi<IEnumerable<ApplicationDTO>>.Error("No se encontraron aplicaciones activas."));
                }
                _logger.LogInformation("{Count} aplicaciones activas obtenidas correctamente.", result.Count());
                return Ok(ResponseApi<IEnumerable<ApplicationDTO>>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener aplicaciones activas.");
                return StatusCode(500, ResponseApi<IEnumerable<ApplicationDTO>>.Error("Error interno del servidor."));
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se recibió un ID no válido ({ApplicationId}) en la solicitud de búsqueda.", id);
                return BadRequest(ResponseApi<ApplicationDTO>.Error("El ID debe ser mayor a 0."));
            }
            _logger.LogInformation("Buscando aplicación con ID: {ApplicationId}", id);
            try
            {
                var result = await _service.GetByIdAsync(id);
                if (result == null)
                {
                    _logger.LogWarning("No se encontró la aplicación con ID {ApplicationId}.", id);
                    return NotFound(ResponseApi<ApplicationDTO>.Error("Aplicación no encontrada."));
                }
                _logger.LogInformation("Aplicación encontrada: ID={ApplicationId}, Nombre={Name}", result.ApplicationId, result.Name);
                return Ok(ResponseApi<ApplicationDTO>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la aplicación con ID {ApplicationId}.", id);
                return StatusCode(500, ResponseApi<ApplicationDTO>.Error("Error interno del servidor."));
            }
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetApplications([FromQuery] string filterField, [FromQuery] string filterValue)
        {
            try
            {
                if (string.IsNullOrEmpty(filterField) || string.IsNullOrEmpty(filterValue))
                {
                    return BadRequest(ResponseApi<IEnumerable<ApplicationDTO>>.Error("Debe proporcionar un campo y un valor para filtrar."));
                }
                var propertyInfo = typeof(ApplicationDTO).GetProperty(filterField);
                if (propertyInfo == null)
                {
                    return BadRequest(ResponseApi<IEnumerable<ApplicationDTO>>.Error($"El campo '{filterField}' no existe en ApplicationDTO."));
                }
                object typedValue;
                try
                {
                    typedValue = Convert.ChangeType(filterValue, propertyInfo.PropertyType);
                }
                catch (Exception)
                {
                    return BadRequest(ResponseApi<IEnumerable<ApplicationDTO>>.Error($"El valor '{filterValue}' no se puede convertir al tipo {propertyInfo.PropertyType.Name}."));
                }
                ParameterExpression param = Expression.Parameter(typeof(ApplicationDTO), "dto");
                MemberExpression property = Expression.Property(param, filterField);
                ConstantExpression constant = Expression.Constant(typedValue, propertyInfo.PropertyType);
                BinaryExpression comparison = Expression.Equal(property, constant);
                Expression<Func<ApplicationDTO, bool>> filter = Expression.Lambda<Func<ApplicationDTO, bool>>(comparison, param);
                var result = await _service.GetAllAsync(new List<Expression<Func<ApplicationDTO, bool>>> { filter });
                return Ok(ResponseApi<IEnumerable<ApplicationDTO>>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener aplicaciones con filtro.");
                return StatusCode(500, ResponseApi<IEnumerable<ApplicationDTO>>.Error("Error interno del servidor."));
            }
        }

        [HttpGet("filters")]
        public async Task<IActionResult> GetApplications([FromQuery] Dictionary<string, string> filters)
        {
            try
            {
                if (filters == null || filters.Count == 0)
                {
                    return BadRequest(ResponseApi<IEnumerable<ApplicationDTO>>.Error("Debe proporcionar al menos un filtro."));
                }
                ParameterExpression param = Expression.Parameter(typeof(ApplicationDTO), "dto");
                Expression finalExpression = null;
                foreach (var filter in filters)
                {
                    var propertyInfo = typeof(ApplicationDTO).GetProperty(filter.Key);
                    if (propertyInfo == null)
                    {
                        return BadRequest(ResponseApi<IEnumerable<ApplicationDTO>>.Error($"El campo '{filter.Key}' no existe en ApplicationDTO."));
                    }
                    object typedValue;
                    try
                    {
                        typedValue = Convert.ChangeType(filter.Value, propertyInfo.PropertyType);
                    }
                    catch (Exception)
                    {
                        return BadRequest(ResponseApi<IEnumerable<ApplicationDTO>>.Error($"El valor '{filter.Value}' no se puede convertir al tipo {propertyInfo.PropertyType.Name}."));
                    }
                    MemberExpression property = Expression.Property(param, propertyInfo);
                    ConstantExpression constant = Expression.Constant(typedValue, propertyInfo.PropertyType);
                    BinaryExpression comparison = Expression.Equal(property, constant);
                    finalExpression = finalExpression == null ? comparison : Expression.AndAlso(finalExpression, comparison);
                }
                var filterExpression = Expression.Lambda<Func<ApplicationDTO, bool>>(finalExpression, param);
                var result = await _service.GetAllAsync(new List<Expression<Func<ApplicationDTO, bool>>> { filterExpression });
                return Ok(ResponseApi<IEnumerable<ApplicationDTO>>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener aplicaciones con múltiples filtros.");
                return StatusCode(500, ResponseApi<IEnumerable<ApplicationDTO>>.Error("Error interno del servidor."));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create([FromBody] ApplicationDTO applicationDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Se recibió una solicitud con datos inválidos para crear una aplicación.");
                return BadRequest(ResponseApi<ApplicationDTO>.Error("Datos de entrada inválidos."));
            }
            _logger.LogInformation("Creando nueva aplicación: {Name}", applicationDTO.Name);
            try
            {
                var result = await _service.CreateAsync(applicationDTO);
                if (result == null)
                {
                    _logger.LogWarning("No se pudo crear la aplicación.");
                    return BadRequest(ResponseApi<ApplicationDTO>.Error("No se pudo crear la aplicación."));
                }
                _logger.LogInformation("Aplicación creada con éxito: ID={ApplicationId}, Nombre={Name}", result.ApplicationId, result.Name);
                return CreatedAtAction(nameof(GetById), new { id = result.ApplicationId },
                    ResponseApi<ApplicationDTO>.Success(result, "Aplicación creada con éxito."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la aplicación.");
                return StatusCode(500, ResponseApi<ApplicationDTO>.Error("Error interno del servidor."));
            }
        }

        [HttpPut]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Update([FromBody] ApplicationDTO applicationDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Se recibió una solicitud con datos inválidos para actualizar una aplicación.");
                return BadRequest(ResponseApi<ApplicationDTO>.Error("Datos de entrada inválidos."));
            }
            _logger.LogInformation("Actualizando aplicación con ID: {ApplicationId}, Nombre: {Name}", applicationDTO.ApplicationId, applicationDTO.Name);
            try
            {
                var result = await _service.UpdateAsync(applicationDTO);
                if (result == null)
                {
                    _logger.LogWarning("No se pudo actualizar la aplicación con ID {ApplicationId}.", applicationDTO.ApplicationId);
                    return NotFound(ResponseApi<ApplicationDTO>.Error("Aplicación no encontrada."));
                }
                _logger.LogInformation("Aplicación actualizada con éxito: ID={ApplicationId}, Nombre={Name}", result.ApplicationId, result.Name);
                return Ok(ResponseApi<ApplicationDTO>.Success(result, "Aplicación actualizada correctamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la aplicación con ID {ApplicationId}.", applicationDTO.ApplicationId);
                return StatusCode(500, ResponseApi<ApplicationDTO>.Error("Error interno del servidor."));
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se recibió un ID no válido ({ApplicationId}) en la solicitud de eliminación.", id);
                return BadRequest(ResponseApi<bool>.Error("El ID debe ser mayor a 0."));
            }
            _logger.LogInformation("Eliminando aplicación con ID: {ApplicationId}", id);
            try
            {
                var result = await _service.DeleteAsync(id);
                if (!result)
                {
                    _logger.LogWarning("No se encontró la aplicación con ID {ApplicationId} para eliminar.", id);
                    return NotFound(ResponseApi<bool>.Error("Aplicación no encontrada."));
                }
                _logger.LogInformation("Aplicación eliminada con éxito: ID={ApplicationId}", id);
                return Ok(ResponseApi<bool>.Success(result, "Aplicación eliminada correctamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la aplicación con ID {ApplicationId}.", id);
                return StatusCode(500, ResponseApi<bool>.Error("Error interno del servidor."));
            }
        }
    }
}