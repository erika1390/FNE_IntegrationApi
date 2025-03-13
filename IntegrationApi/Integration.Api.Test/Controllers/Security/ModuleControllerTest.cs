using Integration.Api.Controllers.Security;
using Integration.Application.Interfaces.Security;
using Integration.Shared.DTO.Security;
using Integration.Shared.Response;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
namespace Integration.Api.Tests.Controllers.Security
{
    [TestFixture]
    public class ModuleControllerTest
    {
        private Mock<IModuleService> _serviceMock;
        private Mock<ILogger<ModuleController>> _loggerMock;
        private ModuleController _controller;
        [SetUp]
        public void SetUp()
        {
            _serviceMock = new Mock<IModuleService>();
            _loggerMock = new Mock<ILogger<ModuleController>>();
            _controller = new ModuleController(_serviceMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task GetAllActive_ShouldReturnOk_WhenModulesExist()
        {
            // Arrange
            var modules = new List<ModuleDTO>
            {
                new ModuleDTO { Code = "MOD0000001", Name = "Aplicaciones", IsActive = true, ApplicationCode="APP0000001", CreatedBy="System" }
            };
            _serviceMock.Setup(s => s.GetAllActiveAsync()).ReturnsAsync(modules);

            // Act
            var result = await _controller.GetAllActive();

            // Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [Test]
        public async Task GetAllActive_ShouldReturnNotFound_WhenNoModulesExist()
        {
            // Arrange
            _serviceMock.Setup(s => s.GetAllActiveAsync()).ReturnsAsync(new List<ModuleDTO>());

            // Act
            var result = await _controller.GetAllActive();

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        [Test]
        public async Task GetByCode_ShouldReturnOk_WhenModuleExists()
        {
            // Arrange
            var module = new ModuleDTO { Code = "MOD0000001", Name = "Aplicaciones", IsActive = true, ApplicationCode = "APP0000001", CreatedBy = "System" };
            _serviceMock.Setup(s => s.GetByCodeAsync("MOD0000001")).ReturnsAsync(module);

            // Act
            var result = await _controller.GetByCode("MOD0000001");

            // Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [Test]
        public async Task GetByCode_ShouldReturnNotFound_WhenModuleDoesNotExist()
        {
            // Arrange
            _serviceMock.Setup(s => s.GetByCodeAsync("MOD0000001")).ReturnsAsync((ModuleDTO)null);

            // Act
            var result = await _controller.GetByCode("MOD0000001");

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        [Test]
        public async Task Create_ShouldReturnCreatedAtAction_WhenModuleIsCreated()
        {
            // Arrange
            var module = new ModuleDTO { Code = "MOD0000001", Name = "Aplicaciones", IsActive = true, ApplicationCode = "APP0000001", CreatedBy = "System" };
            _serviceMock.Setup(s => s.CreateAsync(module)).ReturnsAsync(module);

            // Act
            var result = await _controller.Create(module);

            // Assert
            var createdAtActionResult = result as CreatedAtActionResult;
            Assert.NotNull(createdAtActionResult);
            Assert.AreEqual(201, createdAtActionResult.StatusCode);
        }

        [Test]
        public async Task Create_ShouldReturnBadRequest_WhenModuleIsNull()
        {
            // Act
            var result = await _controller.Create(null);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);

            var response = badRequestResult.Value as ResponseApi<ModuleDTO>;
            Assert.NotNull(response);
            Assert.AreEqual("Los datos del módulo no pueden ser nulos.", response.Message);
        }

        [Test]
        public async Task Update_ShouldReturnOk_WhenModuleIsUpdated()
        {
            // Arrange
            var module = new ModuleDTO { Code = "MOD0000001", Name = "Aplicaciones", IsActive = true, ApplicationCode = "APP0000001", CreatedBy = "System" };
            _serviceMock.Setup(s => s.UpdateAsync(module)).ReturnsAsync(module);

            // Act
            var result = await _controller.Update(module);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [Test]
        public async Task Update_ShouldReturnNotFound_WhenModuleDoesNotExist()
        {
            // Arrange
            var module = new ModuleDTO { Code = "MOD0000001", Name = "Aplicaciones", IsActive = true, ApplicationCode = "APP0000001", CreatedBy = "System" };
            _serviceMock.Setup(s => s.UpdateAsync(module)).ReturnsAsync((ModuleDTO)null);

            // Act
            var result = await _controller.Update(module);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        [Test]
        public async Task Delete_ShouldReturnOk_WhenModuleIsDeleted()
        {
            // Arrange
            _serviceMock.Setup(s => s.DeactivateAsync("MOD0000001")).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete("MOD0000001");

            // Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [Test]
        public async Task Delete_ShouldReturnNotFound_WhenModuleDoesNotExist()
        {
            // Arrange
            _serviceMock.Setup(s => s.DeactivateAsync("MOD0000001")).ReturnsAsync(false);

            // Act
            var result = await _controller.Delete("MOD00000010000");

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }
    }
}