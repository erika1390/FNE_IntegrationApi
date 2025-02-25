using Integration.Core.Entities.Security;
using Integration.Infrastructure.Data.Contexts;
using Integration.Infrastructure.Interfaces.Security;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System.Linq;
using System.Linq.Expressions;
namespace Integration.Infrastructure.Repositories.Security
{
    public class ModuleRepository : IModuleRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        public ModuleRepository(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Module> CreateAsync(Module module)
        {
            if (module == null)
            {
                _logger.LogWarning("Intento de crear un módulo con datos nulos.");
                throw new ArgumentNullException(nameof(module), "El módulo no puede ser nulo.");
            }
            try
            {
                await _context.Modules.AddAsync(module);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Módulo creado exitosamente: Módulo: {ModuleId}, Nombre: {Name}", module.ModuleId, module.Name);
                return module;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error de base de datos al crear el módulo.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al crear el módulo.");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var module = await _context.Modules.FindAsync(id);
                if (module == null)
                {
                    _logger.LogWarning("No se encontró el módulo con ID {ModuleId} para eliminar.", id);
                    return false;
                }

                module.IsActive = false;
                module.UpdatedAt = DateTime.UtcNow;

                _context.Modules.Update(module);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Módulo desactivado: {ModuleId}", id);
                return true;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error de base de datos al eliminar el módulo con ID {ModuleId}.", id);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al eliminar el módulo con ID {ModuleId}.", id);
                return false;
            }
        }

        public async Task<IEnumerable<Module>> GetAllActiveAsync()
        {
            try
            {
                var modules = await _context.Modules.Where(m => m.IsActive == true).AsNoTracking().ToListAsync();

                _logger.LogInformation("Se obtuvieron {Count} módulos de la base de datos.", modules.Count);
                return modules;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los módulos.");
                return Enumerable.Empty<Module>();
            }
        }

        public async Task<List<Module>> GetAllAsync(Expression<Func<Module, bool>> predicado)
        {
            return await _context.Modules.Where(predicado).ToListAsync();
        }

        public async Task<List<Module>> GetAllAsync(List<Expression<Func<Module, bool>>> predicados)
        {
            var query = _context.Modules.AsQueryable();
            foreach (var predicado in predicados)
            {
                query = query.Where(predicado);
            }
            return await query.ToListAsync();
        }

        public async Task<Module> GetByIdAsync(int id)
        {
            try
            {
                var module = await _context.Modules.AsNoTracking().FirstOrDefaultAsync(m => m.ModuleId == id);
                if (module == null)
                {
                    _logger.LogWarning("No se encontró el módulo con ID {ModuleId}.", id);
                    return null;
                }

                _logger.LogInformation("Módulo encontrado: Módulo: {ModuleId}, Nombre: {Name}", module.ModuleId, module.Name);

                return module;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el módulo con ID {ModuleId}.", id);
                return null;
            }
        }

        public async Task<Module> UpdateAsync(Module module)
        {
            if (module == null)
            {
                _logger.LogWarning("Intento de actualizar un módulo con datos nulos.");
                throw new ArgumentNullException(nameof(module), "El módulo no puede ser nulo.");
            }
            try
            {
                var moduleEntity = await _context.Modules.FindAsync(module.ModuleId);
                if (moduleEntity == null)
                {
                    _logger.LogWarning("No se encontró el módulo con ID {ModuleId} para actualizar.", module.ModuleId);
                    return null;
                }
                moduleEntity.Name = module.Name;
                moduleEntity.UpdatedBy = module.UpdatedBy;
                moduleEntity.UpdatedAt = DateTime.UtcNow;
                moduleEntity.IsActive = module.IsActive;
                _context.Modules.Update(moduleEntity);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Módulo actualizado: {ModuleId}, Nombre: {Name}", module.ModuleId, module.Name);
                return moduleEntity;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error de base de datos al actualizar el módulo con ID {ModuleId}.", module.ModuleId);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al actualizar el módulo con ID {ModuleId}.", module.ModuleId);
                return null;
            }
        }
    }
}