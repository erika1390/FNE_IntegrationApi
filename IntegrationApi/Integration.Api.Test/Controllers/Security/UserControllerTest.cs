using FluentValidation;

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
    public class UserControllerTest
    {
        private Mock<IUserService> _serviceMock;
        private Mock<ILogger<UserController>> _loggerMock;
        private Mock<IValidator<UserDTO>> _validatorMock;
        private UserController _controller;

        [SetUp]
        public void SetUp()
        {
            _serviceMock = new Mock<IUserService>();
            _loggerMock = new Mock<ILogger<UserController>>();
            _validatorMock = new Mock<IValidator<UserDTO>>();
            _controller = new UserController(_serviceMock.Object, _loggerMock.Object, _validatorMock.Object);
        }

        [Test]
        public async Task GetAllActive_ShouldReturnOk_WhenUsersExist()
        {
            // Arrange
            var users = new List<UserDTO>
            {
                new UserDTO { Code = "USR0000001", FirstName = "Erika", LastName = "Pulido", IsActive = true, CreatedBy="system" },
                new UserDTO { Code = "USR0000002", FirstName = "Jane", LastName = "Smith", IsActive = true, CreatedBy="system" }
            };
            _serviceMock.Setup(s => s.GetAllActiveAsync()).ReturnsAsync(users);

            // Act
            var result = await _controller.GetAllActive(new HeaderDTO());

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var response = okResult.Value as ResponseApi<IEnumerable<UserDTO>>;
            Assert.IsNotNull(response);
            Assert.AreEqual(2, response.Data.Count());
        }

        [Test]
        public async Task GetAllActive_ShouldReturnNotFound_WhenNoUsersExist()
        {
            // Arrange
            _serviceMock.Setup(s => s.GetAllActiveAsync()).ReturnsAsync(new List<UserDTO>());

            // Act
            var result = await _controller.GetAllActive(new HeaderDTO());

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);

            var response = notFoundResult.Value as ResponseApi<IEnumerable<UserDTO>>;
            Assert.IsNotNull(response);
            Assert.AreEqual("No se encontraron usuarios.", response.Message);
        }

        [Test]
        public async Task GetByCode_ShouldReturnOk_WhenUserExists()
        {
            // Arrange
            var user = new UserDTO { Code = "USR0000001", FirstName = "Erika", LastName = "Pulido", IsActive = true, CreatedBy = "system" };
            _serviceMock.Setup(s => s.GetByCodeAsync("USR0000001")).ReturnsAsync(user);

            // Act
            var result = await _controller.GetByCode(new HeaderDTO(), "USR0000001");

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var response = okResult.Value as ResponseApi<UserDTO>;
            Assert.IsNotNull(response);
            Assert.AreEqual("USR0000001", response.Data.Code);
        }

        [Test]
        public async Task GetByCode_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            _serviceMock.Setup(s => s.GetByCodeAsync("USR0000001")).ReturnsAsync((UserDTO)null);

            // Act
            var result = await _controller.GetByCode(new HeaderDTO(), "USR0000001");

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);

            var response = notFoundResult.Value as ResponseApi<UserDTO>;
            Assert.IsNotNull(response);
            Assert.AreEqual("Usuario no encontrado.", response.Message);
        }

        [Test]
        public async Task Create_ShouldReturnCreated_WhenUserIsValid()
        {
            // Arrange
            var user = new UserDTO { Code = "USR0000001", FirstName = "Erika", LastName = "Pulido", IsActive = true, CreatedBy = "system" };
            _validatorMock.Setup(v => v.ValidateAsync(user, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());
            _serviceMock.Setup(s => s.CreateAsync(It.IsAny<HeaderDTO>(), user)).ReturnsAsync(user);

            // Act
            var result = await _controller.Create(new HeaderDTO(), user);

            // Assert
            Assert.IsInstanceOf<CreatedAtActionResult>(result);
            var createdResult = result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);

            var response = createdResult.Value as ResponseApi<UserDTO>;
            Assert.IsNotNull(response);
            Assert.AreEqual("USR0000001", response.Data.Code);
        }

        [Test]
        public async Task Create_ShouldReturnBadRequest_WhenUserIsInvalid()
        {
            // Arrange
            var user = new UserDTO { Code = "USR0000001", FirstName = "Erika", LastName = "Pulido", IsActive = true, CreatedBy = "system" };
            var validationErrors = new List<FluentValidation.Results.ValidationFailure>
            {
                new FluentValidation.Results.ValidationFailure("FirstName", "El campo FirstName es obligatorio.")
            };
            _validatorMock.Setup(v => v.ValidateAsync(user, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult(validationErrors));

            // Act
            var result = await _controller.Create(new HeaderDTO(), user);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);

            var response = badRequestResult.Value as ResponseApi<UserDTO>;
            Assert.IsNotNull(response);
        }

        [Test]
        public async Task Deactivate_ShouldReturnOk_WhenUserIsDeactivated()
        {
            // Arrange
            _serviceMock.Setup(s => s.DeactivateAsync(It.IsAny<HeaderDTO>(), "USR0000001")).ReturnsAsync(true);

            // Act
            var result = await _controller.Deactivate(new HeaderDTO(), "USR0000001");

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var response = okResult.Value as ResponseApi<bool>;
            Assert.IsNotNull(response);
        }

        [Test]
        public async Task Deactivate_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            _serviceMock.Setup(s => s.DeactivateAsync(It.IsAny<HeaderDTO>(), "USR0000001")).ReturnsAsync(false);

            // Act
            var result = await _controller.Deactivate(new HeaderDTO(), "USR0000001");

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);

            var response = notFoundResult.Value as ResponseApi<bool>;
            Assert.IsNotNull(response);
            Assert.AreEqual("Usuario no encontrado.", response.Message);
        }
    }
}