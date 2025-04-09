using Integration.Application.Interfaces.Security;

namespace Integration.Api.Middlewares
{
    public class HeaderMiddleware
    {
        private readonly RequestDelegate _next;

        public HeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ICurrentUserService currentUserService, IUserService userService)
        {
            if (context.Request.Headers.TryGetValue("UserCode", out var userCodeHeader))
            {
                string userCode = userCodeHeader.ToString();
                currentUserService.UserCode = userCode;

                // Asegúrate de que esta línea exista y esté siendo esperada correctamente
                string userName = await userService.GetUserNameByCodeAsync(userCode);
                currentUserService.UserName = userName; // Esta línea es crítica
            }
            await _next(context);
        }
    }
}