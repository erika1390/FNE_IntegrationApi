using Integration.Application.Exceptions;
using Integration.Application.Interfaces.Security;
using Integration.Shared.DTO.Security;
using Integration.Shared.Response;
using Microsoft.AspNetCore.Mvc;
namespace Integration.Api.Controllers.Security
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IJwtService jwtService, ILogger<AuthController> logger)
        {
            _jwtService = jwtService;
            _logger = logger;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDTO request)
        {
            try
            {
                // Validación de entrada
                if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
                {
                    throw new ValidationException("El usuario y la contraseña son obligatorios.");
                }

                // Simulación de autenticación (debería validarse contra la base de datos)
                if (request.Username == "Administrador" && request.Password == "123456")
                {
                    var token = _jwtService.GenerateToken(request.Username, "Administrador");
                    return Ok(ResponseApi<string>.Success(token, "Autenticación exitosa."));
                }

                throw new UnauthorizedException("Usuario o contraseña incorrectos.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el proceso de login");
                return Unauthorized(ResponseApi<string>.Error(ex.Message));
            }
        }
    }
}