using Integration.Core.Entities.Audit;

namespace Integration.Infrastructure.Interfaces.Audit
{
    public interface ILogRepository
    {
        Task<Log> CreateAsync(Log log);
        Task<IEnumerable<Log>> SearchAsync(string? codeApplication, string? codeUser, DateTime? timestamp, string? level, string? source, string? method);
    }
}