using Integration.Application.Interfaces.Security;
using Integration.Shared.DTO.Security;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Integration.Api.Controllers.Security
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationService _service;

        public ApplicationController(IApplicationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IEnumerable<ApplicationDTO>> GetAll()
        {
            return await _service.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ApplicationDTO> GetById(int id)
        {
            return await _service.GetByIdAsync(id);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ApplicationDTO> Create(ApplicationDTO entity)
        {
            return await _service.CreateAsync(entity);
        }

        [HttpPut]
        public async Task<ApplicationDTO> Update(ApplicationDTO entity)
        {
            return await _service.UpdateAsync(entity);
        }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            return await _service.DeleteAsync(id);
        }
    }
}
