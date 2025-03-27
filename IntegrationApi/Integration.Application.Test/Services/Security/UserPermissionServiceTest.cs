using Integration.Application.Interfaces.Security;
using Integration.Application.Services.Security;
using Integration.Infrastructure.Interfaces.Security;
using Integration.Shared.DTO.Security;

using Microsoft.Extensions.Logging;

using Moq;

namespace Integration.Application.Test.Services.Security
{
    [TestFixture]
    public class UserPermissionServiceTest
    {
        private Mock<IUserPermissionRepository> _repositoryMock;
        private Mock<IApplicationRepository> _applicationRepositoryMock;
        private Mock<ILogger<UserPermissionService>> _loggerMock;
        private IUserPermissionService _service;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IUserPermissionRepository>();
            _applicationRepositoryMock = new Mock<IApplicationRepository>();
            _loggerMock = new Mock<ILogger<UserPermissionService>>();

            _service = new UserPermissionService(
                _repositoryMock.Object,
                _loggerMock.Object,
                _applicationRepositoryMock.Object
            );
        }

        [Test]
        public async Task GetAllActiveByUserCodeAsync_ShouldReturnPermissions_WhenApplicationExists()
        {
            // Arrange
            string userCode = "USR0000001";
            string applicationCode = "APP0000001";

            var application = new Integration.Core.Entities.Security.Application { 
                Id = 1, 
                Code = applicationCode, 
                Name = "TestApp",
                CreatedAt = DateTime.Now,
                IsActive = true 
            };
            var expectedPermissions = new List<UserPermissionDTOResponse>
            {
                new UserPermissionDTOResponse
                {
                    CodeUser = userCode,
                    CodeRole = "ROL0000001",
                    Role = "Administrador",
                    CodeModule = "MOD0000001",
                    Module = "Módulo 1",
                    CodePermission = "PER0000001",
                    Permission = "Ver",
                    UserName = "TestUser"
                }
            };

            _applicationRepositoryMock
                .Setup(repo => repo.GetByCodeAsync(applicationCode))
                .ReturnsAsync(application);

            _repositoryMock
                .Setup(repo => repo.GetAllActiveByUserIdAsync(userCode, application.Id))
                .ReturnsAsync(expectedPermissions);

            // Act
            var result = await _service.GetAllActiveByUserCodeAsync(userCode, applicationCode);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedPermissions.Count, result.Count());
            Assert.AreEqual(expectedPermissions.First().CodePermission, result.First().CodePermission);
        }

        [Test]
        public void GetAllActiveByUserCodeAsync_ShouldThrowException_WhenApplicationNotFound()
        {
            // Arrange
            string userCode = "USR0000001";
            string applicationCode = "APP_INVALID";

            _applicationRepositoryMock
                .Setup(repo => repo.GetByCodeAsync(applicationCode))
                .ReturnsAsync((Integration.Core.Entities.Security.Application)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<NullReferenceException>(() =>
                _service.GetAllActiveByUserCodeAsync(userCode, applicationCode));

            Assert.That(ex.Message, Is.Null.Or.Contain("Object reference"));
        }

        [Test]
        public void GetAllActiveByUserCodeAsync_ShouldLogError_WhenExceptionIsThrown()
        {
            // Arrange
            string userCode = "USR0000001";
            string applicationCode = "APP0000001";

            _applicationRepositoryMock
                .Setup(repo => repo.GetByCodeAsync(applicationCode))
                .ThrowsAsync(new Exception("Simulated failure"));

            // Act & Assert
            Assert.ThrowsAsync<Exception>(() => _service.GetAllActiveByUserCodeAsync(userCode, applicationCode));

            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error al obtener todos los UserPermissionDTOResponse")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}