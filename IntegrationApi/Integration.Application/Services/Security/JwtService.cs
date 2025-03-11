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

        public JwtService(IConfiguration config, ILogger<JwtService> logger, IAuthenticationService authenticationService)
        {
            _config = config;
            _logger = logger;
            _authenticationService = authenticationService;
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

            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, request.UserName),
                new Claim(ClaimTypes.Role, "User") // Ajusta esto según tus necesidades
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            _logger.LogInformation("Token generado con éxito para usuario: {UserName}", request.UserName);
            return tokenString;
        }
    }
}