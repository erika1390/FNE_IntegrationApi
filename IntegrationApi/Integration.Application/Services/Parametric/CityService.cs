using AutoMapper;

using Integration.Application.Interfaces.Parametric;
using Integration.Infrastructure.Interfaces.Parametric;
using Integration.Shared.DTO.Parametric;

using Microsoft.Extensions.Logging;

namespace Integration.Application.Services.Parametric
{
    public class CityService : ICityService
    {
        private readonly ICityRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CityService> _logger;

        public CityService(ICityRepository repository, IMapper mapper, ILogger<CityService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<List<CityDTO>> GetByDepartmentIdAsync(int departmentId)
        {
            _logger.LogInformation("Obteniendo todos las ciudades por departamento.");
            try
            {
                var cities = await _repository.GetByDepartmentIdAsync(departmentId);
                var citiesDTO = _mapper.Map<List<CityDTO>>(cities);
                _logger.LogInformation("{Count} ciudades obtenidas con éxito.", citiesDTO.Count());
                return citiesDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener ciudades.");
                throw new Exception("Error al obtener ciudades", ex);
            }
        }
    }
}