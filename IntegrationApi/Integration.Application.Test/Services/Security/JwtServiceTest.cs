using Integration.Application.Interfaces.Security;
using Integration.Application.Services.Security;
using Integration.Shared.DTO.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System.IdentityModel.Tokens.Jwt;
namespace Integration.Application.Test.Services.Security
{
    [TestFixture]
    public class JwtServiceTests
    {
        private Mock<IConfiguration> _configMock;
        private Mock<ILogger<JwtService>> _loggerMock;
        private Mock<IAuthenticationService> _authenticationServiceMock;
        private JwtService _jwtService;

        [SetUp]
        public void SetUp()
        {
            _configMock = new Mock<IConfiguration>();
            _loggerMock = new Mock<ILogger<JwtService>>();
            _authenticationServiceMock = new Mock<IAuthenticationService>();

            // Configurar valores simulados en `IConfiguration`
            _configMock.Setup(config => config["JwtSettings:SecretKey"]).Returns("SuperSecretKeyForTestingOnly123!");
            _configMock.Setup(config => config["JwtSettings:Issuer"]).Returns("TestIssuer");
            _configMock.Setup(config => config["JwtSettings:Audience"]).Returns("TestAudience");
            _configMock.Setup(config => config["JwtSettings:ExpirationInMinutes"]).Returns("60");

            _jwtService = new JwtService(_configMock.Object, _loggerMock.Object, _authenticationServiceMock.Object);
        }

        // ✅ PRUEBA: Genera un token cuando las credenciales son válidas
        [Test]
        public async Task GenerateTokenAsync_ShouldReturnToken_WhenCredentialsAreValid()
        {
            // Arrange
            var userName = "epulido";
            var password = "SecurePassword123!";
            var request = new LoginRequestDTO { UserName = userName, Password = password };
            _authenticationServiceMock
                .Setup(auth => auth.ValidateCredentialsAsync(request.UserName, request.Password))
                .ReturnsAsync(true);
            // Act
            var token = await _jwtService.GenerateTokenAsync(request);
            // Assert
            Assert.IsNotNull(token);
            Assert.IsNotEmpty(token);
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            Console.WriteLine($"Token Claims: {string.Join(", ", jwtToken.Claims.Select(c => $"{c.Type}: {c.Value}"))}");
            // ✅ Buscar el claim como "unique_name" en lugar de `ClaimTypes.Name`
            var userClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.UniqueName)?.Value;
            Assert.IsNotNull(userClaim, "Claim 'unique_name' no encontrado en el token.");
            Assert.AreEqual("epulido", userClaim);
        }

        // ✅ PRUEBA: Lanza ArgumentException si el UserName o Password están vacíos
        [Test]
        public void GenerateTokenAsync_ShouldThrowArgumentException_WhenUserNameOrPasswordIsEmpty()
        {
            // Arrange
            var request = new LoginRequestDTO { UserName = "", Password = "ValidPassword123" };

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _jwtService.GenerateTokenAsync(request));
            Assert.AreEqual("Nombre de usuario y contraseña son obligatorios.", ex.Message);

            _loggerMock.Verify(
                log => log.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Credenciales inválidas proporcionadas.")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()
                ),
                Times.Once
            );
        }

        // ✅ PRUEBA: Lanza UnauthorizedAccessException si las credenciales son incorrectas
        [Test]
        public void GenerateTokenAsync_ShouldThrowUnauthorizedAccessException_WhenCredentialsAreInvalid()
        {
            // Arrange
            var request = new LoginRequestDTO { UserName = "testUser", Password = "WrongPassword" };

            _authenticationServiceMock
                .Setup(auth => auth.ValidateCredentialsAsync(request.UserName, request.Password))
                .ReturnsAsync(false);

            // Act & Assert
            var ex = Assert.ThrowsAsync<UnauthorizedAccessException>(() => _jwtService.GenerateTokenAsync(request));
            Assert.AreEqual("Credenciales inválidas.", ex.Message);

            _loggerMock.Verify(
                log => log.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Credenciales inválidas para usuario")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()
                ),
                Times.Once
            );
        }
    }
}
