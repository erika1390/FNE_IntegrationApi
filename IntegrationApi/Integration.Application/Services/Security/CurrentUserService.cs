using Integration.Application.Interfaces.Security;

namespace Integration.Application.Services.Security
{
    public class CurrentUserService : ICurrentUserService
    {
        public string UserCode { get; set; } = "system";
        public string UserName { get; set; } = "system";
    }
}