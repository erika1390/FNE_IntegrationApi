using FluentValidation;
using Integration.Api.Controllers.Security;
using Integration.Application.Interfaces.Security;
using Integration.Shared.DTO.Security;
using Integration.Shared.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using FluentValidation.Results;
namespace Integration.Api.Tests.Controllers.Security
{
    [TestFixture]
    public class ApplicationControllerTest
    {
        private Mock<IApplicationService> _serviceMock;
        private Mock<ILogger<ApplicationController>> _loggerMock;
        private Mock<IValidator<ApplicationDTO>> _validatorMock;
        private ApplicationController _controller;

        [SetUp]
        public void SetUp()
        {
            _serviceMock = new Mock<IApplicationService>();
            _loggerMock = new Mock<ILogger<ApplicationController>>();
            _validatorMock = new Mock<IValidator<ApplicationDTO>>();
            _controller = new ApplicationController(_serviceMock.Object, _loggerMock.Object, _validatorMock.Object);
        }

        [Test]
        public async Task GetAllActive_ShouldReturnOk_WhenApplicationsExist()
        {
            var applications = new List<ApplicationDTO>
            {
                new ApplicationDTO { Code = "APP0000001", Name = "Integration", IsActive = true, CreatedBy = "System" }
            };
            _serviceMock.Setup(s => s.GetAllActiveAsync()).ReturnsAsync(applications);
            var result = await _controller.GetAllActive();
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            var response = okResult.Value as ResponseApi<IEnumerable<ApplicationDTO>>;
            Assert.NotNull(response);
            Assert.IsTrue(response.State);
            Assert.IsNotEmpty(response.Data);
        }

        [Test]
        public async Task GetAllActive_ShouldReturnNotFound_WhenNoApplicationsExist()
        {
            _serviceMock.Setup(s => s.GetAllActiveAsync()).ReturnsAsync(new List<ApplicationDTO>());
            var result = await _controller.GetAllActive();
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
            var response = notFoundResult.Value as ResponseApi<IEnumerable<ApplicationDTO>>;
            Assert.NotNull(response);
            Assert.IsFalse(response.State);
            Assert.IsNull(response.Data);
        }

        [Test]
        public async Task GetByCode_ShouldReturnOk_WhenApplicationExists()
        {
            var application = new ApplicationDTO { Code = "APP0000001", Name = "Integration", IsActive = true, CreatedBy = "System" };
            _serviceMock.Setup(s => s.GetByCodeAsync("APP0000001")).ReturnsAsync(application);
            var result = await _controller.GetByCode("APP0000001");
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            var response = okResult.Value as ResponseApi<ApplicationDTO>;
            Assert.NotNull(response);
            Assert.IsTrue(response.State);
            Assert.AreEqual("APP0000001", response.Data.Code);
        }

        [Test]
        public async Task GetByCode_ShouldReturnNotFound_WhenApplicationDoesNotExist()
        {
            _serviceMock.Setup(s => s.GetByCodeAsync("APP0000001")).ReturnsAsync((ApplicationDTO)null);
            var result = await _controller.GetByCode("APP0000001");
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
            var response = notFoundResult.Value as ResponseApi<ApplicationDTO>;
            Assert.NotNull(response);
            Assert.IsFalse(response.State);
            Assert.IsNull(response.Data);
        }

        [Test]
        public async Task GetByCode_ShouldReturnBadRequest_WhenCodeIsEmpty()
        {
            var result = await _controller.GetByCode("");
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
            var response = badRequestResult.Value as ResponseApi<ApplicationDTO>;
            Assert.NotNull(response);
            Assert.IsFalse(response.State);
            Assert.AreEqual("El ApplicationCode no debe ser nulo o vacío", response.Message);
        }

        [Test]
        public async Task Create_ShouldReturnCreatedAtAction_WhenApplicationIsCreated()
        {
            var application = new ApplicationDTO { Code = "APP0000001", Name = "Integration", IsActive = true, CreatedBy = "System" };
            _validatorMock.Setup(v => v.ValidateAsync(application, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());
            _serviceMock.Setup(s => s.CreateAsync(application)).ReturnsAsync(application);
            var result = await _controller.Create(application);
            var createdAtActionResult = result as CreatedAtActionResult;
            Assert.NotNull(createdAtActionResult);
            Assert.AreEqual(201, createdAtActionResult.StatusCode);
            var response = createdAtActionResult.Value as ResponseApi<ApplicationDTO>;
            Assert.NotNull(response);
            Assert.IsTrue(response.State);
            Assert.AreEqual(application.Code, response.Data.Code);
        }

        [Test]
        public async Task Create_ShouldReturnBadRequest_WhenApplicationIsNull()
        {
            var result = await _controller.Create(null);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
            var response = badRequestResult.Value as ResponseApi<ApplicationDTO>;
            Assert.NotNull(response);
            Assert.IsFalse(response.State);
            Assert.AreEqual("Los datos de la aplicación no pueden ser nulos.", response.Message);
        }


        [Test]
        public async Task Update_ShouldReturnOk_WhenApplicationIsUpdated()
        {
            var application = new ApplicationDTO { Code = "APP0000001", Name = "Integration", IsActive = true, CreatedBy = "System" };
            _validatorMock.Setup(v => v.ValidateAsync(application, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());
            _serviceMock.Setup(s => s.UpdateAsync(application)).ReturnsAsync(application);
            var result = await _controller.Update(application);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            var response = okResult.Value as ResponseApi<ApplicationDTO>;
            Assert.NotNull(response);
            Assert.IsTrue(response.State);
            Assert.AreEqual(application.Code, response.Data.Code);
        }

        [Test]
        public async Task Update_ShouldReturnNotFound_WhenApplicationDoesNotExist()
        {
            var application = new ApplicationDTO { Code = "APP0000001", Name = "Integration", IsActive = true, CreatedBy = "System" };
            _validatorMock.Setup(v => v.ValidateAsync(application, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());
            _serviceMock.Setup(s => s.UpdateAsync(application)).ReturnsAsync((ApplicationDTO)null);
            var result = await _controller.Update(application);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
            var response = notFoundResult.Value as ResponseApi<ApplicationDTO>;
            Assert.NotNull(response);
            Assert.IsFalse(response.State);
            Assert.AreEqual("Aplicación no encontrada.", response.Message);
        }

        [Test]
        public async Task Delete_ShouldReturnOk_WhenApplicationIsDeleted()
        {
            _serviceMock.Setup(s => s.DeactivateAsync("APP0000001")).ReturnsAsync(true);
            var result = await _controller.Deactivate("APP0000001");
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            var response = okResult.Value as ResponseApi<bool>;
            Assert.NotNull(response);
            Assert.IsTrue(response.State);
            Assert.IsTrue(response.Data);
        }

        [Test]
        public async Task Delete_ShouldReturnNotFound_WhenApplicationDoesNotExist()
        {
            _serviceMock.Setup(s => s.DeactivateAsync("APP0000001")).ReturnsAsync(false);
            var result = await _controller.Deactivate("APP0000001");
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
            var response = notFoundResult.Value as ResponseApi<bool>;
            Assert.NotNull(response);
            Assert.IsFalse(response.State);
            Assert.AreEqual("Aplicación no encontrada.", response.Message);
        }

        [Test]
        public async Task Delete_ShouldReturnBadRequest_WhenCodeIsEmpty()
        {
            var result = await _controller.Deactivate("");
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
            var response = badRequestResult.Value as ResponseApi<bool>;
            Assert.NotNull(response);
            Assert.IsFalse(response.State);
            Assert.AreEqual("El Code debe ser nulo o vacío", response.Message);
        }
    }
}