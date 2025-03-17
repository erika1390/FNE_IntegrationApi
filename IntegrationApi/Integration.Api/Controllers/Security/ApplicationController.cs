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
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationService _service;
        private readonly ILogger<ApplicationController> _logger;
        private readonly IValidator<ApplicationDTO> _validator;

        public ApplicationController(IApplicationService service, ILogger<ApplicationController> logger, IValidator<ApplicationDTO> validator)
        {
            _service = service;
            _logger = logger;
            _validator = validator;
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetAllActive([FromHeader] HeaderDTO header)
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

        [HttpGet("{code}")]
        public async Task<IActionResult> GetByCode([FromHeader] HeaderDTO header, string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                _logger.LogWarning("Se recibió un ApplicationCode no válido ({ApplicationCode}) en la solicitud de búsqueda.", code);
                return BadRequest(ResponseApi<ApplicationDTO>.Error("El ApplicationCode no debe ser nulo o vacío"));
            }
            _logger.LogInformation("Buscando aplicación con ApplicationCode: {ApplicationCode}", code);
            try
            {
                var result = await _service.GetByCodeAsync(code);
                if (result == null)
                {
                    _logger.LogWarning("No se encontró la aplicación con ApplicationCode {ApplicationCode}.", code);
                    return NotFound(ResponseApi<ApplicationDTO>.Error("Aplicación no encontrada."));
                }
                _logger.LogInformation("Aplicación encontrada: ApplicationCode={ApplicationCode}, Nombre={Name}", result.Code, result.Name);
                return Ok(ResponseApi<ApplicationDTO>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la aplicación con ApplicationCode {ApplicationCode}.", code);
                return StatusCode(500, ResponseApi<ApplicationDTO>.Error("Error interno del servidor."));
            }
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetApplications([FromHeader] HeaderDTO header, [FromQuery] string filterField, [FromQuery] string filterValue)
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
        public async Task<IActionResult> GetApplications([FromHeader] HeaderDTO header, [FromQuery] Dictionary<string, string> filters)
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
        public async Task<IActionResult> Create([FromHeader] HeaderDTO header, [FromBody] ApplicationDTO applicationDTO)
        {
            if (applicationDTO == null)
            {
                _logger.LogWarning("Se recibió una solicitud con datos nulos para crear una aplicación.");
                return BadRequest(ResponseApi<ApplicationDTO>.Error("Los datos de la aplicación no pueden ser nulos."));
            }

            var validationResult = await _validator.ValidateAsync(applicationDTO);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(ResponseApi<ApplicationDTO>.Error(errors));
            }

            _logger.LogInformation("Creando nueva aplicación: {Name}", applicationDTO.Name);
            try
            {
                var result = await _service.CreateAsync(header, applicationDTO);
                if (result == null)
                {
                    _logger.LogWarning("No se pudo crear la aplicación.");
                    return BadRequest(ResponseApi<ApplicationDTO>.Error("No se pudo crear la aplicación."));
                }
                _logger.LogInformation("Aplicación creada con éxito: Código={Code}, Nombre={Name}", result.Code, result.Name);
                return CreatedAtAction(nameof(GetByCode), new { code = result.Code },
                    ResponseApi<ApplicationDTO>.Success(result, "Aplicación creada con éxito."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la aplicación.");
                return StatusCode(500, ResponseApi<ApplicationDTO>.Error("Error interno del servidor."));
            }
        }


        [HttpPut]
        public async Task<IActionResult> Update([FromHeader] HeaderDTO header, [FromBody] ApplicationDTO applicationDTO)
        {
            if (applicationDTO == null)
            {
                _logger.LogWarning("Se recibió una solicitud con datos nulos para modificar una aplicación.");
                return BadRequest(ResponseApi<ApplicationDTO>.Error("Los datos de la aplicación no pueden ser nulos."));
            }

            var validationResult = await _validator.ValidateAsync(applicationDTO);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(ResponseApi<ApplicationDTO>.Error(errors));
            }

            _logger.LogInformation("Actualizando aplicación con ApplicatioCode: {ApplicatioCode}, Nombre: {Name}", applicationDTO.Code, applicationDTO.Name);
            try
            {
                var result = await _service.UpdateAsync(header, applicationDTO);
                if (result == null)
                {
                    _logger.LogWarning("No se pudo actualizar la aplicación con ApplicatioCode {ApplicatioCode}.", applicationDTO.Code);
                    return NotFound(ResponseApi<ApplicationDTO>.Error("Aplicación no encontrada."));
                }
                _logger.LogInformation("Aplicación actualizada con éxito: ApplicatioCode={ApplicatioCode}, Nombre={Name}", result.Code, result.Name);
                return Ok(ResponseApi<ApplicationDTO>.Success(result, "Aplicación actualizada correctamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la aplicación con ApplicatioCode {ApplicatioCode}.", applicationDTO.Code);
                return StatusCode(500, ResponseApi<ApplicationDTO>.Error("Error interno del servidor."));
            }
        }

        [HttpDelete("{code}")]
        public async Task<IActionResult> Deactivate([FromHeader] HeaderDTO header, string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                _logger.LogWarning("Se recibió un Code no válido ({ApplicationCode}) en la solicitud de eliminación.", code);
                return BadRequest(ResponseApi<bool>.Error("El Code debe ser nulo o vacío"));
            }
            _logger.LogInformation("Eliminando aplicación con Code: {ApplicationCode}", code);
            try
            {
                var result = await _service.DeactivateAsync(header, code);
                if (!result)
                {
                    _logger.LogWarning("No se encontró la aplicación con ApplicationCode {ApplicationCode} para eliminar.", code);
                    return NotFound(ResponseApi<bool>.Error("Aplicación no encontrada."));
                }
                _logger.LogInformation("Aplicación eliminada con éxito: ApplicationCode={ApplicationCode}", code);
                return Ok(ResponseApi<bool>.Success(result, "Aplicación eliminada correctamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la aplicación con ApplicationCode {ApplicationCode}.", code);
                return StatusCode(500, ResponseApi<bool>.Error("Error interno del servidor."));
            }
        }
    }
}