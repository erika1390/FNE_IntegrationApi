using Integration.Application.Exceptions;
using Integration.Application.Interfaces.Security;
using Integration.Shared.DTO.Security;
using Integration.Shared.Response;

using Microsoft.AspNetCore.Identity;
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
        private readonly IUserService _userService;
        public AuthController(IJwtService jwtService, ILogger<AuthController> logger, IConfiguration configuration, IUserService userService)
        {
            _jwtService = jwtService;
            _logger = logger;
            _configuration = configuration;
            _userService = userService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {
            try
            {
                // Validación de entrada
                if (string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Password))
                {
                    throw new ValidationException("El usuario y la contraseña son obligatorios.");
                }
                _logger.LogInformation($"Intento de inicio de sesión para el usuario: {request.UserName}");

                var token = await _jwtService.GenerateTokenAsync(request); 
                return Ok(ResponseApi<string>.Success(token, "Autenticación exitosa."));
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