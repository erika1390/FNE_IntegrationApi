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
    public class RoleMenuPermissionService : IRoleMenuPermissionService
    {
        private readonly IRoleMenuPermissionRepository _roleMenuPermissionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RoleMenuPermissionService> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMenuRepository _menuRepository;
        private readonly IPermissionRepository _permissionsRepository;
        public RoleMenuPermissionService(IRoleMenuPermissionRepository repository, IMapper mapper, ILogger<RoleMenuPermissionService> logger, IUserRepository userRepository, IRoleRepository roleRepository, IMenuRepository menuRepository, IPermissionRepository permissionsRepository)
        {
            _roleMenuPermissionRepository = repository;
            _mapper = mapper;
            _logger = logger;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _menuRepository = menuRepository;
            _permissionsRepository = permissionsRepository;
        }
        public async Task<RoleMenuPermissionDTO> CreateAsync(HeaderDTO header, RoleMenuPermissionDTO roleMenuPermissionDTO)
        {
            if (header == null || string.IsNullOrEmpty(header.UserCode))
            {
                throw new ArgumentNullException(nameof(header), "El encabezado o UserCode no pueden ser nulos.");
            }
            _logger.LogInformation("Creando RoleMenuPermissions: RoleCode : {RoleCode}, MenuCode : {MenuCode}, PermissionCode : {PermissionCode}", roleMenuPermissionDTO.RoleCode, roleMenuPermissionDTO.MenuCode, roleMenuPermissionDTO.PermissionCode);
            try
            {
                var user = await _userRepository.GetByCodeAsync(header.UserCode);
                if (user == null)
                {
                    throw new Exception($"No se encontró el usuario con código {header.UserCode}.");
                }
                var roleMenuPermission = _mapper.Map<Integration.Core.Entities.Security.RoleMenuPermission>(roleMenuPermissionDTO);
                roleMenuPermission.CreatedBy = user.UserName;
                roleMenuPermission.UpdatedBy = user.UserName;
                var role = await _roleRepository.GetByCodeAsync(roleMenuPermissionDTO.RoleCode);
                roleMenuPermission.RoleId = role.Id;
                var menu = await _menuRepository.GetByCodeAsync(roleMenuPermissionDTO.MenuCode);
                roleMenuPermission.MenuId = menu.Id;
                var permission = await _permissionsRepository.GetByCodeAsync(roleMenuPermissionDTO.PermissionCode);
                roleMenuPermission.PermissionId = permission.Id;
                var result = await _roleMenuPermissionRepository.CreateAsync(roleMenuPermission);
                _logger.LogInformation("RoleMenuPermissions creado con éxito: RoleCode : {RoleCode}, MenuCode : {MenuCode}, PermissionCode : {PermissionCode}", roleMenuPermissionDTO.RoleCode, roleMenuPermissionDTO.MenuCode, roleMenuPermissionDTO.PermissionCode);
                return _mapper.Map<RoleMenuPermissionDTO>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el RoleMenuPermissions: RoleCode : {RoleCode}, MenuCode : {MenuCode}, PermissionCode : {PermissionCode}", roleMenuPermissionDTO.RoleCode, roleMenuPermissionDTO.MenuCode, roleMenuPermissionDTO.PermissionCode);
                throw;
            }
        }

        public async Task<bool> DeactivateAsync(HeaderDTO header, RoleMenuPermissionDTO RoleMenuPermissionsDTO)
        {
            _logger.LogInformation("Desactivar RoleMenuPermissionsDTO con RoleCode : {RoleCode}, MenuCode : {MenuCode}, PermissionCode : {PermissionCode}", RoleMenuPermissionsDTO.RoleCode, RoleMenuPermissionsDTO.MenuCode, RoleMenuPermissionsDTO.PermissionCode);
            try
            {
                var user = await _userRepository.GetByCodeAsync(header.UserCode);
                if (user == null)
                {
                    throw new Exception($"No se encontró el usuario con código {header.UserCode}.");
                }
                var roleModulePermission = _mapper.Map<Integration.Core.Entities.Security.RoleMenuPermission>(RoleMenuPermissionsDTO);
                var role = await _roleRepository.GetByCodeAsync(RoleMenuPermissionsDTO.RoleCode);
                roleModulePermission.RoleId = role.Id;
                var menu = await _menuRepository.GetByCodeAsync(RoleMenuPermissionsDTO.MenuCode);
                roleModulePermission.MenuId = menu.Id;
                var permission = await _permissionsRepository.GetByCodeAsync(RoleMenuPermissionsDTO.PermissionCode);
                roleModulePermission.PermissionId = permission.Id;
                roleModulePermission.UpdatedBy = user.UserName;
                bool success = await _roleMenuPermissionRepository.DeactivateAsync(roleModulePermission);
                if (success)
                {
                    _logger.LogInformation("RoleMenuPermissionsDTO con RoleCode : {RoleCode}, MenuCode : {MenuCode}, PermissionCode : {PermissionCode} desactivada correctamente.", RoleMenuPermissionsDTO.RoleCode, RoleMenuPermissionsDTO.MenuCode, RoleMenuPermissionsDTO.PermissionCode);
                }
                else
                {
                    _logger.LogWarning("No se encontró el RoleMenuPermissionsDTO con RoleCode : {RoleCode}, MenuCode : {MenuCode}, PermissionCode : {PermissionCode} para desactivar.", RoleMenuPermissionsDTO.RoleCode, RoleMenuPermissionsDTO.MenuCode, RoleMenuPermissionsDTO.PermissionCode);
                }
                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al desactivar el RoleMenuPermissionsDTO con RoleCode : {RoleCode}, MenuCode : {MenuCode}, PermissionCode : {PermissionCode}.", RoleMenuPermissionsDTO.RoleCode, RoleMenuPermissionsDTO.MenuCode, RoleMenuPermissionsDTO.PermissionCode);
                throw;
            }
        }

        public async Task<IEnumerable<RoleMenuPermissionDTO>> GetAllActiveAsync()
        {
            _logger.LogInformation("Obteniendo todos los RoleMenuPermissions.");
            try
            {
                var RoleMenuPermissions = await _roleMenuPermissionRepository.GetAllActiveAsync();
                var RoleMenuPermissionsDTO = _mapper.Map<IEnumerable<RoleMenuPermissionDTO>>(RoleMenuPermissions);
                _logger.LogInformation("{Count} RoleMenuPermissions obtenidas con éxito.", RoleMenuPermissionsDTO.Count());
                return RoleMenuPermissionsDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los RoleMenuPermissions.");
                throw;
            }
        }

        public async Task<List<RoleMenuPermissionDTO>> GetByFilterAsync(Expression<Func<RoleMenuPermissionDTO, bool>> predicado)
        {
            try
            {
                _logger.LogInformation("Obteniendo permisos de roles y aplicando los filtros.");

                // Inicializar filtro base
                Expression<Func<RoleMenuPermission, bool>> roleMenuFilter = a => true;

                // Obtener filtros de RoleCode, Code y PermissionCode
                string roleCode = null, menuCode = null, permissionCode = null;

                if (predicado != null)
                {
                    roleCode = GetFilterValue(predicado, "RoleCode");
                    menuCode = GetFilterValue(predicado, "MenuCode");
                    permissionCode = GetFilterValue(predicado, "PermissionCode");
                }

                // Aplicar filtro de RoleCode
                if (!string.IsNullOrEmpty(roleCode))
                {
                    var role = await _roleRepository.GetByCodeAsync(roleCode);
                    if (role == null) return new List<RoleMenuPermissionDTO>();
                    roleMenuFilter = a => a.RoleId == role.Id;
                }

                // Aplicar filtro de Code
                if (!string.IsNullOrEmpty(menuCode))
                {
                    var manu = await _menuRepository.GetByCodeAsync(menuCode);
                    if (manu == null) return new List<RoleMenuPermissionDTO>();
                    roleMenuFilter = a => a.MenuId == manu.Id;
                }

                // Aplicar filtro de PermissionCode
                if (!string.IsNullOrEmpty(permissionCode))
                {
                    var permission = await _permissionsRepository.GetByCodeAsync(permissionCode);
                    if (permission == null) return new List<RoleMenuPermissionDTO>();
                    roleMenuFilter = a => a.PermissionId == permission.Id;
                }

                // Obtener los permisos filtrados desde la base de datos
                var RoleMenuPermissions = await _roleMenuPermissionRepository.GetByFilterAsync(roleMenuFilter);
                var RoleMenuPermissionsDTOs = _mapper.Map<List<RoleMenuPermissionDTO>>(RoleMenuPermissions);

                // Aplicar otros filtros en memoria si es necesario
                if (predicado != null && string.IsNullOrEmpty(roleCode) && string.IsNullOrEmpty(menuCode) && string.IsNullOrEmpty(permissionCode))
                {
                    RoleMenuPermissionsDTOs = RoleMenuPermissionsDTOs.AsQueryable().Where(predicado).ToList();
                }

                return RoleMenuPermissionsDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio al obtener los menus.");
                throw;
            }
        }

        private string GetFilterValue(Expression<Func<RoleMenuPermissionDTO, bool>> predicate, string propertyName)
        {
            if (predicate.Body is BinaryExpression binaryExp &&
                binaryExp.Left is MemberExpression member &&
                member.Member.Name == propertyName &&
                binaryExp.Right is ConstantExpression constant)
            {
                return constant.Value?.ToString();
            }

            return null;
        }

        public async Task<List<RoleMenuPermissionDTO>> GetByMultipleFiltersAsync(List<Expression<Func<RoleMenuPermissionDTO, bool>>> predicates)
        {
            try
            {
                _logger.LogInformation("Obteniendo permisos de roles con múltiples filtros.");

                // Filtro base para la base de datos
                Expression<Func<RoleMenuPermission, bool>> roleMenuFilter = a => true;

                // Aplicar filtros en la base de datos si existen
                foreach (var predicate in predicates ?? new List<Expression<Func<RoleMenuPermissionDTO, bool>>>())
                {
                    if (TryGetFilterValue(predicate, "RoleCode", out string roleCode))
                    {
                        var role = await _roleRepository.GetByCodeAsync(roleCode);
                        if (role == null) return new List<RoleMenuPermissionDTO>();
                        roleMenuFilter = a => a.RoleId == role.Id;
                    }
                    if (TryGetFilterValue(predicate, "MenuCode", out string menuCode))
                    {
                        var menu = await _menuRepository.GetByCodeAsync(menuCode);
                        if (menu == null) return new List<RoleMenuPermissionDTO>();
                        roleMenuFilter = a => a.MenuId == menu.Id;
                    }
                    if (TryGetFilterValue(predicate, "PermissionCode", out string permissionCode))
                    {
                        var permission = await _permissionsRepository.GetByCodeAsync(permissionCode);
                        if (permission == null) return new List<RoleMenuPermissionDTO>();
                        roleMenuFilter = a => a.PermissionId == permission.Id;
                    }
                }

                // Obtener datos de la base de datos con los filtros aplicados
                var RoleMenuPermissions = await _roleMenuPermissionRepository.GetByFilterAsync(roleMenuFilter);
                var RoleMenuPermissionsDTOs = _mapper.Map<List<RoleMenuPermissionDTO>>(RoleMenuPermissions);

                // Aplicar los filtros en memoria si aún quedan predicados
                if (predicates != null && predicates.Any(p => !IsFilteringByCode(p)))
                {
                    RoleMenuPermissionsDTOs = RoleMenuPermissionsDTOs.AsQueryable()
                        .Where(p => predicates.All(predicate => predicate.Compile()(p)))
                        .ToList();
                }

                return RoleMenuPermissionsDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio al obtener los menus.");
                throw;
            }
        }

        private bool IsFilteringByCode(Expression<Func<RoleMenuPermissionDTO, bool>> predicate)
        {
            return TryGetFilterValue(predicate, "RoleCode", out _) ||
                   TryGetFilterValue(predicate, "MenuCode", out _) ||
                   TryGetFilterValue(predicate, "PermissionCode", out _);
        }

        private bool TryGetFilterValue(Expression<Func<RoleMenuPermissionDTO, bool>> predicate, string propertyName, out string value)
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

        public async Task<RoleMenuPermissionDTO> GetByCodesAsync(RoleMenuPermissionDTO RoleMenuPermissionsDTO)
        {
            _logger.LogInformation("Buscando RoleMenuPermissions con RoleCode: {RoleCode}, MenuCode: {MenuCode}, PermissionCode: {PermissionCode}.",
                RoleMenuPermissionsDTO.RoleCode, RoleMenuPermissionsDTO.MenuCode, RoleMenuPermissionsDTO.PermissionCode);

            try
            {
                // Buscar Role, Menu y Permission por sus códigos
                var roleModulePermission = _mapper.Map<Integration.Core.Entities.Security.RoleMenuPermission>(RoleMenuPermissionsDTO);
                var role = await _roleRepository.GetByCodeAsync(RoleMenuPermissionsDTO.RoleCode);
                roleModulePermission.RoleId = role.Id;
                var menu = await _menuRepository.GetByCodeAsync(RoleMenuPermissionsDTO.MenuCode);
                roleModulePermission.MenuId = menu.Id;
                var permission = await _permissionsRepository.GetByCodeAsync(RoleMenuPermissionsDTO.PermissionCode);
                roleModulePermission.PermissionId = permission.Id;
                var result = await _roleMenuPermissionRepository.GetByRoleIdMenuIdPermissionsIdAsync(roleModulePermission);

                if (result == null)
                {
                    _logger.LogWarning("No se encontró RoleMenuPermissions con los códigos proporcionados.");
                    return null;
                }

                _logger.LogInformation("RoleMenuPermissions encontrada con RoleCode: {RoleCode}, MenuCode: {MenuCode}, PermissionCode: {PermissionCode}.",
                    RoleMenuPermissionsDTO.RoleCode, RoleMenuPermissionsDTO.MenuCode, RoleMenuPermissionsDTO.PermissionCode);

                // Mapear la entidad a DTO y retornar
                return _mapper.Map<RoleMenuPermissionDTO>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener RoleMenuPermissions con RoleCode: {RoleCode}, MenuCode: {MenuCode}, PermissionCode: {PermissionCode}.",
                    RoleMenuPermissionsDTO.RoleCode, RoleMenuPermissionsDTO.MenuCode, RoleMenuPermissionsDTO.PermissionCode);
                throw;
            }
        }

        public async Task<RoleMenuPermissionDTO> UpdateAsync(HeaderDTO header, RoleMenuPermissionDTO RoleMenuPermissionsDTO)
        {
            _logger.LogInformation("Actualizando RoleMenuPermissions con RoleCode : {RoleCode}, MenuCode : {MenuCode}, PermissionCode : {PermissionCode}.", RoleMenuPermissionsDTO.RoleCode, RoleMenuPermissionsDTO.MenuCode, RoleMenuPermissionsDTO.PermissionCode);
            try
            {
                var user = await _userRepository.GetByCodeAsync(header.UserCode);
                if (user == null)
                {
                    throw new Exception($"No se encontró el usuario con código {header.UserCode}.");
                }
                var roleModulePermission = _mapper.Map<Integration.Core.Entities.Security.RoleMenuPermission>(RoleMenuPermissionsDTO);
                roleModulePermission.UpdatedBy = user.UserName;
                var role = await _roleRepository.GetByCodeAsync(RoleMenuPermissionsDTO.RoleCode);
                roleModulePermission.RoleId = role.Id;
                var menu = await _menuRepository.GetByCodeAsync(RoleMenuPermissionsDTO.MenuCode);
                roleModulePermission.MenuId = menu.Id;
                var permission = await _permissionsRepository.GetByCodeAsync(RoleMenuPermissionsDTO.PermissionCode);
                roleModulePermission.PermissionId = permission.Id;
                var updatedRoleModulePermission = await _roleMenuPermissionRepository.UpdateAsync(roleModulePermission);
                if (updatedRoleModulePermission == null)
                {
                    _logger.LogWarning("No se pudo actualizar el RoleMenuPermissions con RoleCode : {RoleCode}, MenuCode : {MenuCode}, PermissionCode : {PermissionCode}.", RoleMenuPermissionsDTO.RoleCode, RoleMenuPermissionsDTO.MenuCode, RoleMenuPermissionsDTO.PermissionCode);
                    return null;
                }
                _logger.LogInformation("RoleMenuPermissions actualizado con éxito con RoleCode : {RoleCode}, MenuCode : {MenuCode}, PermissionCode : {PermissionCode}.", RoleMenuPermissionsDTO.RoleCode, RoleMenuPermissionsDTO.MenuCode, RoleMenuPermissionsDTO.PermissionCode);
                return _mapper.Map<RoleMenuPermissionDTO>(updatedRoleModulePermission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el RoleMenuPermissions con RoleCode : {RoleCode}, MenuCode : {MenuCode}, PermissionCode : {PermissionCode}.", RoleMenuPermissionsDTO.RoleCode, RoleMenuPermissionsDTO.MenuCode, RoleMenuPermissionsDTO.PermissionCode);
                throw new Exception("Error al actualizar el RoleMenuPermission", ex);
            }
        }
    }
}