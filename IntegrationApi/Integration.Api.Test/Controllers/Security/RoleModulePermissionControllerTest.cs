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
    public class RoleModulePermissionControllerTest
    {
        private Mock<IRoleMenuPermissionService> _serviceMock;
        private Mock<ILogger<RoleMenuPermissionController>> _loggerMock;
        private Mock<IValidator<RoleMenuPermissionDTO>> _validatorMock;
        private RoleMenuPermissionController _controller;

        [SetUp]
        public void SetUp()
        {
            _serviceMock = new Mock<IRoleMenuPermissionService>();
            _loggerMock = new Mock<ILogger<RoleMenuPermissionController>>();
            _validatorMock = new Mock<IValidator<RoleMenuPermissionDTO>>();
            _controller = new RoleMenuPermissionController(_serviceMock.Object, _loggerMock.Object, _validatorMock.Object);
        }
        [Test]
        public async Task GetAllActive_ShouldReturnOk_WhenPermissionsExist()
        {
            // Arrange
            var data = new List<RoleMenuPermissionDTO> { 
                new() { 
                    RoleCode = "ROL0000001", 
                    MenuCode = "MOD0000001", 
                    PermissionCode = "PER0000001",
                    CreatedBy = "USR0000001"
                }
            };
            _serviceMock.Setup(s => s.GetAllActiveAsync()).ReturnsAsync(data);

            // Act
            var result = await _controller.GetAllActive(new HeaderDTO());

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var response = okResult.Value as ResponseApi<IEnumerable<RoleMenuPermissionDTO>>;
            Assert.AreEqual(1, response.Data.Count());
        }

        [Test]
        public async Task GetByCodes_ShouldReturnNotFound_WhenResultIsNull()
        {
            // Arrange
            var dto = new RoleMenuPermissionDTO {
                RoleCode = "ROL0000001",
                MenuCode = "MOD0000001",
                PermissionCode = "PER0000001",
                CreatedBy = "USR0000001"
            };
            _serviceMock.Setup(s => s.GetByCodesAsync(dto)).ReturnsAsync((RoleMenuPermissionDTO)null);

            // Act
            var result = await _controller.GetByCodes(new HeaderDTO(), dto);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public async Task Create_ShouldReturnBadRequest_WhenValidationFails()
        {
            // Arrange
            var dto = new RoleMenuPermissionDTO() {
                CreatedBy = "USR0000001",
                RoleCode = "ROL0000001",
                MenuCode = "MEN0000001",
                PermissionCode = "PER0000001",
            };
            _validatorMock.Setup(v => v.ValidateAsync(dto, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult
            {
                Errors = new List<FluentValidation.Results.ValidationFailure> { new("", "Error de validación") }
            });

            // Act
            var result = await _controller.Create(new HeaderDTO(), dto);

            // Assert
            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            var response = badRequest.Value as ResponseApi<RoleMenuPermissionDTO>;
            Assert.IsNotEmpty(response.Errors);
        }

        [Test]
        public async Task Deactivate_ShouldReturnOk_WhenDeactivationIsSuccessful()
        {
            // Arrange
            var dto = new RoleMenuPermissionDTO {
                RoleCode = "ROL0000001",
                MenuCode = "MOD0000001",
                PermissionCode = "PER0000001",
                CreatedBy = "USR0000001"
            };
            _serviceMock.Setup(s => s.DeactivateAsync(It.IsAny<HeaderDTO>(), dto)).ReturnsAsync(true);

            // Act
            var result = await _controller.Deactivate(new HeaderDTO(), dto);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var response = okResult.Value as ResponseApi<bool>;
            Assert.IsTrue(response.Data);
        }

        [Test]
        public async Task GetByFilter_ShouldReturnOk_WhenValidFilter()
        {
            // Arrange
            var filterField = "RoleCode";
            var filterValue = "ROL0001";

            var expectedData = new List<RoleMenuPermissionDTO>
            {
                new RoleMenuPermissionDTO { 
                    RoleCode = "ROL0000001", 
                    MenuCode = "MOD0000001", 
                    PermissionCode = "PER0000001",
                    CreatedBy = "USR0000001"
                }
            };

            _serviceMock.Setup(s => s.GetByFilterAsync(It.IsAny<Expression<Func<RoleMenuPermissionDTO, bool>>>()))
                        .ReturnsAsync(expectedData);

            // Act
            var result = await _controller.GetByFilter(new HeaderDTO(), filterField, filterValue);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var response = okResult.Value as ResponseApi<IEnumerable<RoleMenuPermissionDTO>>;
            Assert.AreEqual(1, response.Data.Count());
            Assert.AreEqual("ROL0000001", response.Data.First().RoleCode);
        }

        [Test]
        public async Task GetByMultipleFilters_ShouldReturnOk_WhenValidFilters()
        {
            // Arrange
            var filters = new Dictionary<string, string>
            {
                { "RoleCode", "ROL0000001" },
                { "ModuleCode", "MOD0000001" }
            };

            var expectedResult = new List<RoleMenuPermissionDTO>
            {
                new RoleMenuPermissionDTO
                {
                    RoleCode = "ROL0000001",
                    MenuCode = "MOD0000001",
                    PermissionCode = "PER0000001",
                    CreatedBy = "admin"
                }
            };

            _serviceMock.Setup(s => s.GetByMultipleFiltersAsync(It.IsAny<List<Expression<Func<RoleMenuPermissionDTO, bool>>>>()))
                        .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.GetByMultipleFilters(new HeaderDTO(), filters);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var response = okResult.Value as ResponseApi<IEnumerable<RoleMenuPermissionDTO>>;
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Data.Count());
            Assert.AreEqual("ROL0000001", response.Data.First().RoleCode);
            Assert.AreEqual("MOD0000001", response.Data.First().MenuCode);
        }

        [Test]
        public async Task Update_ShouldReturnOk_WhenValidDataIsProvided()
        {
            // Arrange
            var header = new HeaderDTO { UserCode = "USR0000001", ApplicationCode = "APP0000001" };
            var dto = new RoleMenuPermissionDTO
            {
                RoleCode = "ROL0000001",
                MenuCode = "MOD0000001",
                PermissionCode = "PER0000001",
                CreatedBy = "admin",
                UpdatedBy = "admin",
                CreatedAt = DateTime.UtcNow.AddMinutes(-10),
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _validatorMock.Setup(v => v.ValidateAsync(dto, default))
                          .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _serviceMock.Setup(s => s.UpdateAsync(header, dto))
                        .ReturnsAsync(dto);

            // Act
            var result = await _controller.Update(header, dto);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var response = okResult.Value as ResponseApi<RoleMenuPermissionDTO>;
            Assert.IsNotNull(response);
            Assert.AreEqual("RoleModulePermission actualizada correctamente.", response.Message);
            Assert.AreEqual(dto.RoleCode, response.Data.RoleCode);
        }

    }
}
