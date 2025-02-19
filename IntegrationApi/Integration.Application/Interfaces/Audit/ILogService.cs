using Integration.Shared.DTO.Audit;

namespace Integration.Application.Interfaces.Audit
{
    public interface ILogService
    {
        Task<bool> CreateAsync(LogDTO log);
    }
}