using Integration.Core.Entities.Security;
using Integration.Infrastructure.Data.Contexts;
using Integration.Infrastructure.Interfaces.Security;

using Microsoft.EntityFrameworkCore;

namespace Integration.Infrastructure.Repositories.Security
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly ApplicationDbContext _context;
        public ApplicationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Application> CreateAsync(Application entity)
        {
            await _context.Applications.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Applications.FindAsync(id);
            if (entity == null) return false;

            _context.Applications.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Application>> GetAllAsync()
        {
            return await _context.Applications.ToListAsync();
        }

        public async Task<Application> GetByIdAsync(int id)
        {
            return await _context.Applications.FindAsync(id);
        }

        public async Task<Application> UpdateAsync(Application entity)
        {
            _context.Applications.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}