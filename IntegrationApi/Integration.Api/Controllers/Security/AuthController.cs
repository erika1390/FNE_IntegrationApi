using Integration.Application.Exceptions;
using Integration.Application.Interfaces.Security;
using Integration.Shared.DTO.Security;
using Integration.Shared.Response;

using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;
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
                    return BadRequest(ResponseApi<AuthDTO>.Error("Los datos de login no pueden ser nulos."));
                }

                // Validación del modelo de entrada
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Se recibió una solicitud de login con datos inválidos.");
                    return BadRequest(ResponseApi<AuthDTO>.Error("Datos de entrada inválidos."));
                }

                // Validar que usuario y contraseña no estén vacíos
                if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
                {
                    _logger.LogWarning("Intento de inicio de sesión con datos vacíos.");
                    return BadRequest(ResponseApi<AuthDTO>.Error("El usuario y la contraseña son obligatorios."));
                }

                _logger.LogInformation("Intento de inicio de sesión para el usuario: {UserName}", request.UserName);

                // Generar token JWT
                var token = await _jwtService.GenerateTokenAsync(request);
                var user = await _userService.GetByUserNameAsync(request.UserName);

                AuthDTO authDTO = new AuthDTO
                {
                    Token = token,
                    UserCode = user.Code,
                };

                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogWarning("Autenticación fallida para el usuario: {UserName}", request.UserName);
                    return Unauthorized(ResponseApi<AuthDTO>.Error("Credenciales inválidas."));
                }

                _logger.LogInformation("Autenticación exitosa para el usuario: {UserName}", request.UserName);
                return Ok(ResponseApi<AuthDTO>.Success(authDTO, "Autenticación exitosa."));
            }
            catch (ValidationException ve)
            {
                _logger.LogWarning("Error de validación en el proceso de login: {Message}", ve.Message);
                return BadRequest(ResponseApi<AuthDTO>.Error(ve.Message));
            }
            catch (UnauthorizedException ue)
            {
                _logger.LogWarning("Error de autenticación en el proceso de login: {Message}", ue.Message);
                return Unauthorized(ResponseApi<AuthDTO>.Error(ue.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado en el proceso de login.");
                return StatusCode(500, ResponseApi<AuthDTO>.Error("Error interno del servidor."));
            }
        }

        [HttpGet("validate-token")]
        public async Task<IActionResult> ValidateToken()
        {
            try
            {
                var authHeader = Request.Headers["Authorization"].ToString();

                if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
                {
                    _logger.LogWarning("Encabezado Authorization inválido o ausente.");
                    return BadRequest(ResponseApi<bool>.Error("Encabezado Authorization inválido o ausente."));
                }

                var token = authHeader.Substring("Bearer ".Length).Trim();

                var isValid = await _jwtService.ValidateTokenAsync(token);
                if (!isValid)
                {
                    _logger.LogWarning("Token JWT inválido.");
                    return Unauthorized(ResponseApi<bool>.Error("Token inválido o expirado."));
                }

                _logger.LogInformation("Token JWT validado exitosamente.");
                return Ok(ResponseApi<bool>.Success(true, "Token válido."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al validar el token.");
                return StatusCode(500, ResponseApi<bool>.Error("Error interno del servidor."));
            }
        }
    }
}