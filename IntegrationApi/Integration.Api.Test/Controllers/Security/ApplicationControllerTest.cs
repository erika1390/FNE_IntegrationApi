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
    public class ApplicationControllerTest
    {
        private Mock<IApplicationService> _serviceMock;
        private Mock<ILogger<ApplicationController>> _loggerMock;
        private ApplicationController _controller;

        [SetUp]
        public void SetUp()
        {
            _serviceMock = new Mock<IApplicationService>();
            _loggerMock = new Mock<ILogger<ApplicationController>>();
            _controller = new ApplicationController(_serviceMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task GetAllActive_ShouldReturnOk_WhenApplicationsExist()
        {
            // Arrange
            var applications = new List<ApplicationDTO>
            {
                new ApplicationDTO { Code = "APP0000001", Name = "Integration", IsActive = true, CreatedBy = "System" }
            };
            _serviceMock.Setup(s => s.GetAllActiveAsync()).ReturnsAsync(applications);

            // Act
            var result = await _controller.GetAllActive();

            // Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [Test]
        public async Task GetAllActive_ShouldReturnNotFound_WhenNoApplicationsExist()
        {
            // Arrange
            _serviceMock.Setup(s => s.GetAllActiveAsync()).ReturnsAsync(new List<ApplicationDTO>());

            // Act
            var result = await _controller.GetAllActive();

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        [Test]
        public async Task GetByCode_ShouldReturnOk_WhenApplicationExists()
        {
            // Arrange
            var application = new ApplicationDTO { Code = "APP0000001", Name = "Integration", IsActive = true, CreatedBy = "System" };
            _serviceMock.Setup(s => s.GetByCodeAsync("APP0000001")).ReturnsAsync(application);

            // Act
            var result = await _controller.GetByCode("APP0000001");

            // Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [Test]
        public async Task GetByCode_ShouldReturnNotFound_WhenApplicationDoesNotExist()
        {
            // Arrange
            _serviceMock.Setup(s => s.GetByCodeAsync("APP0000001")).ReturnsAsync((ApplicationDTO)null);

            // Act
            var result = await _controller.GetByCode("APP0000001");

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        [Test]
        public async Task GetByCode_ShouldReturnBadRequest_WhenCodeIsEmpty()
        {
            // Act
            var result = await _controller.GetByCode("");

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
        }

        [Test]
        public async Task Create_ShouldReturnCreatedAtAction_WhenApplicationIsCreated()
        {
            // Arrange
            var application = new ApplicationDTO { Code = "APP0000001", Name = "Integration", IsActive = true, CreatedBy = "System" };
            _serviceMock.Setup(s => s.CreateAsync(application)).ReturnsAsync(application);

            // Act
            var result = await _controller.Create(application);

            // Assert
            var createdAtActionResult = result as CreatedAtActionResult;
            Assert.NotNull(createdAtActionResult);
            Assert.AreEqual(201, createdAtActionResult.StatusCode);
        }

        [Test]
        public async Task Create_ShouldReturnBadRequest_WhenApplicationIsNull()
        {
            // Act
            var result = await _controller.Create(null);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);

            var response = badRequestResult.Value as ResponseApi<ApplicationDTO>;
            Assert.NotNull(response);
            Assert.AreEqual("Los datos de la aplicación no pueden ser nulos.", response.Message);
        }


        [Test]
        public async Task Update_ShouldReturnOk_WhenApplicationIsUpdated()
        {
            // Arrange
            var application = new ApplicationDTO { Code = "APP0000001", Name = "Integration", IsActive = true, CreatedBy = "System" };
            _serviceMock.Setup(s => s.UpdateAsync(application)).ReturnsAsync(application);

            // Act
            var result = await _controller.Update(application);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [Test]
        public async Task Update_ShouldReturnNotFound_WhenApplicationDoesNotExist()
        {
            // Arrange
            var application = new ApplicationDTO { Code = "APP0000001", Name = "Integration", IsActive = true, CreatedBy = "System" };
            _serviceMock.Setup(s => s.UpdateAsync(application)).ReturnsAsync((ApplicationDTO)null);

            // Act
            var result = await _controller.Update(application);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        [Test]
        public async Task Delete_ShouldReturnOk_WhenApplicationIsDeleted()
        {
            // Arrange
            _serviceMock.Setup(s => s.DeactivateAsync("APP0000001")).ReturnsAsync(true);

            // Act
            var result = await _controller.Deactivate("APP0000001");

            // Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [Test]
        public async Task Delete_ShouldReturnNotFound_WhenApplicationDoesNotExist()
        {
            // Arrange
            _serviceMock.Setup(s => s.DeactivateAsync("APP0000001")).ReturnsAsync(false);

            // Act
            var result = await _controller.Deactivate("APP0000001");

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        [Test]
        public async Task Delete_ShouldReturnBadRequest_WhenCodeIsEmpty()
        {
            // Act
            var result = await _controller.Deactivate("");

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
        }
    }
}