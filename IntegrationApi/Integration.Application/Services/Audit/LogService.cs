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
        private readonly ILogger<LogService> _logger;

        public LogService(ILogRepository repository, IMapper mapper, ILogger<LogService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<LogDTO> CreateAsync(LogDTO logDTO)
        {
            _logger.LogInformation("Creando log: {Level}", logDTO.Level);

            try
            {
                var log = _mapper.Map<Integration.Core.Entities.Audit.Log>(logDTO);
                var result = await _repository.CreateAsync(log);

                _logger.LogInformation("Log creado con éxito: {LogId}, CodeApplication: {CodeApplication}, CodeUser: {CodeUser}", result.LogId, result.CodeApplication, result.CodeUser);
                return _mapper.Map<LogDTO>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la log: CodeApplication: {CodeApplication}, CodeUser: {CodeUser}", logDTO.CodeApplication, logDTO.CodeUser);
                throw;
            }
        }
        public async Task<IEnumerable<LogDTO>> SearchAsync(string? codeApplication, string? codeUser, DateTime? timestamp, string? level, string? source, string? method)
        {
            _logger.LogInformation("Buscando logs con filtros en el servicio.");

            var logs = await _repository.SearchAsync(codeApplication, codeUser, timestamp, level, source, method);

            if (logs == null || !logs.Any())
            {
                _logger.LogWarning("No se encontraron logs con los filtros aplicados.");
                return Enumerable.Empty<LogDTO>();
            }

            _logger.LogInformation("{Count} logs encontrados con los filtros aplicados.", logs.Count());
            return _mapper.Map<IEnumerable<LogDTO>>(logs);
        }
    }
}