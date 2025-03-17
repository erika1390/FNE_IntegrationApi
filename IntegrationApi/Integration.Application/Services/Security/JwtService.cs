using Integration.Application.Interfaces.Security;
using Integration.Shared.DTO.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace Integration.Application.Services.Security
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<JwtService> _logger;
        private readonly IAuthenticationService _authenticationService;
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expirationInMinutes;

        public JwtService(IConfiguration config, ILogger<JwtService> logger, IAuthenticationService authenticationService)
        {
            _config = config;
            _logger = logger;
            _authenticationService = authenticationService;
            _secretKey = _config["JwtSettings:SecretKey"];
            _issuer = _config["JwtSettings:Issuer"];
            _audience = _config["JwtSettings:Audience"];
            _expirationInMinutes = int.Parse(_config["JwtSettings:ExpirationInMinutes"]);
        }

        public async Task<string> GenerateTokenAsync(LoginRequestDTO request)
        {
            _logger.LogInformation("Generando token para usuario: {UserName}", request.UserName);

            if (string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Password))
            {
                _logger.LogWarning("Credenciales inválidas proporcionadas.");
                throw new ArgumentException("Nombre de usuario y contraseña son obligatorios.");
            }

            bool isValid = await _authenticationService.ValidateCredentialsAsync(request.UserName, request.Password);

            if (!isValid)
            {
                _logger.LogWarning("Credenciales inválidas para usuario: {UserName}", request.UserName);
                throw new UnauthorizedAccessException("Credenciales inválidas.");
            }

            var key = Encoding.UTF8.GetBytes(_secretKey);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, request.UserName),
                new Claim(ClaimTypes.Role, "User") // Ajusta esto según tus necesidades
            };

            Console.WriteLine($"Claims generados: {string.Join(", ", claims.Select(c => $"{c.Type}: {c.Value}"))}");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_expirationInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _issuer,
                Audience = _audience
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            _logger.LogInformation("Token generado con éxito para usuario: {UserName}", request.UserName);
            return tokenString;
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            return await Task.Run(() =>
            {
                _logger.LogInformation("Iniciando validación del token JWT.");
                if (string.IsNullOrWhiteSpace(token))
                {
                    _logger.LogWarning("Validación JWT fallida: no se proporcionó el token.");
                    return false;
                }
                // Si el token viene con prefijo "Bearer ", lo eliminamos.
                if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    token = token.Substring("Bearer ".Length).Trim();
                }
                try
                {
                    // Preparar la clave secreta y parámetros de validación
                    var secretKeyBytes = Encoding.UTF8.GetBytes(_secretKey);
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var validationParams = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true, // Validar la firma con la clave secreta
                        IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),
                        ValidateIssuer = true,
                        ValidIssuer = _issuer,
                        ValidateAudience = true,
                        ValidAudience = _audience,
                        ValidateLifetime = true, // Verificar expiración del token
                        ClockSkew = TimeSpan.Zero // Sin margen de tiempo adicional en expiración
                    };
                    // Validar el token (verifica firma, expiración, emisor y audiencia)
                    tokenHandler.ValidateToken(token, validationParams, out _);
                    _logger.LogInformation("Token JWT válido.");
                    return true;
                }
                catch (SecurityTokenExpiredException ex)
                {
                    // Token expirado
                    _logger.LogWarning(ex, "Token JWT expirado.");
                }
                catch (SecurityTokenException ex)
                {
                    // Error en la firma, emisor, audiencia u otro relacionado con JWT inválido
                    _logger.LogWarning(ex, "Token JWT inválido: {Reason}", ex.Message);
                }
                catch (Exception ex)
                {
                    // Cualquier otra excepción inesperada
                    _logger.LogError(ex, "Error inesperado durante la validación del JWT.");
                }

                return false; // Si hubo cualquier error, el token es inválido
            });
        }
    }
}