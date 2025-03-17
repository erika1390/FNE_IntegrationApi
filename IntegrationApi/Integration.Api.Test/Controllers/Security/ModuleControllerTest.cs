using FluentValidation;
using FluentValidation.Results;

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
        private Mock<IValidator<ModuleDTO>> _validatorMock;
        [SetUp]
        public void SetUp()
        {
            _serviceMock = new Mock<IModuleService>();
            _loggerMock = new Mock<ILogger<ModuleController>>();
            _validatorMock = new Mock<IValidator<ModuleDTO>>();
            _controller = new ModuleController(_serviceMock.Object, _loggerMock.Object, _validatorMock.Object);
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
            var module = new ModuleDTO
            {
                Code = "MOD0000001",
                Name = "Aplicaciones",
                IsActive = true,
                ApplicationCode = "APP0000001",
                CreatedBy = "System"
            };

            var serviceMock = new Mock<IModuleService>();
            var validatorMock = new Mock<IValidator<ModuleDTO>>();
            var loggerMock = new Mock<ILogger<ModuleController>>();

            // Configurar validación exitosa
            validatorMock
                .Setup(v => v.ValidateAsync(It.IsAny<ModuleDTO>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            // Configurar el servicio mock para devolver el módulo esperado
            serviceMock
                .Setup(s => s.CreateAsync(It.IsAny<ModuleDTO>()))
                .ReturnsAsync(module);

            var controller = new ModuleController(serviceMock.Object, loggerMock.Object, validatorMock.Object);

            // Act
            var result = await controller.Create(module);

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
            var module = new ModuleDTO
            {
                Code = "MOD0000001",
                Name = "Aplicaciones",
                IsActive = true,
                ApplicationCode = "APP0000001",
                CreatedBy = "System"
            };

            var serviceMock = new Mock<IModuleService>();
            var validatorMock = new Mock<IValidator<ModuleDTO>>();
            var loggerMock = new Mock<ILogger<ModuleController>>();

            // Configurar validación exitosa
            validatorMock
                .Setup(v => v.ValidateAsync(It.IsAny<ModuleDTO>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            // Configurar el servicio para devolver el módulo actualizado
            serviceMock
                .Setup(s => s.UpdateAsync(It.IsAny<ModuleDTO>()))
                .ReturnsAsync(module);

            // Instanciar el controlador con los mocks
            var controller = new ModuleController(serviceMock.Object, loggerMock.Object, validatorMock.Object);

            // Act
            var result = await controller.Update(module);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [Test]
        public async Task Update_ShouldReturnNotFound_WhenModuleDoesNotExist()
        {
            // Arrange
            var module = new ModuleDTO
            {
                Code = "MOD0000001",
                Name = "Aplicaciones",
                IsActive = true,
                ApplicationCode = "APP0000001",
                CreatedBy = "System"
            };

            var serviceMock = new Mock<IModuleService>();
            var validatorMock = new Mock<IValidator<ModuleDTO>>();
            var loggerMock = new Mock<ILogger<ModuleController>>();

            // Configurar el validador para devolver una validación exitosa
            validatorMock
                .Setup(v => v.ValidateAsync(It.IsAny<ModuleDTO>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            // Configurar el servicio para devolver `null` (simulando que el módulo no existe)
            serviceMock
                .Setup(s => s.UpdateAsync(It.IsAny<ModuleDTO>()))
                .ReturnsAsync((ModuleDTO)null);

            // Instanciar el controlador con los mocks
            var controller = new ModuleController(serviceMock.Object, loggerMock.Object, validatorMock.Object);

            // Act
            var result = await controller.Update(module);

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