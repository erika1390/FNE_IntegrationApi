using AutoMapper;

using Integration.Application.Interfaces.Audit;
using Integration.Infrastructure.Interfaces.Audit;
using Integration.Shared.DTO.Audit;

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
        public async Task<bool> CreateAsync(LogDTO logDTO)
        {
            var log = _mapper.Map<Integration.Core.Entities.Audit.Log>(logDTO); 
            var success = await _repository.CreateAsync(log);
            return success;
        }
    }
}