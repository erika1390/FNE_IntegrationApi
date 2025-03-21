using Integration.Application.Interfaces.Security;
using Integration.Shared.DTO.Header;
using Integration.Shared.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace Integration.Api.Filters
{
    public class ValidateHeadersFilter : ActionFilterAttribute
    {
        private readonly IConfiguration _config;
        private readonly ILogger<ValidateHeadersFilter> _logger;
        private readonly string _secretKey = string.Empty;
        private readonly IJwtService _jwtService;

        public ValidateHeadersFilter(IConfiguration config, ILogger<ValidateHeadersFilter> logger, IJwtService jwtService)
        {
            _config = config;
            _logger = logger;
            _jwtService = jwtService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var errors = new List<string>();

            if (!context.ActionArguments.TryGetValue("header", out var value) || value is not HeaderDTO header)
            {
                _logger.LogWarning("No se proporcionó el objeto HeaderDTO.");
                context.Result = new BadRequestObjectResult(ResponseApi<object>.Error(new List<string> { "No se proporcionó el objeto HeaderDTO." }, "Error en los headers."));
                return;
            }

            if (string.IsNullOrWhiteSpace(header.ApplicationCode))
                errors.Add("El campo ApplicationCode es obligatorio.");

            if (string.IsNullOrWhiteSpace(header.RolCode))
                errors.Add("El campo RolCode es obligatorio.");

            if (string.IsNullOrWhiteSpace(header.UserCode))
                errors.Add("El campo UserCode es obligatorio.");            

            if (string.IsNullOrWhiteSpace(header.Authorization))
                errors.Add("El campo Authorization es obligatorio.");
            else
            {
                // Validar el token JWT
                var token = header.Authorization.StartsWith("Bearer ") ? header.Authorization.Substring(7) : null;

                if (string.IsNullOrWhiteSpace(token))
                {
                    errors.Add("El token JWT es inválido o está mal formado.");
                }
                else if (!_jwtService.ValidateTokenAsync(token).Result)
                {
                    errors.Add("El token JWT no es válido o ha expirado.");
                }
            }
            if (errors.Any())
            {
                _logger.LogWarning("Errores en los headers: {Errors}", string.Join(", ", errors));
                context.Result = new BadRequestObjectResult(ResponseApi<object>.Error(errors, "Errores en la validación de los headers."));
            }
        }
    }
}