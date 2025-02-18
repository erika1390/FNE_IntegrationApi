using Integration.Application.Interfaces.Security;
using Integration.Shared.DTO.Security;

using Microsoft.AspNetCore.Mvc;

namespace Integration.Api.Controllers.Security
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IJwtService jwtService) : ControllerBase
    {
        private readonly IJwtService _jwtService = jwtService;

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDTO request)
        {
            // Aquí se validaría el usuario en la base de datos
            if (request.Username == "admin" && request.Password == "123456")
            {
                var token = _jwtService.GenerateToken(request.Username, "Admin");
                return Ok(new { token });
            }
            return Unauthorized("Usuario o contraseña incorrectos.");
        }
    }
}
