using Integration.Core.Entities.Audit;
using Integration.Infrastructure.Data.Contexts;
using Integration.Infrastructure.Interfaces.Audit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
namespace Integration.Infrastructure.Repositories.Audit
{
    public class LogRepository : ILogRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LogRepository> _logger;
        public LogRepository(ApplicationDbContext context, ILogger<LogRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Log> CreateAsync(Log log)
        {
            if (log == null)
            {
                throw new ArgumentNullException(nameof(log), "El log no puede ser nulo.");
            }
            try
            {
                await _context.Logs.AddAsync(log);
                await _context.SaveChangesAsync();
                return log;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<IEnumerable<Log>> SearchAsync(string? codeApplication, string? codeUser, DateTime? timestamp, string? level, string? source, string? method)
        {
            var query = _context.Logs.AsQueryable();
            if (!string.IsNullOrEmpty(codeApplication))
                query = query.Where(log => log.CodeApplication == codeApplication);
            if (!string.IsNullOrEmpty(codeUser))
                query = query.Where(log => log.CodeUser == codeUser);
            if (timestamp.HasValue)
                query = query.Where(log => log.Timestamp.Date == timestamp.Value.Date);
            if (!string.IsNullOrEmpty(level))
                query = query.Where(log => log.Level == level);
            if (!string.IsNullOrEmpty(source))
                query = query.Where(log => log.Source == source);
            if (!string.IsNullOrEmpty(method))
                query = query.Where(log => log.Method == method);
            var logs = await query.ToListAsync();
            return logs;
        }
    }
}