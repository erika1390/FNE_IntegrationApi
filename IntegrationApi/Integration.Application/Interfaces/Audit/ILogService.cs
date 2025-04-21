using Integration.Shared.DTO.Audit;
using Integration.Shared.DTO.Header;

namespace Integration.Application.Interfaces.Audit
{
    public interface ILogService
    {
        Task<LogDTO> CreateAsync(HeaderDTO header, LogDTO log);
        Task<IEnumerable<LogDTO>> SearchAsync(HeaderDTO header, DateTime? timestamp, string? level, string? source, string? method);
    }
}