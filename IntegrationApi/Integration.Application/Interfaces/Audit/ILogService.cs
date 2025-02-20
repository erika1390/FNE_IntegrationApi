using Integration.Shared.DTO.Audit;

namespace Integration.Application.Interfaces.Audit
{
    public interface ILogService
    {
        Task<LogDTO> CreateAsync(LogDTO log);
        Task<IEnumerable<LogDTO>> SearchAsync(string? codeApplication, string? codeUser, DateTime? timestamp, string? level, string? source, string? method);
    }
}