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
        public async Task<IActionResult> GetAllPermissionsByUserCodeAsync([FromHeader] HeaderDTO header)
        {
            var result = await _service.GetAllPermissionsByUserCodeAsync(header.UserCode,header.ApplicationCode);
            if (result == null || result.Roles == null || !result.Roles.Any())
            {
                return NotFound(ResponseApi<UserPermissionDTO>.Error("No se encontraron usuarios."));
            }
            return Ok(ResponseApi<UserPermissionDTO>.Success(result));
        }
    }
}
