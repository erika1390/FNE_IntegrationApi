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
    public class ModuleController : ControllerBase
    {
        private readonly IModuleService _service;
        private readonly ILogger<ModuleController> _logger;
        private readonly IValidator<ModuleDTO> _validator;

        public ModuleController(IModuleService service, ILogger<ModuleController> logger, IValidator<ModuleDTO> validator)
        {
            _service = service;
            _logger = logger;
            _validator = validator;
        }

        /// <summary>
        /// Obtiene todos los módulos activos.
        /// </summary>
        [HttpGet("active")]
        public async Task<IActionResult> GetAllActive([FromHeader] HeaderDTO header)
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
        /// Obtiene módulos basados en un solo filtro.
        /// </summary>
        [HttpGet("filter")]
        public async Task<IActionResult> GetModules([FromHeader] HeaderDTO header,  [FromQuery] string filterField, [FromQuery] string filterValue)
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
                var result = await _service.GetAllAsync(filter);
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
        public async Task<IActionResult> GetModules([FromHeader] HeaderDTO header, [FromQuery] Dictionary<string, string> filters)
        {
            try
            {
                if (filters == null || !filters.Any())
                {
                    return BadRequest(ResponseApi<IEnumerable<ModuleDTO>>.Error("Se requiere al menos un filtro."));
                }

                List<Expression<Func<ModuleDTO, bool>>> filterExpressions = new();

                foreach (var filter in filters)
                {
                    string filterField = filter.Key;
                    string filterValue = filter.Value;

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

                    // Crear expresión de filtro
                    ParameterExpression param = Expression.Parameter(typeof(ModuleDTO), "dto");
                    MemberExpression property = Expression.Property(param, filterField);
                    ConstantExpression constant = Expression.Constant(typedValue, propertyInfo.PropertyType);
                    BinaryExpression comparison = Expression.Equal(property, constant);
                    Expression<Func<ModuleDTO, bool>> filterExpression = Expression.Lambda<Func<ModuleDTO, bool>>(comparison, param);

                    filterExpressions.Add(filterExpression);
                }

                // Llamar al servicio con múltiples filtros
                var result = await _service.GetAllAsync(filterExpressions);

                return Ok(ResponseApi<IEnumerable<ModuleDTO>>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recuperar módulos con múltiples filtros.");
                return StatusCode(500, ResponseApi<IEnumerable<ModuleDTO>>.Error("Error interno del servidor."));
            }
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> GetByCode([FromHeader] HeaderDTO header, string code)
        {
            if (code==null)
            {
                _logger.LogWarning("Se recibió un ModuleCode no válido ({ModuleCode}) en la solicitud de búsqueda.", code);
                return BadRequest(ResponseApi<ModuleDTO>.Error("El ModuleCode no debe ser nulo o vacio"));
            }
            _logger.LogInformation("Buscando modulo con ModuleCode: {ModuleCode}", code);
            try
            {
                var result = await _service.GetByCodeAsync(code);
                if (result == null)
                {
                    _logger.LogWarning("No se encontró el modulo con ModuleCode {ModuleCode}.", code);
                    return NotFound(ResponseApi<ModuleDTO>.Error("Modulo no encontrada."));
                }
                _logger.LogInformation("|Modulo encontrada: ModuleCode={ModuleCode}, Nombre={Name}", result.Code, result.Name);
                return Ok(ResponseApi<ModuleDTO>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el modulo con ModuleCode {ModuleCode}.", code);
                return StatusCode(500, ResponseApi<ModuleDTO>.Error("Error interno del servidor."));
            }
        }


        /// <summary>
        /// Crea un nuevo módulo.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromHeader] HeaderDTO header, [FromBody] ModuleDTO moduleDTO)
        {
            if (moduleDTO == null)
            {
                _logger.LogWarning("Se recibió una solicitud con datos nulos para crear un módulo.");
                return BadRequest(ResponseApi<ModuleDTO>.Error("Los datos del módulo no pueden ser nulos."));
            }

            var validationResult = await _validator.ValidateAsync(moduleDTO);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(ResponseApi<ModuleDTO>.Error(errors));
            }   

            _logger.LogInformation("Creando nuevo módulo: {Name}", moduleDTO.Name);
            try
            {
                var result = await _service.CreateAsync(header, moduleDTO);
                if (result == null)
                {
                    _logger.LogWarning("No se pudo crear el módulo.");
                    return BadRequest(ResponseApi<ModuleDTO>.Error("No se pudo crear el módulo."));
                }

                _logger.LogInformation("Módulo creado con éxito: Código={Code}, Nombre={Name}", result.Code, result.Name);
                return CreatedAtAction(nameof(GetByCode), new { code = result.Code },
                    ResponseApi<ModuleDTO>.Success(result, "Módulo creado con éxito."));
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
        public async Task<IActionResult> Update([FromHeader] HeaderDTO header, [FromBody] ModuleDTO moduleDTO)
        {
            if (moduleDTO == null)
            {
                _logger.LogWarning("Se recibió una solicitud con datos nulos para modificar un modulo.");
                return BadRequest(ResponseApi<ModuleDTO>.Error("Los datos del modulo no pueden ser nulos."));
            }
            var validationResult = await _validator.ValidateAsync(moduleDTO);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(ResponseApi<ModuleDTO>.Error(errors));
            }
            _logger.LogInformation("Actualizando módulo con ModuleCode: {ModuleCode}, Name: {Name}", moduleDTO.Code, moduleDTO.Name);
            try
            {
                var result = await _service.UpdateAsync(header, moduleDTO);
                if (result == null)
                {
                    _logger.LogWarning("Módulo con ModuleCode {ModuleCode} no encontrado.", moduleDTO.Code);
                    return NotFound(ResponseApi<ModuleDTO>.Error("Módulo no encontrado."));
                }
                _logger.LogInformation("Módulo creado exitosamente: ModuleCode={ModuleCode}, Name={Name}", result.Code, result.Name);
                return Ok(ResponseApi<ModuleDTO>.Success(result, "Módulo actualizado exitosamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar módulo con ModuleCode {ModuleCode}.", moduleDTO.Code);
                return StatusCode(500, ResponseApi<ModuleDTO>.Error("Error interno del servidor."));
            }
        }

        /// <summary>
        /// Elimina un módulo por su ID.
        /// </summary>
        [HttpDelete("{code}")]
        public async Task<IActionResult> Delete([FromHeader] HeaderDTO header, string code)
        {
            if (code == null)
            {
                _logger.LogWarning("Code no válido recibido ({ModuleCode}) en la solicitud de eliminación.", code);
                return BadRequest(ResponseApi<bool>.Error("El ModuleCode debe ser nulo o vacio."));
            }
            _logger.LogInformation("Eliminando módulo con ModuleCode: {ModuleCode}", code);
            try
            {
                var result = await _service.DeactivateAsync(header, code);
                if (!result)
                {
                    _logger.LogWarning("Módulo con ModuleCode {ModuleCode} no encontrado.", code);
                    return NotFound(ResponseApi<bool>.Error("Módulo no encontrado."));
                }
                _logger.LogInformation("Módulo eliminado exitosamente: ModuleCode={ModuleCode}", code);
                return Ok(ResponseApi<bool>.Success(result, "Módulo eliminado exitosamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar módulo con ModuleCode {ModuleCode}.", code);
                return StatusCode(500, ResponseApi<bool>.Error("Error interno del servidor."));
            }
        }
    }
}