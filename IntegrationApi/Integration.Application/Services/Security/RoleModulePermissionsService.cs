using AutoMapper;

using Integration.Application.Interfaces.Security;
using Integration.Core.Entities.Security;
using Integration.Infrastructure.Interfaces.Security;
using Integration.Shared.DTO.Header;
using Integration.Shared.DTO.Security;

using Microsoft.Extensions.Logging;

using System.Linq.Expressions;

namespace Integration.Application.Services.Security
{
    public class RoleModulePermissionsService : IRoleModulePermissionsService
    {
        private readonly IRoleModulePermissionsRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<RoleModulePermissionsService> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IModuleRepository _moduleRepository;
        private readonly IPermissionRepository _permissionsRepository;
        public RoleModulePermissionsService(IRoleModulePermissionsRepository repository, IMapper mapper, ILogger<RoleModulePermissionsService> logger, IUserRepository userRepository, IRoleRepository roleRepository, IModuleRepository moduleRepository, IPermissionRepository permissionsRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _moduleRepository = moduleRepository;
            _permissionsRepository = permissionsRepository;
        }
        public async Task<RoleModulePermissionsDTO> CreateAsync(HeaderDTO header, RoleModulePermissionsDTO roleModulePermissionsDTO)
        {
            if (header == null || string.IsNullOrEmpty(header.UserCode))
            {
                throw new ArgumentNullException(nameof(header), "El encabezado o UserCode no pueden ser nulos.");
            }
            _logger.LogInformation("Creando RoleModulePermissions: RoleCode : {RoleCode}, ModuleCode : {ModuleCode}, PermissionCode : {PermissionCode}", roleModulePermissionsDTO.RoleCode, roleModulePermissionsDTO.ModuleCode, roleModulePermissionsDTO.PermissionCode);
            try
            {
                var user = await _userRepository.GetByCodeAsync(header.UserCode);
                if (user == null)
                {
                    throw new Exception($"No se encontró el usuario con código {header.UserCode}.");
                }
                var roleModulePermission = _mapper.Map<Integration.Core.Entities.Security.RoleModulePermissions>(roleModulePermissionsDTO);
                roleModulePermission.CreatedBy = user.UserName;
                roleModulePermission.UpdatedBy = user.UserName;
                var role = await _roleRepository.GetByCodeAsync(roleModulePermissionsDTO.RoleCode);
                roleModulePermission.RoleId = role.Id;
                var module = await _moduleRepository.GetByCodeAsync(roleModulePermissionsDTO.ModuleCode);
                roleModulePermission.ModuleId = module.Id;
                var permission = await _permissionsRepository.GetByCodeAsync(roleModulePermissionsDTO.PermissionCode);
                roleModulePermission.PermissionId = permission.Id;
                var result = await _repository.CreateAsync(roleModulePermission);
                _logger.LogInformation("RoleModulePermissions creado con éxito: RoleCode : {RoleCode}, ModuleCode : {ModuleCode}, PermissionCode : {PermissionCode}", roleModulePermissionsDTO.RoleCode, roleModulePermissionsDTO.ModuleCode, roleModulePermissionsDTO.PermissionCode);
                return _mapper.Map<RoleModulePermissionsDTO>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el RoleModulePermissions: RoleCode : {RoleCode}, ModuleCode : {ModuleCode}, PermissionCode : {PermissionCode}", roleModulePermissionsDTO.RoleCode, roleModulePermissionsDTO.ModuleCode, roleModulePermissionsDTO.PermissionCode);
                throw;
            }
        }

        public async Task<bool> DeactivateAsync(HeaderDTO header, RoleModulePermissionsDTO roleModulePermissionsDTO)
        {
            _logger.LogInformation("Desactivar RoleModulePermissionsDTO con RoleCode : {RoleCode}, ModuleCode : {ModuleCode}, PermissionCode : {PermissionCode}", roleModulePermissionsDTO.RoleCode, roleModulePermissionsDTO.ModuleCode, roleModulePermissionsDTO.PermissionCode);
            try
            {
                var user = await _userRepository.GetByCodeAsync(header.UserCode);
                if (user == null)
                {
                    throw new Exception($"No se encontró el usuario con código {header.UserCode}.");
                }
                var roleModulePermission = _mapper.Map<Integration.Core.Entities.Security.RoleModulePermissions>(roleModulePermissionsDTO);
                roleModulePermission.UpdatedBy = user.UserName;
                bool success = await _repository.DeactivateAsync(roleModulePermission);
                if (success)
                {
                    _logger.LogInformation("RoleModulePermissionsDTO con RoleCode : {RoleCode}, ModuleCode : {ModuleCode}, PermissionCode : {PermissionCode} desactivada correctamente.", roleModulePermissionsDTO.RoleCode, roleModulePermissionsDTO.ModuleCode, roleModulePermissionsDTO.PermissionCode);
                }
                else
                {
                    _logger.LogWarning("No se encontró el RoleModulePermissionsDTO con RoleCode : {RoleCode}, ModuleCode : {ModuleCode}, PermissionCode : {PermissionCode} para desactivar.", roleModulePermissionsDTO.RoleCode, roleModulePermissionsDTO.ModuleCode, roleModulePermissionsDTO.PermissionCode);
                }
                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al desactivar el RoleModulePermissionsDTO con RoleCode : {RoleCode}, ModuleCode : {ModuleCode}, PermissionCode : {PermissionCode}.", roleModulePermissionsDTO.RoleCode, roleModulePermissionsDTO.ModuleCode, roleModulePermissionsDTO.PermissionCode);
                throw;
            }
        }

        public async Task<IEnumerable<RoleModulePermissionsDTO>> GetAllActiveAsync()
        {
            _logger.LogInformation("Obteniendo todos los RoleModulePermissions.");
            try
            {
                var roleModulePermissions = await _repository.GetAllActiveAsync();
                var roleModulePermissionsDTO = _mapper.Map<IEnumerable<RoleModulePermissionsDTO>>(roleModulePermissions);
                _logger.LogInformation("{Count} RoleModulePermissions obtenidas con éxito.", roleModulePermissionsDTO.Count());
                return roleModulePermissionsDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los RoleModulePermissions.");
                throw;
            }
        }

        public async Task<List<RoleModulePermissionsDTO>> GetAllAsync(Expression<Func<RoleModulePermissionsDTO, bool>> predicado)
        {
            try
            {
                _logger.LogInformation("Obteniendo permisos de roles y aplicando los filtros.");

                // Inicializar filtro base
                Expression<Func<RoleModulePermissions, bool>> roleModuleFilter = a => true;

                // Obtener filtros de RoleCode, ModuleCode y PermissionCode
                string roleCode = null, moduleCode = null, permissionCode = null;

                if (predicado != null)
                {
                    roleCode = GetFilterValue(predicado, "RoleCode");
                    moduleCode = GetFilterValue(predicado, "ModuleCode");
                    permissionCode = GetFilterValue(predicado, "PermissionCode");
                }

                // Aplicar filtro de RoleCode
                if (!string.IsNullOrEmpty(roleCode))
                {
                    var role = await _roleRepository.GetByCodeAsync(roleCode);
                    if (role == null) return new List<RoleModulePermissionsDTO>();
                    roleModuleFilter = a => a.RoleId == role.Id;
                }

                // Aplicar filtro de ModuleCode
                if (!string.IsNullOrEmpty(moduleCode))
                {
                    var module = await _moduleRepository.GetByCodeAsync(moduleCode);
                    if (module == null) return new List<RoleModulePermissionsDTO>();
                    roleModuleFilter = a => a.ModuleId == module.Id;
                }

                // Aplicar filtro de PermissionCode
                if (!string.IsNullOrEmpty(permissionCode))
                {
                    var permission = await _permissionsRepository.GetByCodeAsync(permissionCode);
                    if (permission == null) return new List<RoleModulePermissionsDTO>();
                    roleModuleFilter = a => a.PermissionId == permission.Id;
                }

                // Obtener los permisos filtrados desde la base de datos
                var roleModulePermissions = await _repository.GetAllAsync(roleModuleFilter);
                var roleModulePermissionsDTOs = _mapper.Map<List<RoleModulePermissionsDTO>>(roleModulePermissions);

                // Aplicar otros filtros en memoria si es necesario
                if (predicado != null && string.IsNullOrEmpty(roleCode) && string.IsNullOrEmpty(moduleCode) && string.IsNullOrEmpty(permissionCode))
                {
                    roleModulePermissionsDTOs = roleModulePermissionsDTOs.AsQueryable().Where(predicado).ToList();
                }

                return roleModulePermissionsDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio al obtener los permisos.");
                throw;
            }
        }

        private string GetFilterValue(Expression<Func<RoleModulePermissionsDTO, bool>> predicado, string propertyName)
        {
            if (predicado.Body is BinaryExpression binaryExp &&
                binaryExp.Left is MemberExpression member &&
                member.Member.Name == propertyName &&
                binaryExp.Right is ConstantExpression constant)
            {
                return constant.Value?.ToString();
            }

            return null;
        }

        public async Task<List<RoleModulePermissionsDTO>> GetAllAsync(List<Expression<Func<RoleModulePermissionsDTO, bool>>> predicates)
        {
            try
            {
                _logger.LogInformation("Obteniendo permisos de roles con múltiples filtros.");

                // Filtro base para la base de datos
                Expression<Func<RoleModulePermissions, bool>> roleModuleFilter = a => true;

                // Aplicar filtros en la base de datos si existen
                foreach (var predicate in predicates ?? new List<Expression<Func<RoleModulePermissionsDTO, bool>>>())
                {
                    if (TryGetFilterValue(predicate, "RoleCode", out string roleCode))
                    {
                        var role = await _roleRepository.GetByCodeAsync(roleCode);
                        if (role == null) return new List<RoleModulePermissionsDTO>();
                        roleModuleFilter = a => a.RoleId == role.Id;
                    }
                    if (TryGetFilterValue(predicate, "ModuleCode", out string moduleCode))
                    {
                        var module = await _moduleRepository.GetByCodeAsync(moduleCode);
                        if (module == null) return new List<RoleModulePermissionsDTO>();
                        roleModuleFilter = a => a.ModuleId == module.Id;
                    }
                    if (TryGetFilterValue(predicate, "PermissionCode", out string permissionCode))
                    {
                        var permission = await _permissionsRepository.GetByCodeAsync(permissionCode);
                        if (permission == null) return new List<RoleModulePermissionsDTO>();
                        roleModuleFilter = a => a.PermissionId == permission.Id;
                    }
                }

                // Obtener datos de la base de datos con los filtros aplicados
                var roleModulePermissions = await _repository.GetAllAsync(roleModuleFilter);
                var roleModulePermissionsDTOs = _mapper.Map<List<RoleModulePermissionsDTO>>(roleModulePermissions);

                // Aplicar los filtros en memoria si aún quedan predicados
                if (predicates != null && predicates.Any(p => !IsFilteringByCode(p)))
                {
                    roleModulePermissionsDTOs = roleModulePermissionsDTOs.AsQueryable()
                        .Where(p => predicates.All(predicate => predicate.Compile()(p)))
                        .ToList();
                }

                return roleModulePermissionsDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio al obtener los permisos.");
                throw;
            }
        }

        private bool IsFilteringByCode(Expression<Func<RoleModulePermissionsDTO, bool>> predicate)
        {
            return TryGetFilterValue(predicate, "RoleCode", out _) ||
                   TryGetFilterValue(predicate, "ModuleCode", out _) ||
                   TryGetFilterValue(predicate, "PermissionCode", out _);
        }

        private bool TryGetFilterValue(Expression<Func<RoleModulePermissionsDTO, bool>> predicate, string propertyName, out string value)
        {
            value = null;

            if (predicate.Body is BinaryExpression binaryExp &&
                binaryExp.Left is MemberExpression member &&
                member.Member.Name == propertyName &&
                binaryExp.Right is ConstantExpression constant)
            {
                value = constant.Value?.ToString();
                return !string.IsNullOrEmpty(value);
            }

            return false;
        }

        public async Task<RoleModulePermissionsDTO> GetByRoleCodeModuleCodePermissionsCodeAsync(RoleModulePermissionsDTO roleModulePermissionsDTO)
        {
            _logger.LogInformation("Buscando RoleModulePermissions con RoleCode: {RoleCode}, ModuleCode: {ModuleCode}, PermissionCode: {PermissionCode}.",
                roleModulePermissionsDTO.RoleCode, roleModulePermissionsDTO.ModuleCode, roleModulePermissionsDTO.PermissionCode);

            try
            {
                // Buscar Role, Module y Permission por sus códigos
                var role = await _roleRepository.GetByCodeAsync(roleModulePermissionsDTO.RoleCode);
                if (role == null)
                {
                    _logger.LogWarning("No se encontró el RoleModulePermissions con RoleCode: {RoleCode}, ModuleCode: {ModuleCode}, PermissionCode: {PermissionCode}.",
                roleModulePermissionsDTO.RoleCode, roleModulePermissionsDTO.ModuleCode, roleModulePermissionsDTO.PermissionCode);
                    return null;
                }

                var module = await _moduleRepository.GetByCodeAsync(roleModulePermissionsDTO.ModuleCode);
                if (module == null)
                {
                    _logger.LogWarning("No se encontró el RoleModulePermissions con RoleCode: {RoleCode}, ModuleCode: {ModuleCode}, PermissionCode: {PermissionCode}.",
                roleModulePermissionsDTO.RoleCode, roleModulePermissionsDTO.ModuleCode, roleModulePermissionsDTO.PermissionCode);
                    return null;
                }

                var permission = await _permissionsRepository.GetByCodeAsync(roleModulePermissionsDTO.PermissionCode);
                if (permission == null)
                {
                    _logger.LogWarning("No se encontró el RoleModulePermissions con RoleCode: {RoleCode}, ModuleCode: {ModuleCode}, PermissionCode: {PermissionCode}.",
                roleModulePermissionsDTO.RoleCode, roleModulePermissionsDTO.ModuleCode, roleModulePermissionsDTO.PermissionCode);
                    return null;
                }

                // Crear la entidad con los IDs obtenidos
                var roleModulePermission = new RoleModulePermissions
                {
                    RoleId = role.Id,
                    ModuleId = module.Id,
                    PermissionId = permission.Id
                };

                var result = await _repository.GetByRoleIdModuleIdPermissionsIdAsync(roleModulePermission);

                if (result == null)
                {
                    _logger.LogWarning("No se encontró RoleModulePermissions con los códigos proporcionados.");
                    return null;
                }

                _logger.LogInformation("RoleModulePermissions encontrada con RoleCode: {RoleCode}, ModuleCode: {ModuleCode}, PermissionCode: {PermissionCode}.",
                    roleModulePermissionsDTO.RoleCode, roleModulePermissionsDTO.ModuleCode, roleModulePermissionsDTO.PermissionCode);

                // Mapear la entidad a DTO y retornar
                return _mapper.Map<RoleModulePermissionsDTO>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener RoleModulePermissions con RoleCode: {RoleCode}, ModuleCode: {ModuleCode}, PermissionCode: {PermissionCode}.",
                    roleModulePermissionsDTO.RoleCode, roleModulePermissionsDTO.ModuleCode, roleModulePermissionsDTO.PermissionCode);
                throw;
            }
        }

        public async Task<RoleModulePermissionsDTO> UpdateAsync(HeaderDTO header, RoleModulePermissionsDTO roleModulePermissionsDTO)
        {
            _logger.LogInformation("Actualizando RoleModulePermissions con RoleCode : {RoleCode}, ModuleCode : {ModuleCode}, PermissionCode : {PermissionCode}.", roleModulePermissionsDTO.RoleCode, roleModulePermissionsDTO.ModuleCode, roleModulePermissionsDTO.PermissionCode);
            try
            {
                var user = await _userRepository.GetByCodeAsync(header.UserCode);
                if (user == null)
                {
                    throw new Exception($"No se encontró el usuario con código {header.UserCode}.");
                }
                var roleModulePermission = _mapper.Map<Integration.Core.Entities.Security.RoleModulePermissions>(roleModulePermissionsDTO);
                roleModulePermission.UpdatedBy = user.UserName;
                var role = await _roleRepository.GetByCodeAsync(roleModulePermissionsDTO.RoleCode);
                roleModulePermission.RoleId = role.Id;
                var module = await _moduleRepository.GetByCodeAsync(roleModulePermissionsDTO.ModuleCode);
                roleModulePermission.ModuleId = module.Id;
                var permission = await _permissionsRepository.GetByCodeAsync(roleModulePermissionsDTO.PermissionCode);
                roleModulePermission.PermissionId = permission.Id;
                var updatedRoleModulePermission = await _repository.UpdateAsync(roleModulePermission);
                if (updatedRoleModulePermission == null)
                {
                    _logger.LogWarning("No se pudo actualizar el RoleModulePermissions con RoleCode : {RoleCode}, ModuleCode : {ModuleCode}, PermissionCode : {PermissionCode}.", roleModulePermissionsDTO.RoleCode, roleModulePermissionsDTO.ModuleCode, roleModulePermissionsDTO.PermissionCode);
                    return null;
                }
                _logger.LogInformation("RoleModulePermissions actualizado con éxito con RoleCode : {RoleCode}, ModuleCode : {ModuleCode}, PermissionCode : {PermissionCode}.", roleModulePermissionsDTO.RoleCode, roleModulePermissionsDTO.ModuleCode, roleModulePermissionsDTO.PermissionCode);
                return _mapper.Map<RoleModulePermissionsDTO>(updatedRoleModulePermission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el RoleModulePermissions con RoleCode : {RoleCode}, ModuleCode : {ModuleCode}, PermissionCode : {PermissionCode}.", roleModulePermissionsDTO.RoleCode, roleModulePermissionsDTO.ModuleCode, roleModulePermissionsDTO.PermissionCode);
                throw new Exception("Error al actualizar el permiso", ex);
            }
        }
    }
}