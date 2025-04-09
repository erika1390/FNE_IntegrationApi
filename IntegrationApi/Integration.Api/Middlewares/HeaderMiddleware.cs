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
            if (context.Request.Headers.TryGetValue("UserCode", out var userCode))
            {
                currentUserService.UserCode = userCode.ToString();
                currentUserService.UserName = await userService.GetUserNameByCodeAsync(currentUserService.UserCode);
            }

            await _next(context);
        }
    }
}