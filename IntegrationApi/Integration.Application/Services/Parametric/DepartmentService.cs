using AutoMapper;

using Integration.Application.Interfaces.Parametric;
using Integration.Infrastructure.Interfaces.Parametric;
using Integration.Shared.DTO.Parametric;

using Microsoft.Extensions.Logging;

namespace Integration.Application.Services.Parametric
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<DepartmentService> _logger;

        public DepartmentService(IDepartmentRepository repository, IMapper mapper, ILogger<DepartmentService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<IEnumerable<DepartmentDTO>> GetAllActiveAsync()
        {
            _logger.LogInformation("Obteniendo todos los departamentos activos.");
            try
            {
                var departments = await _repository.GetAllActiveAsync();
                var departmentsDTO = _mapper.Map<IEnumerable<DepartmentDTO>>(departments);
                _logger.LogInformation("{Count} departamentos activos obtenidos con éxito.", departmentsDTO.Count());
                return departmentsDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los departamentos activos.");
                throw new Exception("Error al obtener todos los departamentos activos", ex);
            }
        }
    }
}