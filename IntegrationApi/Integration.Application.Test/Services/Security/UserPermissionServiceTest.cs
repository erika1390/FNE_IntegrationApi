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
        private UserPermissionService _service;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IUserPermissionRepository>();
            _applicationRepositoryMock = new Mock<IApplicationRepository>();
            _loggerMock = new Mock<ILogger<UserPermissionService>>();
            _service = new UserPermissionService(_repositoryMock.Object, _loggerMock.Object, _applicationRepositoryMock.Object);
        }

        [Test]
        public async Task GetAllPermissionsByUserCodeAsync_ShouldReturnPermissions_WhenUserAndApplicationExist()
        {
            // Arrange
            var userCode = "USR0000001";
            var applicationCode = "APP0000001";
            var application = new Integration.Core.Entities.Security.Application { Id = 1, Code = applicationCode, Name = "Integrador" };
            var permissions = new UserPermissionDTO
            {
                CodeUser = userCode,
                UserName = "epulido",
                Roles = new List<RoleDto>
                {
                    new RoleDto
                    {
                        Code = "ROL0000001",
                        Name = "Administrador Integrador",
                        Modules = new List<ModuleDto>
                        {
                            new ModuleDto
                            {
                                Code = "MOD0000001",
                                Name = "Configuración",
                                Menus = new List<MenuDto>
                                {
                                    new MenuDto
                                    {
                                        Code = "MNU0000001",
                                        Name = "Configuración",
                                        Permissions = new List<PermissionDto>
                                        {
                                            new PermissionDto { Code = "PER0000001", Name = "Consultar" }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            _applicationRepositoryMock.Setup(x => x.GetByCodeAsync(applicationCode)).ReturnsAsync(application);
            _repositoryMock.Setup(x => x.GetAllPermissionsByUserCodeAsync(userCode, application.Id)).ReturnsAsync(permissions);

            // Act
            var result = await _service.GetAllPermissionsByUserCodeAsync(userCode, applicationCode);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(userCode, result.CodeUser);
            Assert.AreEqual("epulido", result.UserName);
            Assert.IsNotEmpty(result.Roles);
            Assert.AreEqual("ROL0000001", result.Roles[0].Code);
            _applicationRepositoryMock.Verify(x => x.GetByCodeAsync(applicationCode), Times.Once);
            _repositoryMock.Verify(x => x.GetAllPermissionsByUserCodeAsync(userCode, application.Id), Times.Once);
        }

        [Test]
        public void GetAllPermissionsByUserCodeAsync_ShouldThrowException_WhenApplicationDoesNotExist()
        {
            // Arrange
            var userCode = "USR0000001";
            var applicationCode = "APP0000001";

            _applicationRepositoryMock.Setup(x => x.GetByCodeAsync(applicationCode)).ReturnsAsync((Integration.Core.Entities.Security.Application)null);

            // Act & Assert
            Assert.ThrowsAsync<NullReferenceException>(async () => await _service.GetAllPermissionsByUserCodeAsync(userCode, applicationCode));

            _applicationRepositoryMock.Verify(x => x.GetByCodeAsync(applicationCode), Times.Once);
            _repositoryMock.Verify(x => x.GetAllPermissionsByUserCodeAsync(It.IsAny<string>(), It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void GetAllPermissionsByUserCodeAsync_ShouldThrowException_WhenRepositoryThrowsException()
        {
            // Arrange
            var userCode = "USR0000001";
            var applicationCode = "APP0000001";
            var application = new Integration.Core.Entities.Security.Application { Id = 1, Code = applicationCode, Name = "Integrador" };

            _applicationRepositoryMock.Setup(x => x.GetByCodeAsync(applicationCode)).ReturnsAsync(application);
            _repositoryMock.Setup(x => x.GetAllPermissionsByUserCodeAsync(userCode, application.Id)).ThrowsAsync(new Exception("Repository error"));

            // Act & Assert
            Assert.ThrowsAsync<Exception>(async () => await _service.GetAllPermissionsByUserCodeAsync(userCode, applicationCode));
            _applicationRepositoryMock.Verify(x => x.GetByCodeAsync(applicationCode), Times.Once);
            _repositoryMock.Verify(x => x.GetAllPermissionsByUserCodeAsync(userCode, application.Id), Times.Once);
        }
    }
}