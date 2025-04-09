namespace Integration.Application.Interfaces.Security
{
    public interface ICurrentUserService
    {
        string UserCode { get; set; }
        string UserName { get; set; }
    }
}