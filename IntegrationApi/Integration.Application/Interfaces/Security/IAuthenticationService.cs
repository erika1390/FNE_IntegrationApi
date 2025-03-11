namespace Integration.Application.Interfaces.Security
{
    public interface IAuthenticationService
    {
        Task<bool> ValidateCredentialsAsync(string userName, string password);
        Task<string> GenerarPasswordHashAsync(string password);
        Task<bool> VerifyPassword(string storedHash, string inputPassword);
    }
}