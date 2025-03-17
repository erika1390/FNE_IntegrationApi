using Integration.Shared.DTO.Header;
using Integration.Shared.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace Integration.Api.Filters
{
    public class ValidateHeadersFilter : ActionFilterAttribute
    {
        private readonly ILogger<ValidateHeadersFilter> _logger;

        public ValidateHeadersFilter(ILogger<ValidateHeadersFilter> logger)
        {
            _logger = logger;
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

            if (string.IsNullOrWhiteSpace(header.UserCode))
                errors.Add("El campo UserCode es obligatorio.");

            if (string.IsNullOrWhiteSpace(header.Authorization))
                errors.Add("El campo Authorization es obligatorio.");

            if (errors.Any())
            {
                _logger.LogWarning("Errores en los headers: {Errors}", string.Join(", ", errors));
                context.Result = new BadRequestObjectResult(ResponseApi<object>.Error(errors, "Errores en la validación de los headers."));
            }
        }
    }
}