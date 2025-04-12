using AutoMapper;

using Integration.Application.Interfaces.Security;
using Integration.Infrastructure.Interfaces.Security;
using Integration.Shared.DTO.Header;
using Integration.Shared.DTO.Security;

using Microsoft.Extensions.Logging;

using System.Linq.Expressions;

namespace Integration.Application.Services.Security
{
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IModuleRepository _moduleRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<MenuService> _logger;
        private readonly IUserRepository _userRepository;

        public MenuService(IMenuRepository menuRepository, IModuleRepository moduleRepository, IMapper mapper, ILogger<MenuService> logger, IUserRepository userRepository)
        {
            _menuRepository = menuRepository;
            _moduleRepository = moduleRepository;
            _mapper = mapper;
            _logger = logger;
            _userRepository = userRepository;
        }

        public async Task<MenuDTO> CreateAsync(HeaderDTO header, MenuDTO menuDTO)
        {
            if (header == null || string.IsNullOrEmpty(header.UserCode))
            {
                throw new ArgumentNullException(nameof(header), "El encabezado o UserCode no pueden ser nulos.");
            }
            _logger.LogInformation("Creando menu: {Name}", menuDTO.Name);
            try
            {
                var user = await _userRepository.GetByCodeAsync(header.UserCode);
                if (user == null)
                {
                    throw new Exception($"No se encontró el usuario con código {header.UserCode}.");
                }
                var parentModule = await _menuRepository.GetByCodeAsync(menuDTO.ParentMenuCode);
                var module = await _moduleRepository.GetByCodeAsync(menuDTO.ModuleCode);
                var menu = _mapper.Map<Integration.Core.Entities.Security.Menu>(menuDTO);
                menu.ModuleId = module.Id;
                menu.ParentMenuId = parentModule?.Id;
                menu.CreatedBy = user.UserName;
                menu.UpdatedBy = user.UserName;
                var result = await _menuRepository.CreateAsync(menu);
                _logger.LogInformation("Menu creado con éxito: MenuId {MenuId}, Nombre: {Name}", result.Id, result.Name);
                return _mapper.Map<MenuDTO>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el menu: {Name}", menuDTO.Name);
                throw;
            }
        }

        public async Task<bool> DeactivateAsync(HeaderDTO header, string code)
        {
            _logger.LogInformation("Desactivando aplicación con MenuCode: {MenuCode}", code);
            try
            {
                var user = await _userRepository.GetByCodeAsync(header.UserCode);
                if (user == null)
                {
                    throw new Exception($"No se encontró el usuario con código {header.UserCode}.");
                }
                bool success = await _menuRepository.DeactivateAsync(code, user.UserName);
                if (success)
                {
                    _logger.LogInformation("Menu con MenuCode {MenuCode} desactivada correctamente.", code);
                }
                else
                {
                    _logger.LogWarning("No se encontró el menu con MenuCode {MenuCode} para desactivar.", code);
                }
                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el menu con MenuCode {MenuCode}.", code);
                throw;
            }
        }

        public async Task<IEnumerable<MenuDTO>> GetAllActiveAsync()
        {
            _logger.LogInformation("Obteniendo todos los menus.");
            try
            {
                var menus = await _menuRepository.GetAllActiveAsync();
                var menusDTO = _mapper.Map<IEnumerable<MenuDTO>>(menus);
                _logger.LogInformation("{Count} menus obtenidas con éxito.", menusDTO.Count());
                return menusDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los menus.");
                throw;
            }
        }

        public async Task<MenuDTO> GetByCodeAsync(string code)
        {
            _logger.LogInformation("Buscando menu con MenuCode: {MenuCode}", code);
            try
            {
                var menu = await _menuRepository.GetByCodeAsync(code);
                if (menu == null)
                {
                    _logger.LogWarning("No se encontró la menu con MenuCode {MenuCode}.", code);
                    return null;
                }
                _logger.LogInformation("Menu encontrada: MenuCode {MenuCode}, Nombre: {Name}", menu.Code, menu.Name);
                return _mapper.Map<MenuDTO>(menu);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el menu con MenuCode {MenuCode}.", code);
                throw;
            }
        }

        public async Task<List<MenuDTO>> GetByFilterAsync(Expression<Func<MenuDTO, bool>> predicate)
        {
            try
            {
                _logger.LogInformation("Obteniendo todos los menus y aplicando el filtro.");
                int? moduleId = null;
                Expression<Func<Integration.Core.Entities.Security.Menu, bool>> menuFilter = a => true;
                if (predicate != null && IsFilteringByModuleCode(predicate, out string moduleCode))
                {
                    _logger.LogInformation("Buscando menu del modulo con código: {ModuleCode}", moduleCode);
                    var module = await _moduleRepository.GetByCodeAsync(moduleCode);

                    if (module == null)
                    {
                        _logger.LogWarning("No se encontró el menu del modulo con código: {ModuleCode}", moduleCode);
                        return new List<MenuDTO>();
                    }
                    moduleId = module.Id;
                    menuFilter = a => a.ModuleId == moduleId.Value;
                }
                var menus = await _menuRepository.GetByFilterAsync(menuFilter);
                var menusDTOs = _mapper.Map<List<MenuDTO>>(menus);
                if (predicate != null && !IsFilteringByModuleCode(predicate, out _))
                {
                    menusDTOs = menusDTOs.AsQueryable().Where(predicate).ToList();
                }
                return menusDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio al obtener menus.");
                throw;
            }
        }

        private bool IsFilteringByModuleCode(Expression<Func<MenuDTO, bool>> predicate, out string moduleCode)
        {
            moduleCode = null;

            if (predicate.Body is BinaryExpression binaryExp)
            {
                if (binaryExp.Left is MemberExpression member && member.Member.Name == "ModuleCode" &&
                    binaryExp.Right is ConstantExpression constant)
                {
                    moduleCode = constant.Value?.ToString();
                    return !string.IsNullOrEmpty(moduleCode);
                }
            }
            return false;
        }

        public async Task<List<MenuDTO>> GetByMultipleFiltersAsync(List<Expression<Func<MenuDTO, bool>>> predicates)
        {
            try
            {
                _logger.LogInformation("Obteniendo todos los menus y aplicando múltiples filtros.");

                int? moduleId = null;
                string moduleCode = null;
                List<Expression<Func<MenuDTO, bool>>> otherFilters = new List<Expression<Func<MenuDTO, bool>>>();
                foreach (var predicado in predicates)
                {
                    if (IsFilteringByModuleCode(predicado, out string extractedCode))
                    {
                        if (string.IsNullOrEmpty(moduleCode))
                        {
                            moduleCode = extractedCode;
                        }
                    }
                    else
                    {
                        otherFilters.Add(predicado);
                    }
                }
                if (!string.IsNullOrEmpty(moduleCode))
                {
                    _logger.LogInformation("Buscando menu para el modulo con código: {ModuleCode}", moduleCode);
                    var module = await _moduleRepository.GetByCodeAsync(moduleCode);
                    if (module == null)
                    {
                        _logger.LogWarning("No se encontró menu para el modulo con código: {ModuleCode}", moduleCode);
                        return new List<MenuDTO>();
                    }
                    moduleId = module.Id;
                }
                Expression<Func<Integration.Core.Entities.Security.Menu, bool>> menuFilter = a => true;
                if (moduleId.HasValue)
                {
                    menuFilter = a => a.ModuleId == moduleId.Value;
                }
                var menus = await _menuRepository.GetByFilterAsync(menuFilter);
                var menusDTOs = _mapper.Map<List<MenuDTO>>(menus);
                foreach (var filter in otherFilters)
                {
                    menusDTOs = menusDTOs.Where(filter.Compile()).ToList();
                }
                return menusDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio al obtener los menus con múltiples filtros.");
                throw;
            }
        }

        public async Task<MenuDTO> UpdateAsync(HeaderDTO header, MenuDTO menuDTO)
        {
            _logger.LogInformation("Menu creado exitosamente: MenuCode={MenuCode}, Name={Name}", menuDTO.Code, menuDTO.Name);
            try
            {
                var user = await _userRepository.GetByCodeAsync(header.UserCode);
                if (user == null)
                {
                    throw new Exception($"No se encontró el usuario con código {header.UserCode}.");
                }
                var module = await _moduleRepository.GetByCodeAsync(menuDTO.ModuleCode);
                var menuExist = await _menuRepository.GetByCodeAsync(menuDTO.Code);
                var menu = _mapper.Map<Integration.Core.Entities.Security.Menu>(menuDTO);
                menu.ModuleId = module.Id;
                menu.Id = menuExist.Id;
                menu.UpdatedBy = user.UserName;
                var updatedMenu = await _menuRepository.UpdateAsync(menu);
                if (updatedMenu == null)
                {
                    _logger.LogWarning("Menu con MenuCode {MenuCode} no encontrado.", menuDTO.Code);
                    return null;
                }
                _logger.LogInformation("Menu actualizado con éxito: MenuCode: {MenuCode}, Nombre: {Name}", updatedMenu.Code, updatedMenu.Name);
                return _mapper.Map<MenuDTO>(updatedMenu);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar menu con MenuCode {MenuCode}.", menuDTO.Code);
                throw;
            }
        }
    }
}
