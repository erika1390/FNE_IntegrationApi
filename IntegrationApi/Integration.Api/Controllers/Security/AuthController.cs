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
        private readonly IConfiguration _configuration;

        public AuthController(IJwtService jwtService, ILogger<AuthController> logger, IConfiguration configuration)
        {
            _jwtService = jwtService;
            _logger = logger;
            _configuration = configuration;
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

                // Obtener las credenciales seguras de la configuración
                var adminUsername = _configuration["AdminUsername"];
                var adminPassword = _configuration["AdminPassword"];

                // Simulación de autenticación (debería validarse contra la base de datos)
                if (request.Username == adminUsername && request.Password == adminPassword)
                {
                    var token = _jwtService.GenerateToken(request.Username, "Administrador");
                    return Ok(ResponseApi<string>.Success(token, "Autenticación exitosa."));
                }
                throw new UnauthorizedException("Usuario o contraseña incorrectos.");
            }
            catch (ValidationException ve)
            {
                _logger.LogWarning(ve, "Error de validación en el proceso de login");
                return BadRequest(ResponseApi<string>.Error(ve.Message));
            }
            catch (UnauthorizedException ue)
            {
                _logger.LogWarning(ue, "Error de autenticación en el proceso de login");
                return Unauthorized(ResponseApi<string>.Error(ue.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el proceso de login");
                return StatusCode(500, ResponseApi<string>.Error("Error interno del servidor."));
            }
        }
    }
}