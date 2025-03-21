using Integration.Application.Services.Security;
using Integration.Core.Entities.Security;
using Integration.Infrastructure.Interfaces.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;
namespace Integration.Application.Test.Services.Security
{
    [TestFixture]
    public class AuthenticationServiceTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<ILogger<AuthenticationService>> _loggerMock;
        private AuthenticationService _authenticationService;

        [SetUp]
        public void SetUp()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _loggerMock = new Mock<ILogger<AuthenticationService>>();

            _authenticationService = new AuthenticationService(null, _loggerMock.Object, _userRepositoryMock.Object);
        }

        // ✅ PRUEBA: GenerarPasswordHashAsync debe devolver un hash válido
        [Test]
        public async Task GenerarPasswordHashAsync_ShouldReturn_Hash()
        {
            // Arrange
            var password = "SecurePassword123!";

            // Act
            var hash = await _authenticationService.GenerarPasswordHashAsync(password);

            // Assert
            Assert.IsNotNull(hash);
            Assert.IsNotEmpty(hash);
        }

        // ✅ PRUEBA: VerifyPassword debe retornar true si la contraseña coincide con el hash
        [Test]
        public async Task VerifyPassword_ShouldReturnTrue_WhenPasswordMatches()
        {
            // Arrange
            var password = "SecurePassword123!";
            var passwordHasher = new PasswordHasher<object>();
            var hash = passwordHasher.HashPassword(null, password);

            // Act
            var isMatch = await _authenticationService.VerifyPassword(hash, password);

            // Assert
            Assert.IsTrue(isMatch);
        }

        // ✅ PRUEBA: VerifyPassword debe retornar false si la contraseña NO coincide
        [Test]
        public async Task VerifyPassword_ShouldReturnFalse_WhenPasswordDoesNotMatch()
        {
            // Arrange
            var password = "SecurePassword123!";
            var incorrectPassword = "WrongPassword!";
            var passwordHasher = new PasswordHasher<object>();
            var hash = passwordHasher.HashPassword(null, password);

            // Act
            var isMatch = await _authenticationService.VerifyPassword(hash, incorrectPassword);

            // Assert
            Assert.IsFalse(isMatch);
        }

        // ✅ PRUEBA: ValidateCredentialsAsync debe retornar true si el usuario y contraseña son válidos
        [Test]
        public async Task ValidateCredentialsAsync_ShouldReturnTrue_WhenCredentialsAreValid()
        {
            // Arrange
            var userName = "testUser";
            var password = "SecurePassword123!";
            var passwordHasher = new PasswordHasher<object>();
            var hash = passwordHasher.HashPassword(null, password);

            var user = new User
            {
                Id = 1,
                Code = "USR0000001",
                FirstName = "Erika",
                LastName = "Pulido Moreno",
                DateOfBirth = DateTime.Now.AddYears(-34),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CreatedBy = "epulido",
                UpdatedBy = "epulido",
                IsActive = true,
                UserName = "epulido",
                NormalizedUserName = "EPULIDO",
                Email = "epulido@minsalud.gov.co",
                NormalizedEmail = "EPULIDO@MINSALUD.GOV.CO",
                EmailConfirmed = true,
                PasswordHash = hash,  // ✅ Usa el hash generado correctamente
                SecurityStamp = "QJZQ4Q",
                ConcurrencyStamp = "d1b1b2b3-4b5b-6b7b-8b9b-0b1b2b3b4b5",
                PhoneNumber = "3001234567",
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                LockoutEnd = null,
                LockoutEnabled = true,
                AccessFailedCount = 0
            };

            // ✅ Configurar el mock del repositorio con una expresión válida
            _userRepositoryMock
                .Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(new List<User> { user });

            // Act
            var result = await _authenticationService.ValidateCredentialsAsync(userName, password);

            // Assert
            Assert.IsTrue(result);

            // ✅ Verificar que `ILogger` recibió un `LogInformation`
            _loggerMock.Verify(
                log => log.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(userName)),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()
                ),
                Times.AtLeastOnce()
            );
        }

        // ✅ PRUEBA: ValidateCredentialsAsync debe retornar false si la contraseña es incorrecta
        [Test]
        public async Task ValidateCredentialsAsync_ShouldReturnFalse_WhenPasswordIsInvalid()
        {
            // Arrange
            var userName = "testUser";
            var password = "SecurePassword123!";
            var incorrectPassword = "WrongPassword!";
            var hash = "AQAAAAIAAYagAAAAEHLsg8Ql1DOCO81TgXaBEyfdwyB2bOKyTLpHORs1ZOYKPFttx83Q+cuLI2zPV2FEyA==";

            var user = new User
            {
                Id = 1,
                Code = "USR0000001",
                FirstName = "Erika",
                LastName = "Pulido Moreno",
                DateOfBirth = DateTime.Now.AddYears(-34),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CreatedBy = "epulido",
                UpdatedBy = "epulido",
                IsActive = true,
                UserName = "epulido",
                NormalizedUserName = "EPULIDO",
                Email = "epulido@minsalud.gov.co",
                NormalizedEmail = "EPULIDO@MINSALUD.GOV.CO",
                EmailConfirmed = true,
                PasswordHash = hash,
                SecurityStamp = "QJZQ4Q",
                ConcurrencyStamp = "d1b1b2b3-4b5b-6b7b-8b9b-0b1b2b3b4b5",
                PhoneNumber = "3001234567",
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                LockoutEnd = null,
                LockoutEnabled = true,
                AccessFailedCount = 0
            };

            // ✅ Configurar el mock del repositorio con una expresión válida
            _userRepositoryMock
                .Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(new List<User> { user });

            // Act
            var result = await _authenticationService.ValidateCredentialsAsync(userName, incorrectPassword);

            // Assert
            Assert.IsFalse(result);

            // ✅ Verificar que `ILogger` recibió un `LogWarning`
            _loggerMock.Verify(
                log => log.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(userName)),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()
                ),
                Times.AtLeastOnce()
            );
        }
        // ✅ PRUEBA: ValidateCredentialsAsync debe retornar false si el usuario NO existe
        [Test]
        public async Task ValidateCredentialsAsync_ShouldReturnFalse_WhenUserDoesNotExist()
        {
            // Arrange
            var userName = "epulido";
            var password = "SecurePassword123!";

            _userRepositoryMock
             .Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<User, bool>>>()))
             .ReturnsAsync(new List<User>()); // Simula que el usuario no existe

            // Act
            var result = await _authenticationService.ValidateCredentialsAsync(userName, password);

            // Assert
            Assert.IsFalse(result);

            // ✅ Verificar que el log de advertencia se haya llamado
            _loggerMock.Verify(
                log => log.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Usuario no encontrado")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()
                ),
                Times.AtLeastOnce()
            );
        }
    }
}