using Integration.Core.Entities.Audit;
using Integration.Infrastructure.Data.Contexts;
using Integration.Infrastructure.Interfaces.Audit;

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

        public async Task<bool> CreateAsync(Log log)
        {
            if (log == null)
            {
                _logger.LogWarning("Intento de registrar un log nulo.");
                throw new ArgumentNullException(nameof(log), "El log no puede ser nulo.");
            }

            try
            {
                await _context.Logs.AddAsync(log);
                int count = await _context.SaveChangesAsync();

                bool success = count > 0;
                if (success)
                {
                    _logger.LogInformation("Log registrado correctamente en la base de datos.");
                }
                else
                {
                    _logger.LogWarning("No se insertó ningún registro en la base de datos.");
                }

                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar el log en la base de datos.");
                return false;
            }
        }
    }
}