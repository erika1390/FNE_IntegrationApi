using Integration.Api.Controllers.Security;
using Integration.Application.Exceptions;
using Integration.Application.Interfaces.Security;
using Integration.Shared.DTO.Security;
using Integration.Shared.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
namespace Integration.Api.Test.Controllers.Security
{
    [TestFixture]
    public class AuthControllerTest
    {
        private Mock<IJwtService> _jwtServiceMock;
        private Mock<IUserService> _userServiceMock;
        private Mock<ILogger<AuthController>> _loggerMock;
        private Mock<IConfiguration> _configurationMock;
        private AuthController _controller;

        [SetUp]
        public void SetUp()
        {
            _jwtServiceMock = new Mock<IJwtService>();
            _userServiceMock = new Mock<IUserService>();
            _loggerMock = new Mock<ILogger<AuthController>>();
            _configurationMock = new Mock<IConfiguration>();
            _controller = new AuthController(_jwtServiceMock.Object, _loggerMock.Object, _configurationMock.Object, _userServiceMock.Object);
        }

        [Test]
        public async Task Login_ShouldReturnOk_WhenCredentialsAreValid()
        {
            // Arrange
            var request = new LoginRequestDTO { UserName = "testuser", Password = "password123" };
            _jwtServiceMock.Setup(s => s.GenerateTokenAsync(request)).ReturnsAsync("valid-token");

            // Act
            var result = await _controller.Login(request);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsInstanceOf<ResponseApi<string>>(okResult.Value);

            var response = okResult.Value as ResponseApi<string>;
            Assert.AreEqual("Autenticación exitosa.", response.Message);
            Assert.AreEqual("valid-token", response.Data);
        }

        [Test]
        public async Task Login_ShouldReturnBadRequest_WhenRequestIsNull()
        {
            // Act
            var result = await _controller.Login(null);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
        }

        [Test]
        public async Task Login_ShouldReturnBadRequest_WhenUserNameOrPasswordIsEmpty()
        {
            // Arrange
            var request = new LoginRequestDTO { UserName = "", Password = "password123" };

            // Act
            var result = await _controller.Login(request);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
            Assert.IsInstanceOf<ResponseApi<string>>(badRequestResult.Value);

            var response = badRequestResult.Value as ResponseApi<string>;
            Assert.AreEqual("El usuario y la contraseña son obligatorios.", response.Message);
        }

        [Test]
        public async Task Login_ShouldReturnUnauthorized_WhenCredentialsAreInvalid()
        {
            // Arrange
            var request = new LoginRequestDTO { UserName = "testuser", Password = "wrongpassword" };
            _jwtServiceMock.Setup(s => s.GenerateTokenAsync(request)).ReturnsAsync(string.Empty);

            // Act
            var result = await _controller.Login(request);

            // Assert
            var unauthorizedResult = result as UnauthorizedObjectResult;
            Assert.NotNull(unauthorizedResult);
            Assert.AreEqual(401, unauthorizedResult.StatusCode);
            Assert.IsInstanceOf<ResponseApi<string>>(unauthorizedResult.Value);

            var response = unauthorizedResult.Value as ResponseApi<string>;
            Assert.AreEqual("Credenciales inválidas.", response.Message);
        }

        [Test]
        public async Task Login_ShouldReturnBadRequest_WhenValidationExceptionOccurs()
        {
            // Arrange
            var request = new LoginRequestDTO { UserName = "testuser", Password = "password123" };
            _jwtServiceMock.Setup(s => s.GenerateTokenAsync(request)).ThrowsAsync(new ValidationException("Error de validación"));

            // Act
            var result = await _controller.Login(request);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
            Assert.IsInstanceOf<ResponseApi<string>>(badRequestResult.Value);

            var response = badRequestResult.Value as ResponseApi<string>;
            Assert.AreEqual("Error de validación", response.Message);
        }

        [Test]
        public async Task Login_ShouldReturnUnauthorized_WhenUnauthorizedExceptionOccurs()
        {
            // Arrange
            var request = new LoginRequestDTO { UserName = "testuser", Password = "password123" };
            _jwtServiceMock.Setup(s => s.GenerateTokenAsync(request)).ThrowsAsync(new UnauthorizedException("No autorizado"));

            // Act
            var result = await _controller.Login(request);

            // Assert
            var unauthorizedResult = result as UnauthorizedObjectResult;
            Assert.NotNull(unauthorizedResult);
            Assert.AreEqual(401, unauthorizedResult.StatusCode);
            Assert.IsInstanceOf<ResponseApi<string>>(unauthorizedResult.Value);

            var response = unauthorizedResult.Value as ResponseApi<string>;
            Assert.AreEqual("No autorizado", response.Message);
        }

        [Test]
        public async Task Login_ShouldReturnInternalServerError_WhenUnexpectedExceptionOccurs()
        {
            // Arrange
            var request = new LoginRequestDTO { UserName = "testuser", Password = "password123" };
            _jwtServiceMock.Setup(s => s.GenerateTokenAsync(request)).ThrowsAsync(new Exception("Error inesperado"));

            // Act
            var result = await _controller.Login(request);

            // Assert
            var internalServerErrorResult = result as ObjectResult;
            Assert.NotNull(internalServerErrorResult);
            Assert.AreEqual(500, internalServerErrorResult.StatusCode);
            Assert.IsInstanceOf<ResponseApi<string>>(internalServerErrorResult.Value);

            var response = internalServerErrorResult.Value as ResponseApi<string>;
            Assert.AreEqual("Error interno del servidor.", response.Message);
        }
    }
}