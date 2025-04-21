using FluentValidation;

using Integration.Api.Controllers.Security;
using Integration.Application.Interfaces.Security;
using Integration.Shared.DTO.Header;
using Integration.Shared.DTO.Security;
using Integration.Shared.Response;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Moq;

using System.Linq.Expressions;

namespace Integration.Api.Test.Controllers.Security
{
    [TestFixture]
    public class UserRoleControllerTest
    {
        private Mock<IUserRoleService> _serviceMock;
        private Mock<IValidator<UserRoleDTO>> _validatorMock;
        private Mock<ILogger<UserRoleController>> _loggerMock;
        private UserRoleController _controller;

        [SetUp]
        public void Setup()
        {
            _serviceMock = new Mock<IUserRoleService>();
            _validatorMock = new Mock<IValidator<UserRoleDTO>>();
            _loggerMock = new Mock<ILogger<UserRoleController>>();
            _controller = new UserRoleController(_serviceMock.Object, _loggerMock.Object, _validatorMock.Object);
        }

        [Test]
        public async Task GetAllActive_ShouldReturnOk_WhenDataExists()
        {
            var roles = new List<UserRoleDTO> { new UserRoleDTO { UserCode = "USR0000001", RoleCode = "ROL0000001", CreatedBy  = "epulido" } };
            _serviceMock.Setup(s => s.GetAllActiveAsync()).ReturnsAsync(roles);

            var result = await _controller.GetAllActive(new HeaderDTO());
            var ok = result as OkObjectResult;

            Assert.IsNotNull(ok);
            Assert.IsInstanceOf<ResponseApi<IEnumerable<UserRoleDTO>>>(ok.Value);
        }

        [Test]
        public async Task GetByFilter_ShouldReturnOk_WhenValidFilter()
        {
            var predicate = new List<UserRoleDTO> { new UserRoleDTO { UserCode = "USR0000001", RoleCode = "ROL0000001", CreatedBy = "epulido" } };
            _serviceMock.Setup(s => s.GetByFilterAsync(It.IsAny<Expression<Func<UserRoleDTO, bool>>>())).ReturnsAsync(predicate);

            var result = await _controller.GetByFilter(new HeaderDTO(), "UserCode", "USR0000001");
            var ok = result as OkObjectResult;

            Assert.IsNotNull(ok);
        }

        [Test]
        public async Task GetByMultipleFilters_ShouldReturnOk_WhenValidFilters()
        {
            var filters = new Dictionary<string, string> { { "UserCode", "USR0000001" } };
            var data = new List<UserRoleDTO> { new UserRoleDTO { UserCode = "USR0000001", RoleCode = "ROL0000001", CreatedBy = "epulido" } };

            _serviceMock.Setup(s => s.GetByMultipleFiltersAsync(It.IsAny<List<Expression<Func<UserRoleDTO, bool>>>>())).ReturnsAsync(data);

            var result = await _controller.GetByMultipleFilters(new HeaderDTO(), filters);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetByCodes_ShouldReturnNotFound_WhenNull()
        {
            _serviceMock.Setup(s => s.GetByUserCodeRoleCodeAsync("USR0000001", "ROL0000001")).ReturnsAsync((UserRoleDTO)null);

            var result = await _controller.GetByCodes(new HeaderDTO(), new UserRoleDTO { UserCode = "USR0000001", RoleCode = "ROL0000001", CreatedBy = "epulido" });
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public async Task Create_ShouldReturnCreated_WhenValid()
        {
            var dto = new UserRoleDTO { UserCode = "USR0000001", RoleCode = "ROL0000001", CreatedBy = "epulido" };
            _validatorMock.Setup(v => v.ValidateAsync(dto, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());
            _serviceMock.Setup(s => s.CreateAsync(It.IsAny<HeaderDTO>(), dto)).ReturnsAsync(dto);

            var result = await _controller.Create(new HeaderDTO(), dto);
            Assert.IsInstanceOf<CreatedAtActionResult>(result);
        }

        [Test]
        public async Task Update_ShouldReturnOk_WhenValid()
        {
            var dto = new UserRoleDTO { UserCode = "USR0000001", RoleCode = "ROL0000001", CreatedBy = "epulido" };
            _validatorMock.Setup(v => v.ValidateAsync(dto, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());
            _serviceMock.Setup(s => s.UpdateAsync(It.IsAny<HeaderDTO>(), dto)).ReturnsAsync(dto);

            var result = await _controller.Update(new HeaderDTO(), dto);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task Deactivate_ShouldReturnOk_WhenSuccess()
        {
            var dto = new UserRoleDTO { UserCode = "USR0000001", RoleCode = "ROL0000001", CreatedBy = "epulido" };
            _serviceMock.Setup(s => s.DeactivateAsync(It.IsAny<HeaderDTO>(), dto.UserCode, dto.RoleCode)).ReturnsAsync(true);

            var result = await _controller.Deactivate(new HeaderDTO(), dto);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }
    }
}