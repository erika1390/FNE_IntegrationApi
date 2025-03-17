using Integration.Shared.DTO.Security;
namespace Integration.Application.Interfaces.Security
{
    public interface IJwtService
    {
        Task<string> GenerateTokenAsync(LoginRequestDTO request);
        Task<bool> ValidateTokenAsync(string token);
    }
}