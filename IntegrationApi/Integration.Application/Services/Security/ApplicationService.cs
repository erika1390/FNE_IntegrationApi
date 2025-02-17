using AutoMapper;

using Integration.Application.Interfaces.Security;
using Integration.Infrastructure.Interfaces.Security;
using Integration.Shared.DTO.Security;
namespace Integration.Application.Services.Security
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _repository;
        private readonly IMapper _mapper;

        public ApplicationService(IApplicationRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApplicationDTO> CreateAsync(ApplicationDTO applicationDTO)
        {
            var entity = _mapper.Map<Integration.Core.Entities.Security.Application>(applicationDTO); // Mapea DTO a entidad
            var result = await _repository.CreateAsync(entity);
            return _mapper.Map<ApplicationDTO>(result);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id); throw new NotImplementedException();
        }

        public async Task<IEnumerable<ApplicationDTO>> GetAllAsync()
        {
            var applications = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<ApplicationDTO>>(applications);
        }

        public async Task<ApplicationDTO> GetByIdAsync(int id)
        {
            var application = await _repository.GetByIdAsync(id);
            return _mapper.Map<ApplicationDTO>(application);
        }

        public async Task<ApplicationDTO> UpdateAsync(ApplicationDTO applicationDTO)
        {
            var entity = _mapper.Map<Integration.Core.Entities.Security.Application>(applicationDTO);
            var updatedEntity = await _repository.UpdateAsync(entity);
            return _mapper.Map<ApplicationDTO>(updatedEntity);
        }
    }
}