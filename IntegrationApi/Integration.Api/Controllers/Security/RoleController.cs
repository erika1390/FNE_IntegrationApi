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
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _service;
        private readonly ILogger<RoleController> _logger;
        public RoleController(IRoleService service, ILogger<RoleController> logger)
        {
            _service = service;
            _logger = logger;
        }
        [HttpGet("active")]
        public async Task<IActionResult> GetAllActive()
        {
            _logger.LogInformation("Iniciando solicitud para obtener todos los roles.");

            try
            {
                var result = await _service.GetAllActiveAsync();

                if (result == null || !result.Any())
                {
                    _logger.LogWarning("No se encontraron roles.");
                    return NotFound(ResponseApi<IEnumerable<RoleDTO>>.Error("No se encontraron roles."));
                }

                _logger.LogInformation("{Count} roles obtenidas correctamente.", result.Count());
                return Ok(ResponseApi<IEnumerable<RoleDTO>>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas los roles.");
                return StatusCode(500, ResponseApi<IEnumerable<RoleDTO>>.Error("Error interno del servidor."));
            }
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se recibió un ID no válido ({RoleId}) en la solicitud de búsqueda.", id);
                return BadRequest(ResponseApi<RoleDTO>.Error("El ID debe ser mayor a 0."));
            }
            _logger.LogInformation("Buscando roles con ID: {RoleId}", id);
            try
            {
                var result = await _service.GetByIdAsync(id);
                if (result == null)
                {
                    _logger.LogWarning("No se encontró el rol con ID {RoleId}.", id);
                    return NotFound(ResponseApi<RoleDTO>.Error("Rol no encontrada."));
                }

                _logger.LogInformation("Rol encontrada: ID={RoleId}, Nombre={Name}", result.RoleId, result.Name);
                return Ok(ResponseApi<RoleDTO>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el rol con ID {RoleId}.", id);
                return StatusCode(500, ResponseApi<RoleDTO>.Error("Error interno del servidor."));
            }
        }
        [HttpGet("filter")]
        public async Task<IActionResult> GetRoles([FromQuery] string filterField, [FromQuery] string filterValue)
        {
            try
            {
                if (string.IsNullOrEmpty(filterField) || string.IsNullOrEmpty(filterValue))
                {
                    return BadRequest(ResponseApi<RoleDTO>.Error("Debe proporcionar un campo y un valor para filtrar."));
                }
                var propertyInfo = typeof(RoleDTO).GetProperty(filterField);
                if (propertyInfo == null)
                {
                    return BadRequest(ResponseApi<RoleDTO>.Error($"El campo '{filterField}' no existe en RoleDTO."));
                }
                object typedValue;
                try
                {
                    typedValue = Convert.ChangeType(filterValue, propertyInfo.PropertyType);
                }
                catch (Exception)
                {
                    return BadRequest(ResponseApi<RoleDTO>.Error($"El valor '{filterValue}' no se puede convertir al tipo {propertyInfo.PropertyType.Name}."));
                }
                ParameterExpression param = Expression.Parameter(typeof(RoleDTO), "dto");
                MemberExpression property = Expression.Property(param, filterField);
                ConstantExpression constant = Expression.Constant(typedValue, propertyInfo.PropertyType);
                BinaryExpression comparison = Expression.Equal(property, constant);
                Expression<Func<RoleDTO, bool>> filter = Expression.Lambda<Func<RoleDTO, bool>>(comparison, param);
                var result = await _service.GetAllAsync(new List<Expression<Func<RoleDTO, bool>>> { filter });
                return Ok(ResponseApi<IEnumerable<RoleDTO>>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener roles con filtro.");
                return StatusCode(500, ResponseApi<IEnumerable<RoleDTO>>.Error("Error interno del servidor."));
            }
        }
        [HttpGet("filters")]
        public async Task<IActionResult> GetRoles([FromQuery] Dictionary<string, string> filters)
        {
            try
            {
                if (filters == null || filters.Count == 0)
                {
                    return BadRequest(ResponseApi<RoleDTO>.Error("Debe proporcionar al menos un filtro."));
                }
                ParameterExpression param = Expression.Parameter(typeof(RoleDTO), "dto");
                Expression finalExpression = null;
                foreach (var filter in filters)
                {
                    var propertyInfo = typeof(RoleDTO).GetProperty(filter.Key);
                    if (propertyInfo == null)
                    {
                        return BadRequest(ResponseApi<RoleDTO>.Error($"El campo '{filter.Key}' no existe en RoleDTO."));
                    }
                    object typedValue;
                    try
                    {
                        typedValue = Convert.ChangeType(filter.Value, propertyInfo.PropertyType);
                    }
                    catch (Exception)
                    {
                        return BadRequest(ResponseApi<RoleDTO>.Error($"El valor '{filter.Value}' no se puede convertir al tipo {propertyInfo.PropertyType.Name}."));
                    }
                    MemberExpression property = Expression.Property(param, propertyInfo);
                    ConstantExpression constant = Expression.Constant(typedValue, propertyInfo.PropertyType);
                    BinaryExpression comparison = Expression.Equal(property, constant);
                    finalExpression = finalExpression == null ? comparison : Expression.AndAlso(finalExpression, comparison);
                }
                var filterExpression = Expression.Lambda<Func<RoleDTO, bool>>(finalExpression, param);
                var result = await _service.GetAllAsync(new List<Expression<Func<RoleDTO, bool>>> { filterExpression });
                return Ok(ResponseApi<IEnumerable<RoleDTO>>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener roles con múltiples filtros.");
                return StatusCode(500, ResponseApi<IEnumerable<RoleDTO>>.Error("Error interno del servidor."));
            }
        }
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create([FromBody] RoleDTO permissionDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Se recibió una solicitud con datos inválidos para crear un rol.");
                return BadRequest(ResponseApi<RoleDTO>.Error("Datos de entrada inválidos."));
            }
            _logger.LogInformation("Creando nuevo rol: {Name}", permissionDTO.Name);
            try
            {
                var result = await _service.CreateAsync(permissionDTO);
                if (result == null)
                {
                    _logger.LogWarning("No se pudo crear el rol.");
                    return BadRequest(ResponseApi<RoleDTO>.Error("No se pudo crear el rol."));
                }
                _logger.LogInformation("Rol creado con éxito: ID={RoleId}, Nombre={Name}", result.RoleId, result.Name);
                return CreatedAtAction(nameof(GetById), new { id = result.RoleId },
                    ResponseApi<RoleDTO>.Success(result, "Rol creada con éxito."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la rol.");
                return StatusCode(500, ResponseApi<RoleDTO>.Error("Error interno del servidor."));
            }
        }

        [HttpPut]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Update([FromBody] RoleDTO roleDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Se recibió una solicitud con datos inválidos para actualizar un rol.");
                return BadRequest(ResponseApi<RoleDTO>.Error("Datos de entrada inválidos."));
            }
            _logger.LogInformation("Actualizando rol con ID: {RoleId}, Nombre: {Name}", roleDTO.RoleId, roleDTO.Name);
            try
            {
                var result = await _service.UpdateAsync(roleDTO);
                if (result == null)
                {
                    _logger.LogWarning("No se pudo actualizar el rol con ID {RoleId}.", roleDTO.RoleId);
                    return NotFound(ResponseApi<RoleDTO>.Error("Rol no encontrada."));
                }
                _logger.LogInformation("Rol actualizada con éxito: ID={RoleId}, Nombre={Name}", result.RoleId, result.Name);
                return Ok(ResponseApi<RoleDTO>.Success(result, "Rol actualizada correctamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el rol con ID {RoleId}.", roleDTO.RoleId);
                return StatusCode(500, ResponseApi<RoleDTO>.Error("Error interno del servidor."));
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se recibió un ID no válido ({RoleId}) en la solicitud de eliminación.", id);
                return BadRequest(ResponseApi<bool>.Error("El ID debe ser mayor a 0."));
            }
            _logger.LogInformation("Eliminando rol con ID: {RoleId}", id);
            try
            {
                var result = await _service.DeleteAsync(id);
                if (!result)
                {
                    _logger.LogWarning("No se encontró el rol con ID {RoleId} para eliminar.", id);
                    return NotFound(ResponseApi<bool>.Error("Rol no encontrada."));
                }
                _logger.LogInformation("Rol eliminada con éxito: ID={RoleId}", id);
                return Ok(ResponseApi<bool>.Success(result, "Rol eliminada correctamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el rol con ID {RoleId}.", id);
                return StatusCode(500, ResponseApi<bool>.Error("Error interno del servidor."));
            }
        }
    }
}
