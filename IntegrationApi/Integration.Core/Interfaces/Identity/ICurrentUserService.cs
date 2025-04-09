namespace Integration.Application.Interfaces.Security
{
    public interface ICurrentUserService
    {
        string UserId { get; }
        string Username { get; }
    }
}