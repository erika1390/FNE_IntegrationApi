using Integration.Core.Entities.Audit;

namespace Integration.Infrastructure.Interfaces.Audit
{
    public interface ILogRepository
    {
        Task<bool> CreateAsync(Log application);
    }
}