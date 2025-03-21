using Integration.Application.Interfaces.Security;
using Integration.Infrastructure.Interfaces.Security;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
namespace Integration.Application.Services.Security
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<AuthenticationService> _logger;
        private readonly IUserRepository _userRepository;

        public AuthenticationService(IConfiguration config, ILogger<AuthenticationService> logger, IUserRepository userRepository)
        {
            _config = config;
            _logger = logger;
            _userRepository = userRepository;
        }

        public async Task<string> GenerarPasswordHashAsync(string password)
        {
            var passwordHasher = new PasswordHasher<object>();
            return await Task.FromResult(passwordHasher.HashPassword(null, password));
        }

        public async Task<bool> VerifyPassword(string storedHash, string inputPassword)
        {
            var passwordHasher = new PasswordHasher<object>();
            var result = passwordHasher.VerifyHashedPassword(null, storedHash, inputPassword);

            return result == PasswordVerificationResult.Success;
        }

        public async Task<bool> ValidateCredentialsAsync(string userName, string password)
        {
            _logger.LogInformation("Validando credenciales para usuario: {UserName}", userName);

            var users = await _userRepository.GetByFilterAsync(u => u.UserName == userName && u.IsActive == true);

            // ✅ Verificar si la lista de usuarios está vacía
            var user = users.FirstOrDefault();
            if (user == null)
            {
                _logger.LogWarning("Usuario no encontrado: {UserName}", userName);
                return false;
            }

            var passwordHash = await VerifyPassword(user.PasswordHash, password);

            if (passwordHash)
            {
                _logger.LogInformation("Credenciales válidas para usuario: {UserName}", userName);
                return true;
            }
            else
            {
                _logger.LogWarning("Credenciales inválidas para usuario: {UserName}", userName);
                return false;
            }
        }
    }
}