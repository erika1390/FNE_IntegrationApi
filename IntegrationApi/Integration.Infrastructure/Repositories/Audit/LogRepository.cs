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
                _logger.LogWarning("Intento de registrar un log nulo.");
                throw new ArgumentNullException(nameof(log), "El log no puede ser nulo.");
            }

            try
            {
                await _context.Logs.AddAsync(log);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Log registrado correctamente en la base de datos.");
                return log;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar el log en la base de datos.");
                throw;
            }
        }

        public async Task<IEnumerable<Log>> SearchAsync(string? codeApplication, string? codeUser, DateTime? timestamp, string? level, string? source, string? method)
        {
            _logger.LogInformation("Ejecutando búsqueda de logs con filtros.");

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

            _logger.LogInformation("{Count} logs encontrados con los filtros aplicados.", logs.Count);
            return logs;
        }
    }
}