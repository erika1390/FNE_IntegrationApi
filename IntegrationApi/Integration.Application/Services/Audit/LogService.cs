using AutoMapper;
using Integration.Application.Interfaces.Audit;
using Integration.Infrastructure.Interfaces.Audit;
using Integration.Shared.DTO.Audit;
using Integration.Shared.DTO.Security;

using Microsoft.Extensions.Logging;
namespace Integration.Application.Services.Audit
{
    public class LogService : ILogService
    {
        private readonly ILogRepository _repository;
        private readonly IMapper _mapper;

        public LogService(ILogRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<LogDTO> CreateAsync(LogDTO logDTO)
        {
            try
            {
                var log = _mapper.Map<Integration.Core.Entities.Audit.Log>(logDTO);
                var result = await _repository.CreateAsync(log);
                return _mapper.Map<LogDTO>(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<IEnumerable<LogDTO>> SearchAsync(string? codeApplication, string? codeUser, DateTime? timestamp, string? level, string? source, string? method)
        {
            var logs = await _repository.SearchAsync(codeApplication, codeUser, timestamp, level, source, method);
            if (logs == null || !logs.Any())
            {
                return Enumerable.Empty<LogDTO>();
            }
            return _mapper.Map<IEnumerable<LogDTO>>(logs);
        }
    }
}