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
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _service;
        private readonly ILogger<MenuController> _logger;
        private readonly IValidator<MenuDTO> _validator;

        public MenuController(IMenuService service, ILogger<MenuController> logger, IValidator<MenuDTO> validator)
        {
            _service = service;
            _logger = logger;
            _validator = validator;
        }
        /// <summary>
        /// Obtiene todos los menus activos.
        /// </summary>
        [HttpGet("active")]
        public async Task<IActionResult> GetAllActive([FromHeader] HeaderDTO header)
        {
            _logger.LogInformation("Iniciando solicitud para obtener todos los menus activos.");
            try
            {
                var result = await _service.GetAllActiveAsync();
                if (result == null || !result.Any())
                {
                    _logger.LogWarning("No se encontraron menus.");
                    return NotFound(ResponseApi<IEnumerable<MenuDTO>>.Error("No se encontraron menus."));
                }
                _logger.LogInformation($"{result.Count()} menus recuperados exitosamente.");
                return Ok(ResponseApi<IEnumerable<MenuDTO>>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recuperar todos los menus activos.");
                return StatusCode(500, ResponseApi<IEnumerable<MenuDTO>>.Error("Error interno del servidor."));
            }
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> GetByCode([FromHeader] HeaderDTO header, string code)
        {
            if (code == null)
            {
                _logger.LogWarning("Se recibió un MenuCode no válido ({MenuCode}) en la solicitud de búsqueda.", code);
                return BadRequest(ResponseApi<MenuDTO>.Error("El MenuCode no debe ser nulo o vacio"));
            }
            _logger.LogInformation("Buscando menu con MenuCode: {MenuCode}", code);
            try
            {
                var result = await _service.GetByCodeAsync(code);
                if (result == null)
                {
                    _logger.LogWarning("No se encontró el Menu con MenuCode {MenuCode}.", code);
                    return NotFound(ResponseApi<MenuDTO>.Error("Menu no encontrada."));
                }
                _logger.LogInformation("Menu encontrada: MenuCode={MenuCode}, Nombre={Name}", result.Code, result.Name);
                return Ok(ResponseApi<MenuDTO>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el Menu con MenuCode {MenuCode}.", code);
                return StatusCode(500, ResponseApi<MenuDTO>.Error("Error interno del servidor."));
            }
        }

        /// <summary>
        /// Obtiene menus basados en un solo filtro.
        /// </summary>
        [HttpGet("filter")]
        public async Task<IActionResult> GetByFilter([FromHeader] HeaderDTO header, [FromQuery] string filterField, [FromQuery] string filterValue)
        {
            try
            {
                if (string.IsNullOrEmpty(filterField) || string.IsNullOrEmpty(filterValue))
                {
                    return BadRequest(ResponseApi<IEnumerable<MenuDTO>>.Error("Los campos y valores de filtro son obligatorios."));
                }
                var propertyInfo = typeof(MenuDTO).GetProperty(filterField);
                if (propertyInfo == null)
                {
                    return BadRequest(ResponseApi<IEnumerable<MenuDTO>>.Error($"El campo '{filterField}' no existe en MenuDTO."));
                }
                object typedValue;
                try
                {
                    typedValue = Convert.ChangeType(filterValue, propertyInfo.PropertyType);
                }
                catch (Exception)
                {
                    return BadRequest(ResponseApi<IEnumerable<MenuDTO>>.Error($"El valor '{filterValue}' no puede convertirse a tipo {propertyInfo.PropertyType.Name}."));
                }
                ParameterExpression param = Expression.Parameter(typeof(MenuDTO), "dto");
                MemberExpression property = Expression.Property(param, filterField);
                ConstantExpression constant = Expression.Constant(typedValue, propertyInfo.PropertyType);
                BinaryExpression comparison = Expression.Equal(property, constant);
                Expression<Func<MenuDTO, bool>> filter = Expression.Lambda<Func<MenuDTO, bool>>(comparison, param);
                var result = await _service.GetByFilterAsync(filter);
                return Ok(ResponseApi<IEnumerable<MenuDTO>>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recuperar menus con filtro.");
                return StatusCode(500, ResponseApi<IEnumerable<MenuDTO>>.Error("Error interno del servidor."));
            }
        }

        /// <summary>
        /// Obtiene menus basados en múltiples filtros.
        /// </summary>
        [HttpGet("filters")]
        public async Task<IActionResult> GetByMultipleFilters([FromHeader] HeaderDTO header, [FromQuery] Dictionary<string, string> filters)
        {
            try
            {
                if (filters == null || !filters.Any())
                {
                    return BadRequest(ResponseApi<IEnumerable<MenuDTO>>.Error("Se requiere al menos un filtro."));
                }

                List<Expression<Func<MenuDTO, bool>>> filterExpressions = new();

                foreach (var filter in filters)
                {
                    string filterField = filter.Key;
                    string filterValue = filter.Value;

                    var propertyInfo = typeof(MenuDTO).GetProperty(filterField);
                    if (propertyInfo == null)
                    {
                        return BadRequest(ResponseApi<IEnumerable<MenuDTO>>.Error($"El campo '{filterField}' no existe en MenuDTO."));
                    }

                    object typedValue;
                    try
                    {
                        typedValue = Convert.ChangeType(filterValue, propertyInfo.PropertyType);
                    }
                    catch (Exception)
                    {
                        return BadRequest(ResponseApi<IEnumerable<MenuDTO>>.Error($"El valor '{filterValue}' no puede convertirse a tipo {propertyInfo.PropertyType.Name}."));
                    }

                    // Crear expresión de filtro
                    ParameterExpression param = Expression.Parameter(typeof(MenuDTO), "dto");
                    MemberExpression property = Expression.Property(param, filterField);
                    ConstantExpression constant = Expression.Constant(typedValue, propertyInfo.PropertyType);
                    BinaryExpression comparison = Expression.Equal(property, constant);
                    Expression<Func<MenuDTO, bool>> filterExpression = Expression.Lambda<Func<MenuDTO, bool>>(comparison, param);

                    filterExpressions.Add(filterExpression);
                }

                // Llamar al servicio con múltiples filtros
                var result = await _service.GetByMultipleFiltersAsync(filterExpressions);

                return Ok(ResponseApi<IEnumerable<MenuDTO>>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recuperar menus con múltiples filtros.");
                return StatusCode(500, ResponseApi<IEnumerable<MenuDTO>>.Error("Error interno del servidor."));
            }
        }

        /// <summary>
        /// Crea un nuevo Menu.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromHeader] HeaderDTO header, [FromBody] MenuDTO MenuDTO)
        {
            if (MenuDTO == null)
            {
                _logger.LogWarning("Se recibió una solicitud con datos nulos para crear un Menu.");
                return BadRequest(ResponseApi<MenuDTO>.Error("Los datos del Menu no pueden ser nulos."));
            }

            var validationResult = await _validator.ValidateAsync(MenuDTO);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(ResponseApi<MenuDTO>.Error(errors));
            }

            _logger.LogInformation("Creando nuevo Menu: {Name}", MenuDTO.Name);
            try
            {
                var result = await _service.CreateAsync(header, MenuDTO);
                if (result == null)
                {
                    _logger.LogWarning("No se pudo crear el Menu.");
                    return BadRequest(ResponseApi<MenuDTO>.Error("No se pudo crear el Menu."));
                }

                _logger.LogInformation("Menu creado con éxito: MenuCode={MenuCode}, Nombre={Name}", result.Code, result.Name);
                return CreatedAtAction(nameof(GetByCode), new { code = result.Code },
                    ResponseApi<MenuDTO>.Success(result, "Menu creado con éxito."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el Menu.");
                return StatusCode(500, ResponseApi<MenuDTO>.Error("Error interno del servidor."));
            }
        }

        /// <summary>
        /// Actualiza un Menu existente.
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> Update([FromHeader] HeaderDTO header, [FromBody] MenuDTO MenuDTO)
        {
            if (MenuDTO == null)
            {
                _logger.LogWarning("Se recibió una solicitud con datos nulos para modificar un Menu.");
                return BadRequest(ResponseApi<MenuDTO>.Error("Los datos del Menu no pueden ser nulos."));
            }
            var validationResult = await _validator.ValidateAsync(MenuDTO);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(ResponseApi<MenuDTO>.Error(errors));
            }
            _logger.LogInformation("Actualizando Menu con MenuCode: {MenuCode}, Name: {Name}", MenuDTO.ModuleCode, MenuDTO.Name);
            try
            {
                var result = await _service.UpdateAsync(header, MenuDTO);
                if (result == null)
                {
                    _logger.LogWarning("Menu con MenuCode {MenuCode} no encontrado.", MenuDTO.ModuleCode);
                    return NotFound(ResponseApi<MenuDTO>.Error("Menu no encontrado."));
                }
                _logger.LogInformation("Menu creado exitosamente: MenuCode={MenuCode}, Name={Name}", result.ModuleCode, result.Name);
                return Ok(ResponseApi<MenuDTO>.Success(result, "Menu actualizado exitosamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar Menu con MenuCode {MenuCode}.", MenuDTO.ModuleCode);
                return StatusCode(500, ResponseApi<MenuDTO>.Error("Error interno del servidor."));
            }
        }

        /// <summary>
        /// Elimina un Menu por su ID.
        /// </summary>
        [HttpDelete("{code}")]
        public async Task<IActionResult> Deactivate([FromHeader] HeaderDTO header, string code)
        {
            if (code == null)
            {
                _logger.LogWarning("Code no válido recibido ({MenuCode}) en la solicitud de eliminación.", code);
                return BadRequest(ResponseApi<bool>.Error("El MenuCode debe ser nulo o vacio."));
            }
            _logger.LogInformation("Eliminando Menu con MenuCode: {MenuCode}", code);
            try
            {
                var result = await _service.DeactivateAsync(header, code);
                if (!result)
                {
                    _logger.LogWarning("Menu con MenuCode {MenuCode} no encontrado.", code);
                    return NotFound(ResponseApi<bool>.Error("Menu no encontrado."));
                }
                _logger.LogInformation("Menu eliminado exitosamente: MenuCode={MenuCode}", code);
                return Ok(ResponseApi<bool>.Success(result, "Menu eliminado exitosamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar Menu con MenuCode {MenuCode}.", code);
                return StatusCode(500, ResponseApi<bool>.Error("Error interno del servidor."));
            }
        }
    }
}
