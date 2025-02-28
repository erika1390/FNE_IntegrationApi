﻿using Integration.Application.Interfaces.Security;
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
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _service;
        private readonly ILogger<PermissionController> _logger;
        public PermissionController(IPermissionService service, ILogger<PermissionController> logger)
        {
            _service = service;
            _logger = logger;
        }
        [HttpGet("active")]
        public async Task<IActionResult> GetAllActive()
        {
            _logger.LogInformation("Iniciando solicitud para obtener todos los permisos.");
            try
            {
                var result = await _service.GetAllActiveAsync();
                if (result == null || !result.Any())
                {
                    _logger.LogWarning("No se encontraron permisos.");
                    return NotFound(ResponseApi<IEnumerable<PermissionDTO>>.Error("No se encontraron permisos."));
                }
                _logger.LogInformation("{Count} permisos obtenidas correctamente.", result.Count());
                return Ok(ResponseApi<IEnumerable<PermissionDTO>>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas los permisos.");
                return StatusCode(500, ResponseApi<IEnumerable<PermissionDTO>>.Error("Error interno del servidor."));
            }
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se recibió un ID no válido ({PermissionId}) en la solicitud de búsqueda.", id);
                return BadRequest(ResponseApi<PermissionDTO>.Error("El ID debe ser mayor a 0."));
            }
            _logger.LogInformation("Buscando permisos con ID: {PermissionId}", id);
            try
            {
                var result = await _service.GetByIdAsync(id);
                if (result == null)
                {
                    _logger.LogWarning("No se encontró el permiso con ID {PermissionId}.", id);
                    return NotFound(ResponseApi<PermissionDTO>.Error("Permiso no encontrado."));
                }
                _logger.LogInformation("Permiso encontrada: ID={PermissionId}, Nombre={Name}", result.PermissionId, result.Name);
                return Ok(ResponseApi<PermissionDTO>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el permiso con ID {PermissionId}.", id);
                return StatusCode(500, ResponseApi<PermissionDTO>.Error("Error interno del servidor."));
            }
        }
        [HttpGet("filter")]
        public async Task<IActionResult> GetPermissions([FromQuery] string filterField, [FromQuery] string filterValue)
        {
            try
            {
                if (string.IsNullOrEmpty(filterField) || string.IsNullOrEmpty(filterValue))
                {
                    return BadRequest(ResponseApi<IEnumerable<PermissionDTO>>.Error("Debe proporcionar un campo y un valor para filtrar."));
                }
                var propertyInfo = typeof(PermissionDTO).GetProperty(filterField);
                if (propertyInfo == null)
                {
                    return BadRequest(ResponseApi<IEnumerable<PermissionDTO>>.Error($"El campo '{filterField}' no existe en PermissionDTO."));
                }
                object typedValue;
                try
                {
                    typedValue = Convert.ChangeType(filterValue, propertyInfo.PropertyType);
                }
                catch (Exception)
                {
                    return BadRequest(ResponseApi<IEnumerable<PermissionDTO>>.Error($"El valor '{filterValue}' no se puede convertir al tipo {propertyInfo.PropertyType.Name}."));
                }
                ParameterExpression param = Expression.Parameter(typeof(PermissionDTO), "dto");
                MemberExpression property = Expression.Property(param, filterField);
                ConstantExpression constant = Expression.Constant(typedValue, propertyInfo.PropertyType);
                BinaryExpression comparison = Expression.Equal(property, constant);
                Expression<Func<PermissionDTO, bool>> filter = Expression.Lambda<Func<PermissionDTO, bool>>(comparison, param);
                var result = await _service.GetAllAsync(new List<Expression<Func<PermissionDTO, bool>>> { filter });
                return Ok(ResponseApi<IEnumerable<PermissionDTO>>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener permisos con filtro.");
                return StatusCode(500, ResponseApi<IEnumerable<PermissionDTO>>.Error("Error interno del servidor."));
            }
        }
        [HttpGet("filters")]
        public async Task<IActionResult> GetPermissions([FromQuery] Dictionary<string, string> filters)
        {
            try
            {
                if (filters == null || filters.Count == 0)
                {
                    return BadRequest(ResponseApi<IEnumerable<PermissionDTO>>.Error("Debe proporcionar al menos un filtro."));
                }
                ParameterExpression param = Expression.Parameter(typeof(PermissionDTO), "dto");
                Expression finalExpression = null;
                foreach (var filter in filters)
                {
                    var propertyInfo = typeof(PermissionDTO).GetProperty(filter.Key);
                    if (propertyInfo == null)
                    {
                        return BadRequest(ResponseApi<IEnumerable<PermissionDTO>>.Error($"El campo '{filter.Key}' no existe en PermissionDTO."));
                    }
                    object typedValue;
                    try
                    {
                        typedValue = Convert.ChangeType(filter.Value, propertyInfo.PropertyType);
                    }
                    catch (Exception)
                    {
                        return BadRequest(ResponseApi<IEnumerable<PermissionDTO>>.Error($"El valor '{filter.Value}' no se puede convertir al tipo {propertyInfo.PropertyType.Name}."));
                    }
                    MemberExpression property = Expression.Property(param, propertyInfo);
                    ConstantExpression constant = Expression.Constant(typedValue, propertyInfo.PropertyType);
                    BinaryExpression comparison = Expression.Equal(property, constant);
                    finalExpression = finalExpression == null ? comparison : Expression.AndAlso(finalExpression, comparison);
                }
                var filterExpression = Expression.Lambda<Func<PermissionDTO, bool>>(finalExpression, param);
                var result = await _service.GetAllAsync(new List<Expression<Func<PermissionDTO, bool>>> { filterExpression });
                return Ok(ResponseApi<IEnumerable<PermissionDTO>>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener permisos con múltiples filtros.");
                return StatusCode(500, ResponseApi<IEnumerable<PermissionDTO>>.Error("Error interno del servidor."));
            }
        }
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create([FromBody] PermissionDTO permissionDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Se recibió una solicitud con datos inválidos para crear un permiso.");
                return BadRequest(ResponseApi<PermissionDTO>.Error("Datos de entrada inválidos."));
            }
            _logger.LogInformation("Creando nuevo permiso: {Name}", permissionDTO.Name);
            try
            {
                var result = await _service.CreateAsync(permissionDTO);
                if (result == null)
                {
                    _logger.LogWarning("No se pudo crear el permiso.");
                    return BadRequest(ResponseApi<PermissionDTO>.Error("No se pudo crear la permiso."));
                }
                _logger.LogInformation("Permiso creado con éxito: ID={PermissionId}, Nombre={Name}", result.PermissionId, result.Name);
                return CreatedAtAction(nameof(GetById), new { id = result.PermissionId },
                    ResponseApi<PermissionDTO>.Success(result, "Permiso creada con éxito."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el permiso.");
                return StatusCode(500, ResponseApi<PermissionDTO>.Error("Error interno del servidor."));
            }
        }

        [HttpPut]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Update([FromBody] PermissionDTO permissionDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Se recibió una solicitud con datos inválidos para actualizar un permiso.");
                return BadRequest(ResponseApi<PermissionDTO>.Error("Datos de entrada inválidos."));
            }
            _logger.LogInformation("Actualizando pemiso con ID: {PermissionId}, Nombre: {Name}", permissionDTO.PermissionId, permissionDTO.Name);
            try
            {
                var result = await _service.UpdateAsync(permissionDTO);
                if (result == null)
                {
                    _logger.LogWarning("No se pudo actualizar el permiso con ID {PermissionId}.", permissionDTO.PermissionId);
                    return NotFound(ResponseApi<PermissionDTO>.Error("Permiso no encontrada."));
                }
                _logger.LogInformation("Permiso actualizada con éxito: ID={PermissionId}, Nombre={Name}", result.PermissionId, result.Name);
                return Ok(ResponseApi<PermissionDTO>.Success(result, "Permiso actualizada correctamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el permiso con ID {PermissionId}.", permissionDTO.PermissionId);
                return StatusCode(500, ResponseApi<PermissionDTO>.Error("Error interno del servidor."));
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se recibió un ID no válido ({PermissionId}) en la solicitud de eliminación.", id);
                return BadRequest(ResponseApi<bool>.Error("El ID debe ser mayor a 0."));
            }
            _logger.LogInformation("Eliminando permiso con ID: {PermissionId}", id);
            try
            {
                var result = await _service.DeleteAsync(id);
                if (!result)
                {
                    _logger.LogWarning("No se encontró el permiso con ID {PermissionId} para eliminar.", id);
                    return NotFound(ResponseApi<bool>.Error("Permiso no encontrada."));
                }
                _logger.LogInformation("Pemiso eliminada con éxito: ID={PermissionId}", id);
                return Ok(ResponseApi<bool>.Success(result, "Permiso eliminada correctamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la permiso con ID {PermissionId}.", id);
                return StatusCode(500, ResponseApi<bool>.Error("Error interno del servidor."));
            }
        }
    }
}
