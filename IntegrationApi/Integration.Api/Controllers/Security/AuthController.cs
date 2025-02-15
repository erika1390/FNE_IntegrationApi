using Integration.Aplication.Services;
using Integration.Shared.DTO.Aut;

using Microsoft.AspNetCore.Mvc;

namespace Integration.Api.Controllers.Security
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDTO model)
        {
            var result = await _authService.RegisterUser(model);
            if (result)
                return Ok(new { message = "Usuario registrado con éxito" });

            return BadRequest(new { message = "Error al registrar usuario" });
        }
    }
}
