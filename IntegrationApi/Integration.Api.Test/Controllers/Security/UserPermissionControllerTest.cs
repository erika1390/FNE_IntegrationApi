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
        public void Setup()
        {
            _serviceMock = new Mock<IUserPermissionService>();
            _loggerMock = new Mock<ILogger<UserPermissionController>>();
            _controller = new UserPermissionController(_serviceMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task GetAllActiveByUserCodeAsync_ShouldReturnOk_WhenPermissionsFound()
        {
            // Arrange
            var header = new HeaderDTO { UserCode = "USR0000001", ApplicationCode = "APP0000001" };

            var permissions = new List<UserPermissionDTO>
            {
                new UserPermissionDTO
                {
                    CodeUser = "USR0000001",
                    CodeRole = "ROL0000001",
                    CodeModule = "MOD0000001",
                    CodePermission = "PER0000001",
                    CodeMenu = "MEN0000001",
                    Module = "Módulo 1",
                    Permission = "Permiso 1",
                    Role = "Rol 1",
                    UserName = "Usuario 1",
                    Menu = "Menú 1"
                }
            };

            _serviceMock.Setup(s => s.GetAllActiveByUserCodeAsync(header.UserCode, header.ApplicationCode))
                        .ReturnsAsync(permissions);

            // Act
            var result = await _controller.GetAllActiveByUserCodeAsync(header);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsInstanceOf<ResponseApi<IEnumerable<UserPermissionDTO>>>(okResult.Value);
            var response = okResult.Value as ResponseApi<IEnumerable<UserPermissionDTO>>;
            Assert.AreEqual(permissions.Count, response.Data.Count());
        }

        [Test]
        public async Task GetAllActiveByUserCodeAsync_ShouldReturnNotFound_WhenNoPermissionsFound()
        {
            // Arrange
            var header = new HeaderDTO { UserCode = "USR0000001", ApplicationCode = "APP0000001" };

            _serviceMock.Setup(s => s.GetAllActiveByUserCodeAsync(header.UserCode, header.ApplicationCode))
                        .ReturnsAsync(new List<UserPermissionDTO>());

            // Act
            var result = await _controller.GetAllActiveByUserCodeAsync(header);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            var response = notFoundResult.Value as ResponseApi<IEnumerable<UserPermissionDTO>>;
            Assert.AreEqual("No se encontraron usuarios.", response.Message);
        }

        [Test]
        public void GetAllActiveByUserCodeAsync_ShouldThrowException_WhenServiceFails()
        {
            // Arrange
            var header = new HeaderDTO { UserCode = "USR0000001", ApplicationCode = "APP0000001" };

            _serviceMock.Setup(s => s.GetAllActiveByUserCodeAsync(header.UserCode, header.ApplicationCode))
                        .ThrowsAsync(new Exception("Error inesperado"));

            // Act & Assert
            Assert.ThrowsAsync<Exception>(() => _controller.GetAllActiveByUserCodeAsync(header));
        }
    }
}