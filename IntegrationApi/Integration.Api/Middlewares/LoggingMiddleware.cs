using Newtonsoft.Json;

using System.Diagnostics;
using System.Text;

namespace Integration.Api.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            context.Request.EnableBuffering();
            var requestBody = await new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true).ReadToEndAsync();
            context.Request.Body.Position = 0;
            var headers = JsonConvert.SerializeObject(context.Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()));
            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar la solicitud");
                context.Response.StatusCode = 500; // Asegúrate de establecer el código de estado en caso de error
                await context.Response.WriteAsync("Error interno del servidor."); // Escribe un mensaje de error en la respuesta
            }
            finally
            {
                stopwatch.Stop();
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var responseBodyText = await new StreamReader(context.Response.Body).ReadToEndAsync();
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var logMessage = new StringBuilder();
                logMessage.AppendLine($"HTTP {context.Request.Method} {context.Request.Path} responded {context.Response.StatusCode} in {stopwatch.ElapsedMilliseconds}ms");
                logMessage.AppendLine($"Request: {requestBody}, Response: {responseBodyText}, Headers: {headers}");
                _logger.LogInformation(logMessage.ToString());
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }
    }
}