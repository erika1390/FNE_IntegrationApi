﻿namespace Integration.Api.Tests.Controllers.Security
{
    [TestFixture]
    public class PermissionControllerTest
    {
        private Mock<IPermissionService> _serviceMock;
        private Mock<ILogger<PermissionController>> _loggerMock;
        private PermissionController _controller;

        [SetUp]
        public void SetUp()
        {
            _serviceMock = new Mock<IPermissionService>();
            _loggerMock = new Mock<ILogger<PermissionController>>();
            _controller = new PermissionController(_serviceMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task GetAllActive_ShouldReturnOk_WhenPermissionsExist()
        {
            // Arrange
            var permissions = new List<PermissionDTO>
            {
                new PermissionDTO { Code = "PER0000001", Name = "Consultar", IsActive = true, CreatedBy ="System"}
            };
            _serviceMock.Setup(s => s.GetAllActiveAsync()).ReturnsAsync(permissions);

            // Act
            var result = await _controller.GetAllActive();

            // Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [Test]
        public async Task GetAllActive_ShouldReturnNotFound_WhenNoPermissionsExist()
        {
            // Arrange
            _serviceMock.Setup(s => s.GetAllActiveAsync()).ReturnsAsync(new List<PermissionDTO>());

            // Act
            var result = await _controller.GetAllActive();

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        [Test]
        public async Task GetByCode_ShouldReturnOk_WhenPermissionExists()
        {
            // Arrange
            var permission = new PermissionDTO { Code = "PER0000001", Name = "Consultar", IsActive = true, CreatedBy = "System" };
            _serviceMock.Setup(s => s.GetByCodeAsync("PER0000001")).ReturnsAsync(permission);

            // Act
            var result = await _controller.GetByCode("PER0000001");

            // Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [Test]
        public async Task GetByCode_ShouldReturnNotFound_WhenPermissionDoesNotExist()
        {
            // Arrange
            _serviceMock.Setup(s => s.GetByCodeAsync("PER0000001")).ReturnsAsync((PermissionDTO)null);

            // Act
            var result = await _controller.GetByCode("PER0000001");

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        [Test]
        public async Task Create_ShouldReturnCreatedAtAction_WhenPermissionIsCreated()
        {
            // Arrange
            var permission = new PermissionDTO { Code = "PER0000001", Name = "Consultar", IsActive = true, CreatedBy = "System" };
            _serviceMock.Setup(s => s.CreateAsync(permission)).ReturnsAsync(permission);

            // Act
            var result = await _controller.Create(permission);

            // Assert
            var createdAtActionResult = result as CreatedAtActionResult;
            Assert.NotNull(createdAtActionResult);
            Assert.AreEqual(201, createdAtActionResult.StatusCode);
        }

        [Test]
        public async Task Create_ShouldReturnBadRequest_WhenPermissionIsNull()
        {
            // Act
            var result = await _controller.Create(null);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);

            var response = badRequestResult.Value as ResponseApi<PermissionDTO>;
            Assert.NotNull(response);
            Assert.AreEqual("Los datos del permiso no pueden ser nulos.", response.Message);
        }

        [Test]
        public async Task Update_ShouldReturnOk_WhenPermissionIsUpdated()
        {
            // Arrange
            var permission = new PermissionDTO { Code = "PER0000001", Name = "Consultar", IsActive = true, CreatedBy = "System" };
            _serviceMock.Setup(s => s.UpdateAsync(permission)).ReturnsAsync(permission);

            // Act
            var result = await _controller.Update(permission);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [Test]
        public async Task Update_ShouldReturnNotFound_WhenPermissionDoesNotExist()
        {
            // Arrange
            var permission = new PermissionDTO { Code = "PER0000001", Name = "Consultar", IsActive = true, CreatedBy = "System" };
            _serviceMock.Setup(s => s.UpdateAsync(permission)).ReturnsAsync((PermissionDTO)null);

            // Act
            var result = await _controller.Update(permission);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        [Test]
        public async Task Delete_ShouldReturnOk_WhenPermissionIsDeleted()
        {
            // Arrange
            _serviceMock.Setup(s => s.DeactivateAsync("PER0000001")).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete("PER0000001");

            // Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [Test]
        public async Task Delete_ShouldReturnNotFound_WhenPermissionDoesNotExist()
        {
            // Arrange
            _serviceMock.Setup(s => s.DeactivateAsync("PER0000001")).ReturnsAsync(false);

            // Act
            var result = await _controller.Delete("PER0000001");

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }
    }
}