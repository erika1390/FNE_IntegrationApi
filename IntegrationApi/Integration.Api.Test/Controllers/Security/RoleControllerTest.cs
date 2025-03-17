using FluentValidation;
using FluentValidation.Results;

using Integration.Api.Controllers.Security;
using Integration.Application.Interfaces.Security;
using Integration.Shared.DTO.Header;
using Integration.Shared.DTO.Security;
using Integration.Shared.Response;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
namespace Integration.Api.Tests.Controllers.Security
{
    [TestFixture]
    public class RoleControllerTest
    {
        private Mock<IRoleService> _serviceMock;
        private Mock<ILogger<RoleController>> _loggerMock;
        private RoleController _controller;
        private Mock<IValidator<RoleDTO>> _validatorMock;
        [SetUp]
        public void SetUp()
        {
            _serviceMock = new Mock<IRoleService>();
            _loggerMock = new Mock<ILogger<RoleController>>();
            _validatorMock = new Mock<IValidator<RoleDTO>>();
            _controller = new RoleController(_serviceMock.Object, _loggerMock.Object,_validatorMock.Object);
        }

        [Test]
        public async Task GetAllActive_ShouldReturnOk_WhenRolesExist()
        {
            // Arrange
            var header = new HeaderDTO { ApplicationCode = "APP0000001", UserCode = "USR0000001" };
            var roles = new List<RoleDTO>
            {
                new RoleDTO { Code = "ROL0000001", Name = "System", IsActive = true, CreatedBy="System" }
            };
            _serviceMock.Setup(s => s.GetAllActiveAsync()).ReturnsAsync(roles);

            // Act
            var result = await _controller.GetAllActive(header);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [Test]
        public async Task GetAllActive_ShouldReturnNotFound_WhenNoRolesExist()
        {
            // Arrange
            var header = new HeaderDTO { ApplicationCode = "APP0000001", UserCode = "USR0000001" };
            _serviceMock.Setup(s => s.GetAllActiveAsync()).ReturnsAsync(new List<RoleDTO>());

            // Act
            var result = await _controller.GetAllActive(header);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        [Test]
        public async Task GetByCode_ShouldReturnOk_WhenRoleExists()
        {
            // Arrange
            var header = new HeaderDTO { ApplicationCode = "APP0000001", UserCode = "USR0000001" };
            var role = new RoleDTO { Code = "ROL0000001", Name = "System", IsActive = true, CreatedBy = "System" };
            _serviceMock.Setup(s => s.GetByCodeAsync("ROL0000001")).ReturnsAsync(role);

            // Act
            var result = await _controller.GetByCode(header, "ROL0000001");

            // Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [Test]
        public async Task GetByCode_ShouldReturnNotFound_WhenRoleDoesNotExist()
        {
            // Arrange
            var header = new HeaderDTO { ApplicationCode = "APP0000001", UserCode = "USR0000001" };
            _serviceMock.Setup(s => s.GetByCodeAsync("ROL0000001")).ReturnsAsync((RoleDTO)null);

            // Act
            var result = await _controller.GetByCode(header, "ROL0000001");

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        [Test]
        public async Task Create_ShouldReturnCreatedAtAction_WhenRoleIsCreated()
        {
            // Arrange
            var header = new HeaderDTO { ApplicationCode = "APP0000001", UserCode = "USR0000001" };
            var role = new RoleDTO
            {
                Code = "ROL0000001",
                Name = "System",
                IsActive = true,
                CreatedBy = "System"
            };

            var serviceMock = new Mock<IRoleService>();
            var validatorMock = new Mock<IValidator<RoleDTO>>();
            var loggerMock = new Mock<ILogger<RoleController>>();

            // Configurar validación exitosa
            validatorMock
                .Setup(v => v.ValidateAsync(It.IsAny<RoleDTO>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            // Configurar el servicio para devolver el rol creado
            serviceMock
                .Setup(s => s.CreateAsync(header, It.IsAny<RoleDTO>()))
                .ReturnsAsync(role);

            // Instanciar el controlador correctamente
            var controller = new RoleController(serviceMock.Object, loggerMock.Object, validatorMock.Object);

            // Act
            var result = await controller.Create(header, role);

            // Assert
            var createdAtActionResult = result as CreatedAtActionResult;
            Assert.NotNull(createdAtActionResult);
            Assert.AreEqual(201, createdAtActionResult.StatusCode);
        }

        [Test]
        public async Task Create_ShouldReturnBadRequest_WhenRoleIsNull()
        {
            // Act
            var header = new HeaderDTO { ApplicationCode = "APP0000001", UserCode = "USR0000001" };
            var result = await _controller.Create(header, null);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);

            var response = badRequestResult.Value as ResponseApi<RoleDTO>;
            Assert.NotNull(response);
            Assert.AreEqual("Los datos del rol no pueden ser nulos.", response.Message);
        }


        [Test]
        public async Task Update_ShouldReturnOk_WhenRoleIsUpdated()
        {
            // Arrange
            var header = new HeaderDTO { ApplicationCode = "APP0000001", UserCode = "USR0000001" };
            var role = new RoleDTO
            {
                Code = "ROL0000001",
                Name = "System",
                IsActive = true,
                CreatedBy = "System"
            };

            var serviceMock = new Mock<IRoleService>();
            var validatorMock = new Mock<IValidator<RoleDTO>>();
            var loggerMock = new Mock<ILogger<RoleController>>();

            // Configurar validación exitosa
            validatorMock
                .Setup(v => v.ValidateAsync(It.IsAny<RoleDTO>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            // Configurar el servicio para devolver el rol actualizado
            serviceMock
                .Setup(s => s.UpdateAsync(header, It.IsAny<RoleDTO>()))
                .ReturnsAsync(role);

            // Instanciar el controlador correctamente
            var controller = new RoleController(serviceMock.Object, loggerMock.Object, validatorMock.Object);

            // Act
            var result = await controller.Update(header, role);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }


        [Test]
        public async Task Update_ShouldReturnNotFound_WhenRoleDoesNotExist()
        {
            // Arrange
            var header = new HeaderDTO { ApplicationCode = "APP0000001", UserCode = "USR0000001" };
            var role = new RoleDTO
            {
                Code = "ROL0000001",
                Name = "System",
                IsActive = true,
                CreatedBy = "System"
            };

            var serviceMock = new Mock<IRoleService>();
            var validatorMock = new Mock<IValidator<RoleDTO>>();
            var loggerMock = new Mock<ILogger<RoleController>>();

            // Configurar validación exitosa
            validatorMock
                .Setup(v => v.ValidateAsync(It.IsAny<RoleDTO>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            // Configurar el servicio para devolver `null` (simulando que el rol no existe)
            serviceMock
                .Setup(s => s.UpdateAsync(header, It.IsAny<RoleDTO>()))
                .ReturnsAsync((RoleDTO)null);

            // Instanciar el controlador correctamente
            var controller = new RoleController(serviceMock.Object, loggerMock.Object, validatorMock.Object);

            // Act
            var result = await controller.Update(header, role);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        [Test]
        public async Task Delete_ShouldReturnOk_WhenRoleIsDeleted()
        {
            // Arrange
            var header = new HeaderDTO { ApplicationCode = "APP0000001", UserCode = "USR0000001" };
            _serviceMock.Setup(s => s.DeactivateAsync(header, "ROL0000001")).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(header, "ROL0000001");

            // Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [Test]
        public async Task Delete_ShouldReturnNotFound_WhenRoleDoesNotExist()
        {
            // Arrange
            var header = new HeaderDTO { ApplicationCode = "APP0000001", UserCode = "USR0000001" };
            _serviceMock.Setup(s => s.DeactivateAsync(header, "ROL0000001")).ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(header, "ROL0000001");

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }
    }
}