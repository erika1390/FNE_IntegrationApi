using Integration.Core.Entities.Base;
using Integration.Infrastructure.Data.Contexts;
using Integration.Infrastructure.Interfaces.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
namespace Integration.Infrastructure.Repositories.Base
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _applicationDbContext;
        protected DbSet<T> _dbSet;

        public RepositoryBase(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            _dbSet = _applicationDbContext.Set<T>();
        }

        public async Task<T> CreateAsync(T entidad)
        {
            await _dbSet.AddAsync(entidad);
            await _applicationDbContext.SaveChangesAsync();
            return entidad;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
                return false;

            _dbSet.Remove(entity);
            await _applicationDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicado)
        {
            return await _dbSet.Where(predicado).ToListAsync();
        }

        public async Task<List<T>> GetAllAsync(List<Expression<Func<T, bool>>> predicados)
        {
            var query = _dbSet.AsQueryable();
            foreach (var item in predicados)
            {
                query.Where(item);
            }
            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FirstAsync(x => x.Id == id);
        }

        public async Task<T> UpdateAsync(T entidad)
        {
            _dbSet.Update(entidad);
            await _applicationDbContext.SaveChangesAsync();
            return entidad;
        }
    }
}