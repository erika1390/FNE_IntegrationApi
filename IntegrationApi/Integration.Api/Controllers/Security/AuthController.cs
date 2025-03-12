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
                // Verificar si el request es nulo
                if (request == null)
                {
                    _logger.LogWarning("Se recibió una solicitud de login con datos nulos.");
                    return BadRequest(ResponseApi<string>.Error("Los datos de login no pueden ser nulos."));
                }

                // Validación del modelo de entrada
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Se recibió una solicitud de login con datos inválidos.");
                    return BadRequest(ResponseApi<string>.Error("Datos de entrada inválidos."));
                }

                // Validar que usuario y contraseña no estén vacíos
                if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
                {
                    _logger.LogWarning("Intento de inicio de sesión con datos vacíos.");
                    return BadRequest(ResponseApi<string>.Error("El usuario y la contraseña son obligatorios."));
                }

                _logger.LogInformation("Intento de inicio de sesión para el usuario: {UserName}", request.UserName);

                // Generar token JWT
                var token = await _jwtService.GenerateTokenAsync(request);

                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogWarning("Autenticación fallida para el usuario: {UserName}", request.UserName);
                    return Unauthorized(ResponseApi<string>.Error("Credenciales inválidas."));
                }

                _logger.LogInformation("Autenticación exitosa para el usuario: {UserName}", request.UserName);
                return Ok(ResponseApi<string>.Success(token, "Autenticación exitosa."));
            }
            catch (ValidationException ve)
            {
                _logger.LogWarning("Error de validación en el proceso de login: {Message}", ve.Message);
                return BadRequest(ResponseApi<string>.Error(ve.Message));
            }
            catch (UnauthorizedException ue)
            {
                _logger.LogWarning("Error de autenticación en el proceso de login: {Message}", ue.Message);
                return Unauthorized(ResponseApi<string>.Error(ue.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado en el proceso de login.");
                return StatusCode(500, ResponseApi<string>.Error("Error interno del servidor."));
            }
        }
    }
}