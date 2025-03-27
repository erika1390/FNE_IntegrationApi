using FluentValidation;

using Integration.Api.Filters;
using Integration.Application.Interfaces.Security;
using Integration.Shared.DTO.Header;
using Integration.Shared.DTO.Security;
using Integration.Shared.Response;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Integration.Api.Controllers.Security
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ServiceFilter(typeof(ValidateHeadersFilter))]
    public class UserPermissionController : ControllerBase
    {
        private readonly IUserPermissionService _service;
        private readonly ILogger<UserPermissionController> _logger;
        public UserPermissionController(IUserPermissionService service, ILogger<UserPermissionController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllActiveByUserCodeAsync([FromHeader] HeaderDTO header)
        {
            var result = await _service.GetAllActiveByUserCodeAsync(header.UserCode,header.ApplicationCode);
            if (!result.Any())
            {
                return NotFound(ResponseApi<IEnumerable<UserPermissionDTO>>.Error("No se encontraron usuarios."));
            }
            return Ok(ResponseApi<IEnumerable<UserPermissionDTO>>.Success(result));
        }
    }
}
