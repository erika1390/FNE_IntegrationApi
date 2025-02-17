namespace Integration.Application.Interfaces.Security
{
    public interface IJwtService
    {
        string GenerateToken(string username, string role);
    }
}