using Integration.Api.Controllers.Security;
using Integration.Application.Interfaces.Security;
using Integration.Shared.DTO.Header;
using Integration.Shared.DTO.Security;
using Integration.Shared.Response;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Moq;

namespace Integration.Api.Test.Controllers.Security
{
    [TestFixture]
    public class UserPermissionControllerTest
    {
        private Mock<IUserPermissionService> _serviceMock;
        private Mock<ILogger<UserPermissionController>> _loggerMock;
        private UserPermissionController _controller;

        [SetUp]
        public void SetUp()
        {
            _serviceMock = new Mock<IUserPermissionService>();
            _loggerMock = new Mock<ILogger<UserPermissionController>>();
            _controller = new UserPermissionController(_serviceMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task GetAllPermissionsByUserCodeAsync_ShouldReturnOk_WhenPermissionsExist()
        {
            // Arrange
            var header = new HeaderDTO { UserCode = "USR0000001", ApplicationCode = "APP0000001" };
            var permissions = new UserPermissionDTO
            {
                CodeUser = "USR0000001",
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

            _serviceMock.Setup(s => s.GetAllPermissionsByUserCodeAsync(header.UserCode, header.ApplicationCode))
                        .ReturnsAsync(permissions);

            // Act
            var result = await _controller.GetAllPermissionsByUserCodeAsync(header);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var response = okResult.Value as ResponseApi<UserPermissionDTO>;
            Assert.IsNotNull(response);
            Assert.AreEqual("USR0000001", response.Data.CodeUser);
            Assert.AreEqual("epulido", response.Data.UserName);
        }

        [Test]
        public async Task GetAllPermissionsByUserCodeAsync_ShouldReturnNotFound_WhenNoPermissionsExist()
        {
            // Arrange
            var header = new HeaderDTO { UserCode = "USR0000001", ApplicationCode = "APP0000001" };
            _serviceMock.Setup(s => s.GetAllPermissionsByUserCodeAsync(header.UserCode, header.ApplicationCode))
                        .ReturnsAsync((UserPermissionDTO)null);

            // Act
            var result = await _controller.GetAllPermissionsByUserCodeAsync(header);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);

            var response = notFoundResult.Value as ResponseApi<UserPermissionDTO>;
            Assert.IsNotNull(response);
            Assert.AreEqual("No se encontraron usuarios.", response.Message);
        }

        [Test]
        public void GetAllPermissionsByUserCodeAsync_ShouldThrowException_WhenServiceFails()
        {
            // Arrange
            var header = new HeaderDTO { UserCode = "USR0000001", ApplicationCode = "APP0000001" };
            _serviceMock.Setup(s => s.GetAllPermissionsByUserCodeAsync(header.UserCode, header.ApplicationCode))
                        .ThrowsAsync(new Exception("Unexpected error"));

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(() => _controller.GetAllPermissionsByUserCodeAsync(header));
            Assert.AreEqual("Unexpected error", ex.Message);
        }
    }
}