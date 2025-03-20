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
    public class UserRoleController : ControllerBase
    {
        private readonly IUserRoleService _service;
        private readonly ILogger<UserRoleController> _logger;
        private readonly IValidator<UserRoleDTO> _validator;    

        public UserRoleController(IUserRoleService service, ILogger<UserRoleController> logger, IValidator<UserRoleDTO> validator)
        {
            _service = service;
            _logger = logger;
            _validator = validator;
        }

        /// <summary>
        /// Obtiene todos los userRole activos.
        /// </summary>
        [HttpGet("active")]
        public async Task<IActionResult> GetAllActive([FromHeader] HeaderDTO header)
        {
            _logger.LogInformation("Iniciando solicitud para obtener todos los UserRole activos.");
            try
            {
                var result = await _service.GetAllActiveAsync();
                if (result == null || !result.Any())
                {
                    _logger.LogWarning("No se encontraron UserRole.");
                    return NotFound(ResponseApi<IEnumerable<UserRoleDTO>>.Error("No se encontraron UserRole."));
                }
                _logger.LogInformation($"{result.Count()} UserRoles recuperados exitosamente.");
                return Ok(ResponseApi<IEnumerable<UserRoleDTO>>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recuperar todos los UserRoles activos.");
                return StatusCode(500, ResponseApi<IEnumerable<UserRoleDTO>>.Error("Error interno del servidor."));
            }
        }

        /// <summary>
        /// Obtiene UserRole basados en un solo filtro.
        /// </summary>
        [HttpGet("filter")]
        public async Task<IActionResult> GetByFilter([FromHeader] HeaderDTO header, [FromQuery] string filterField, [FromQuery] string filterValue)
        {
            try
            {
                if (string.IsNullOrEmpty(filterField) || string.IsNullOrEmpty(filterValue))
                {
                    return BadRequest(ResponseApi<IEnumerable<UserRoleDTO>>.Error("Los campos y valores de filtro son obligatorios."));
                }
                var propertyInfo = typeof(UserRoleDTO).GetProperty(filterField);
                if (propertyInfo == null)
                {
                    return BadRequest(ResponseApi<IEnumerable<UserRoleDTO>>.Error($"El campo '{filterField}' no existe en UserRoleDTO."));
                }
                object typedValue;
                try
                {
                    typedValue = Convert.ChangeType(filterValue, propertyInfo.PropertyType);
                }
                catch (Exception)
                {
                    return BadRequest(ResponseApi<IEnumerable<UserRoleDTO>>.Error($"El valor '{filterValue}' no puede convertirse a tipo {propertyInfo.PropertyType.Name}."));
                }
                ParameterExpression param = Expression.Parameter(typeof(UserRoleDTO), "dto");
                MemberExpression property = Expression.Property(param, filterField);
                ConstantExpression constant = Expression.Constant(typedValue, propertyInfo.PropertyType);
                BinaryExpression comparison = Expression.Equal(property, constant);
                Expression<Func<UserRoleDTO, bool>> filter = Expression.Lambda<Func<UserRoleDTO, bool>>(comparison, param);
                var result = await _service.GetAllAsync(filter);
                return Ok(ResponseApi<IEnumerable<UserRoleDTO>>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recuperar módulos con filtro.");
                return StatusCode(500, ResponseApi<IEnumerable<UserRoleDTO>>.Error("Error interno del servidor."));
            }
        }

        /// <summary>
        /// Obtiene módulos basados en múltiples filtros.
        /// </summary>
        [HttpGet("filters")]
        public async Task<IActionResult> GetByMultipleFilters([FromHeader] HeaderDTO header, [FromQuery] Dictionary<string, string> filters)
        {
            try
            {
                if (filters == null || !filters.Any())
                {
                    return BadRequest(ResponseApi<IEnumerable<UserRoleDTO>>.Error("Se requiere al menos un filtro."));
                }

                List<Expression<Func<UserRoleDTO, bool>>> filterExpressions = new();

                foreach (var filter in filters)
                {
                    string filterField = filter.Key;
                    string filterValue = filter.Value;

                    var propertyInfo = typeof(UserRoleDTO).GetProperty(filterField);
                    if (propertyInfo == null)
                    {
                        return BadRequest(ResponseApi<IEnumerable<UserRoleDTO>>.Error($"El campo '{filterField}' no existe en UserRoleDTO."));
                    }

                    object typedValue;
                    try
                    {
                        typedValue = Convert.ChangeType(filterValue, propertyInfo.PropertyType);
                    }
                    catch (Exception)
                    {
                        return BadRequest(ResponseApi<IEnumerable<UserRoleDTO>>.Error($"El valor '{filterValue}' no puede convertirse a tipo {propertyInfo.PropertyType.Name}."));
                    }

                    // Crear expresión de filtro
                    ParameterExpression param = Expression.Parameter(typeof(UserRoleDTO), "dto");
                    MemberExpression property = Expression.Property(param, filterField);
                    ConstantExpression constant = Expression.Constant(typedValue, propertyInfo.PropertyType);
                    BinaryExpression comparison = Expression.Equal(property, constant);
                    Expression<Func<UserRoleDTO, bool>> filterExpression = Expression.Lambda<Func<UserRoleDTO, bool>>(comparison, param);

                    filterExpressions.Add(filterExpression);
                }

                // Llamar al servicio con múltiples filtros
                var result = await _service.GetAllAsync(filterExpressions);

                return Ok(ResponseApi<IEnumerable<UserRoleDTO>>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recuperar módulos con múltiples filtros.");
                return StatusCode(500, ResponseApi<IEnumerable<UserRoleDTO>>.Error("Error interno del servidor."));
            }
        }

        [HttpPost("ByCodes")]
        public async Task<IActionResult> GetByCodes([FromHeader] HeaderDTO header, UserRoleDTO userRoleDTO)
        {
            if (userRoleDTO == null)
            {
                _logger.LogWarning("Se recibió un UserRole no válido (UserCode: {UserCode}, RoleCode: {RoleCode}) en la solicitud de búsqueda.", userRoleDTO.UserCode, userRoleDTO.RoleCode);
                return BadRequest(ResponseApi<UserRoleDTO>.Error("El UserCode o RoleCode no debe ser nulo o vacio"));
            }
            _logger.LogInformation("Buscando UserRole con UserCode: {UserCode}, RoleCode: {RoleCode}", userRoleDTO.UserCode, userRoleDTO.RoleCode);
            try
            {
                var result = await _service.GetByUserCodeRoleCodeAsync(userRoleDTO.UserCode, userRoleDTO.RoleCode);
                if (result == null)
                {
                    _logger.LogWarning("No se encontró el UserRole con UserCode: {UserCode}, RoleCode: {RoleCode}.", userRoleDTO.UserCode, userRoleDTO.RoleCode);
                    return NotFound(ResponseApi<UserRoleDTO>.Error("UserRole no encontrada."));
                }
                _logger.LogInformation("UserRole encontrada: UserCode: {UserCode}, RoleCode: {RoleCode}", userRoleDTO.UserCode, userRoleDTO.RoleCode);
                return Ok(ResponseApi<UserRoleDTO>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el UserRole con UserCode: {UserCode}, RoleCode: {RoleCode}", userRoleDTO.UserCode, userRoleDTO.RoleCode);
                return StatusCode(500, ResponseApi<UserRoleDTO>.Error("Error interno del servidor."));
            }
        }


        /// <summary>
        /// Crea un nuevo módulo.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromHeader] HeaderDTO header, [FromBody] UserRoleDTO userRoleDTO)
        {
            if (userRoleDTO == null)
            {
                _logger.LogWarning("Se recibió una solicitud con datos nulos para crear un UserRole.");
                return BadRequest(ResponseApi<UserRoleDTO>.Error("Los datos del UserRole no pueden ser nulos."));
            }

            var validationResult = await _validator.ValidateAsync(userRoleDTO);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(ResponseApi<UserRoleDTO>.Error(errors));
            }

            _logger.LogInformation("Creando nuevo UserRole con UserCode: {UserCode}, RoleCode: {RoleCode}", userRoleDTO.UserCode, userRoleDTO.RoleCode);
            try
            {
                var result = await _service.CreateAsync(header, userRoleDTO);
                if (result == null)
                {
                    _logger.LogWarning("No se pudo crear el UserRole con UserCode: {UserCode}, RoleCode: {RoleCode}", userRoleDTO.UserCode, userRoleDTO.RoleCode);
                    return BadRequest(ResponseApi<UserRoleDTO>.Error("No se pudo crear el UserRole."));
                }

                _logger.LogInformation("UserRole creado con éxito con UserCode: {UserCode}, RoleCode: {RoleCode}", userRoleDTO.UserCode, userRoleDTO.RoleCode);
                return CreatedAtAction(nameof(GetByUserCodeRoleCode), new { userRoleDTO = result },
                    ResponseApi<UserRoleDTO>.Success(result, "UserRole creado con éxito."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el UserRole.");
                return StatusCode(500, ResponseApi<UserRoleDTO>.Error("Error interno del servidor."));
            }
        }


        /// <summary>
        /// Actualiza un módulo existente.
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> Update([FromHeader] HeaderDTO header, [FromBody] UserRoleDTO userRoleDTO)
        {
            if (userRoleDTO == null)
            {
                _logger.LogWarning("Se recibió una solicitud con datos nulos para modificar un UserRole.");
                return BadRequest(ResponseApi<UserRoleDTO>.Error("Los datos del UserRole no pueden ser nulos."));
            }
            var validationResult = await _validator.ValidateAsync(userRoleDTO);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(ResponseApi<UserRoleDTO>.Error(errors));
            }
            _logger.LogInformation("Actualizando UserRole con UserCode: {UserCode}, RoleCode: {RoleCode}", userRoleDTO.UserCode, userRoleDTO.RoleCode);
            try
            {
                var result = await _service.UpdateAsync(header, userRoleDTO);
                if (result == null)
                {
                    _logger.LogWarning("UserRole con UserCode: {UserCode}, RoleCode: {RoleCode} no encontrado.", userRoleDTO.UserCode, userRoleDTO.RoleCode);
                    return NotFound(ResponseApi<UserRoleDTO>.Error("Módulo no encontrado."));
                }
                _logger.LogInformation("UserRole creado exitosamente con UserCode: {UserCode}, RoleCode: {RoleCode}", userRoleDTO.UserCode, userRoleDTO.RoleCode);
                return Ok(ResponseApi<UserRoleDTO>.Success(result, "UserRole actualizado exitosamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar UserRole con UserCode: {UserCode}, RoleCode: {RoleCode}", userRoleDTO.UserCode, userRoleDTO.RoleCode);
                return StatusCode(500, ResponseApi<UserRoleDTO>.Error("Error interno del servidor."));
            }
        }

        /// <summary>
        /// Elimina un módulo por su ID.
        /// </summary>
        [HttpDelete]
        public async Task<IActionResult> Deactivate([FromHeader] HeaderDTO header, UserRoleDTO userRoleDTO)
        {
            if (userRoleDTO == null)
            {
                _logger.LogWarning("Code no válido recibido (UserCode: {UserCode}, RoleCode: {RoleCode}) en la solicitud de eliminación.", userRoleDTO.UserCode, userRoleDTO.RoleCode);
                return BadRequest(ResponseApi<bool>.Error("El ModuleCode debe ser nulo o vacio."));
            }
            _logger.LogInformation("Desactivado UserRole con UserCode: {UserCode}, RoleCode: {RoleCode}", userRoleDTO.UserCode, userRoleDTO.RoleCode);
            try
            {
                var result = await _service.DeactivateAsync(header, userRoleDTO.UserCode, userRoleDTO.RoleCode);
                if (!result)
                {
                    _logger.LogWarning("UserRole con UserCode: {UserCode}, RoleCode: {RoleCode} no encontrado.", userRoleDTO.UserCode, userRoleDTO.RoleCode);
                    return NotFound(ResponseApi<bool>.Error("UserRole no encontrado."));
                }
                _logger.LogInformation("UserRole desactivado exitosamente: UserCode: {UserCode}, RoleCode: {RoleCode}", userRoleDTO.UserCode, userRoleDTO.RoleCode);
                return Ok(ResponseApi<bool>.Success(result, "UserRole desactivado exitosamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al desactivado UserRole con UserCode: {UserCode}, RoleCode: {RoleCode}", userRoleDTO.UserCode, userRoleDTO.RoleCode);
                return StatusCode(500, ResponseApi<bool>.Error("Error interno del servidor."));
            }
        }
    }
}