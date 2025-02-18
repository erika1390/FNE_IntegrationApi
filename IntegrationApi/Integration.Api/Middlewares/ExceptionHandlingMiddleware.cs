using Integration.Application.Exceptions;
using Integration.Shared.Response;

using System.Net;
using System.Text.Json;

using UnauthorizedException = Integration.Application.Exceptions.UnauthorizedException;

namespace Integration.Api.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled Exception");

                var response = context.Response;
                response.ContentType = "application/json";

                var (statusCode, message) = ex switch
                {
                    NotFoundException => (HttpStatusCode.NotFound, ex.Message),
                    ValidationException => (HttpStatusCode.BadRequest, ex.Message),
                    UnauthorizedException => (HttpStatusCode.Unauthorized, ex.Message),
                    BusinessException => (HttpStatusCode.Conflict, ex.Message),
                    _ => (HttpStatusCode.InternalServerError, "Ha ocurrido un error inesperado.")
                };

                response.StatusCode = (int)statusCode;
                var errorResponse = ResponseApi<string>.Error(message);

                await response.WriteAsync(JsonSerializer.Serialize(errorResponse));
            }
        }
    }
}