using Integration.Core.Entities.Security;
using Integration.Infrastructure.Data.Contexts;
using Integration.Infrastructure.Interfaces.Security;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System.Linq.Expressions;

namespace Integration.Infrastructure.Repositories.Security
{
    public class MenuRepository : IMenuRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MenuRepository> _logger;
        public MenuRepository(ApplicationDbContext context, ILogger<MenuRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Menu> CreateAsync(Menu menu)
        {
            if (menu == null)
            {
                _logger.LogWarning("Intento de crear un menu con datos nulos.");
                throw new ArgumentNullException(nameof(menu), "El menu no puede ser nulo.");
            }
            try
            {
                await _context.Menus.AddAsync(menu);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Menu creado exitosamente: MenuCode: {MenuCode}, Nombre: {Name}", menu.Code, menu.Name);
                return menu;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error de base de datos al crear el menu.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al crear el menu.");
                throw;
            }
        }

        public async Task<bool> DeactivateAsync(string code, string userName)
        {
            try
            {
                var menu = await _context.Menus
                    .Where(a => a.Code == code)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
                if (menu == null)
                {
                    _logger.LogWarning("No se encontró el menu con MenuCode {MenuCode} para eliminar.", code);
                    return false;
                }
                menu.IsActive = false;
                menu.UpdatedAt = DateTime.UtcNow;
                menu.UpdatedBy = userName;
                _context.Menus.Update(menu);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Menu desactivado: {MenuCode}", code);
                return true;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error de base de datos al eliminar el menu con MenuCode {MenuCode}.", code);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al eliminar el menu con MenuCode {MenuCode}.", code);
                return false;
            }
        }

        public async Task<IEnumerable<Menu>> GetAllActiveAsync()
        {
            try
            {
                var menu = await _context.Menus.Where(m => m.IsActive == true).AsNoTracking().ToListAsync();

                _logger.LogInformation("Se obtuvieron {Count} menus de la base de datos.", menu.Count);
                return menu;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los menus.");
                return Enumerable.Empty<Menu>();
            }
        }

        public async Task<Menu> GetByCodeAsync(string code)
        {
            try
            {
                var menu = await _context.Menus
                    .Where(a => a.Code == code)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
                if (menu == null)
                {
                    _logger.LogWarning("No se encontró el menu con MenuCode {MenuCode}.", code);
                    return null;
                }

                _logger.LogInformation("Menu encontrado: MenuCode: {MenuCode}, Nombre: {Name}", menu.Code, menu.Name);

                return menu;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el menu con MenuCode {MenuCode}.", code);
                return null;
            }
        }

        public async Task<List<Menu>> GetByFilterAsync(Expression<Func<Menu, bool>> predicate)
        {
            try
            {
                _logger.LogInformation("Obteniendo menus con un predicado específico.");
                var menus = await _context.Menus.Where(predicate).ToListAsync();
                _logger.LogInformation("Se obtuvieron {Count} menus.", menus.Count);
                return menus;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los menus con el predicado especificado.");
                throw;
            }
        }

        public async Task<List<Menu>> GetByMultipleFiltersAsync(List<Expression<Func<Menu, bool>>> predicates)
        {
            try
            {
                _logger.LogInformation("Obteniendo menus con múltiples predicados.");
                var query = _context.Menus.AsQueryable();
                foreach (var predicado in predicates)
                {
                    query = query.Where(predicado);
                }
                var menus = await query.ToListAsync();
                _logger.LogInformation("Se obtuvieron {Count} menus tras aplicar múltiples predicados.", menus.Count);
                return menus;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los menus con múltiples predicados.");
                throw;
            }
        }

        public async Task<Menu> UpdateAsync(Menu menu)
        {
            if (menu == null)
            {
                _logger.LogWarning("Intento de actualizar un menu con datos nulos.");
                throw new ArgumentNullException(nameof(menu), "El menu no puede ser nulo.");
            }
            try
            {
                var menuEntity = await _context.Menus
                    .Where(a => a.Code == menu.Code)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
                if (menuEntity == null)
                {
                    _logger.LogWarning("No se encontró el menu con MenuCode {MenuCode} para actualizar.", menu.Code);
                    return null;
                }
                menuEntity.Name = menu.Name;
                menuEntity.UpdatedBy = menu.UpdatedBy;
                menuEntity.UpdatedAt = DateTime.UtcNow;
                menuEntity.IsActive = menu.IsActive;
                _context.Menus.Update(menuEntity);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Menu actualizado: MenuCode: {MenuCode}, Nombre: {Name}", menu.Id, menu.Name);
                return menuEntity;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error de base de datos al actualizar el menu con ID {MenuId}.", menu.Id);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error de base de datos al actualizar el menu con ID {MenuId}.", menu.Id);
                return null;
            }
        }
    }
}
