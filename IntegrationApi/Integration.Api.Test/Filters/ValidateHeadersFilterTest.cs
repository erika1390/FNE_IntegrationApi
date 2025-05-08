using Integration.Api.Filters;
using Integration.Application.Interfaces.Security;
using Integration.Shared.DTO.Header;
using Integration.Shared.Response;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Moq;

namespace Integration.Api.Test.Filters
{
    [TestFixture]
    public class ValidateHeadersFilterTest
    {
        private Mock<IConfiguration> _configMock;
        private Mock<ILogger<ValidateHeadersFilter>> _loggerMock;
        private Mock<IJwtService> _jwtServiceMock;
        private ValidateHeadersFilter _filter;

        [SetUp]
        public void SetUp()
        {
            _configMock = new Mock<IConfiguration>();
            _loggerMock = new Mock<ILogger<ValidateHeadersFilter>>();
            _jwtServiceMock = new Mock<IJwtService>();
            _filter = new ValidateHeadersFilter(_configMock.Object, _loggerMock.Object, _jwtServiceMock.Object);
        }

        [Test]
        public void OnActionExecuting_ShouldReturnBadRequest_WhenHeaderIsMissing()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            var actionContext = new ActionContext(httpContext, new Microsoft.AspNetCore.Routing.RouteData(), new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor());
            var context = new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                new object());

            // Act
            _filter.OnActionExecuting(context);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(context.Result);
            var result = context.Result as BadRequestObjectResult;
            Assert.AreEqual("No se proporcionó el objeto HeaderDTO.", ((ResponseApi<object>)result.Value).Errors.First());
        }

        [Test]
        public void OnActionExecuting_ShouldReturnBadRequest_WhenRequiredFieldsAreMissing()
        {
            // Arrange
            var header = new HeaderDTO();
            var httpContext = new DefaultHttpContext();
            var actionContext = new ActionContext(httpContext, new Microsoft.AspNetCore.Routing.RouteData(), new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor());
            var context = new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object> { { "header", header } },
                new object());

            // Act
            _filter.OnActionExecuting(context);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(context.Result);
            var result = context.Result as BadRequestObjectResult;
            var errors = ((ResponseApi<object>)result.Value).Errors.ToList();
            Assert.Contains("El campo ApplicationCode es obligatorio.", errors);
            Assert.Contains("El campo RoleCode es obligatorio.", errors);
            Assert.Contains("El campo UserCode es obligatorio.", errors);
            Assert.Contains("El campo Authorization es obligatorio.", errors);
        }

        [Test]
        public void OnActionExecuting_ShouldReturnBadRequest_WhenTokenIsInvalid()
        {
            // Arrange
            var header = new HeaderDTO
            {
                ApplicationCode = "APP0000001",
                UserCode = "USER0000001",
                Authorization = "Bearer invalid_token"
            };
            _jwtServiceMock.Setup(s => s.ValidateTokenAsync(It.IsAny<string>())).ReturnsAsync(false);
            var httpContext = new DefaultHttpContext();
            var actionContext = new ActionContext(httpContext, new Microsoft.AspNetCore.Routing.RouteData(), new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor());
            var context = new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object> { { "header", header } },
                new object());

            // Act
            _filter.OnActionExecuting(context);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(context.Result);
            var result = context.Result as BadRequestObjectResult;
            var errors = ((ResponseApi<object>)result.Value).Errors.ToList();
            Assert.Contains("El token JWT no es válido o ha expirado.", errors);
        }

        [Test]
        public void OnActionExecuting_ShouldProceed_WhenHeadersAreValid()
        {
            // Arrange
            var header = new HeaderDTO
            {
                ApplicationCode = "APP0000001",
                UserCode = "USER0000001",
                Authorization = "Bearer valid_token"
            };
            _jwtServiceMock.Setup(s => s.ValidateTokenAsync(It.IsAny<string>())).ReturnsAsync(true);
            var httpContext = new DefaultHttpContext();
            var actionContext = new ActionContext(httpContext, new Microsoft.AspNetCore.Routing.RouteData(), new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor());
            var context = new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object> { { "header", header } },
                new object());

            // Act
            _filter.OnActionExecuting(context);

            // Assert
            Assert.IsNull(context.Result);
        }
    }
}