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
    public class MenuControllerTest
    {
        private Mock<IMenuService> _menuServiceMock;
        private Mock<ILogger<MenuController>> _loggerMock;
        private Mock<IValidator<MenuDTO>> _validatorMock;
        private MenuController _controller;

        [SetUp]
        public void SetUp()
        {
            _menuServiceMock = new Mock<IMenuService>();
            _loggerMock = new Mock<ILogger<MenuController>>();
            _validatorMock = new Mock<IValidator<MenuDTO>>();
            _controller = new MenuController(_menuServiceMock.Object, _loggerMock.Object, _validatorMock.Object);
        }

        [Test]
        public async Task GetAllActive_ShouldReturnOk_WhenMenusExist()
        {
            var menus = new List<MenuDTO>
            {
                new MenuDTO { Code = "MNU0000001", Name = "Administrador", ModuleCode="MOD0000001", CreatedBy = "USR0000001", IsActive = true },
                new MenuDTO { Code = "MNU0000002", Name = "Gestionar Aplicación", ModuleCode="MOD0000001", CreatedBy = "USR0000001", IsActive = true }
            };

            _menuServiceMock.Setup(s => s.GetAllActiveAsync()).ReturnsAsync(menus);

            var result = await _controller.GetAllActive(new HeaderDTO());

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var response = okResult.Value as ResponseApi<IEnumerable<MenuDTO>>;
            Assert.IsNotNull(response);
            Assert.AreEqual(2, response.Data.Count());
        }

        [Test]
        public async Task GetAllActive_ShouldReturnNotFound_WhenNoMenusExist()
        {
            _menuServiceMock.Setup(s => s.GetAllActiveAsync()).ReturnsAsync(new List<MenuDTO>());

            var result = await _controller.GetAllActive(new HeaderDTO());

            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);

            var response = notFoundResult.Value as ResponseApi<IEnumerable<MenuDTO>>;
            Assert.IsNotNull(response);
        }

        [Test]
        public async Task GetByCode_ShouldReturnOk_WhenMenuExists()
        {
            var menu = new MenuDTO { Code = "MNU0000001", Name = "Administrador", ModuleCode = "MOD0000001", CreatedBy = "USR0000001", IsActive = true };
            _menuServiceMock.Setup(s => s.GetByCodeAsync("MNU0000001")).ReturnsAsync(menu);

            var result = await _controller.GetByCode(new HeaderDTO(), "MNU0000001");

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var response = okResult.Value as ResponseApi<MenuDTO>;
            Assert.IsNotNull(response);
            Assert.AreEqual("MNU0000001", response.Data.Code);
        }

        [Test]
        public async Task GetByCode_ShouldReturnNotFound_WhenMenuDoesNotExist()
        {
            _menuServiceMock.Setup(s => s.GetByCodeAsync("MNU0000001")).ReturnsAsync((MenuDTO)null);

            var result = await _controller.GetByCode(new HeaderDTO(), "MNU0000001");

            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);

            var response = notFoundResult.Value as ResponseApi<MenuDTO>;
            Assert.IsNotNull(response);
        }

        [Test]
        public async Task Create_ShouldReturnCreated_WhenMenuIsValid()
        {
            var menu = new MenuDTO { Code = "MNU0000001", Name = "Administrador", ModuleCode = "MOD0000001", CreatedBy = "USR0000001", IsActive = true };

            _validatorMock.Setup(v => v.ValidateAsync(menu, default))
                          .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _menuServiceMock.Setup(s => s.CreateAsync(It.IsAny<HeaderDTO>(), menu))
                            .ReturnsAsync(menu);

            var result = await _controller.Create(new HeaderDTO(), menu);

            var createdResult = result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);

            var response = createdResult.Value as ResponseApi<MenuDTO>;
            Assert.IsNotNull(response);
            Assert.AreEqual("MNU0000001", response.Data.Code);
        }

        [Test]
        public async Task Create_ShouldReturnBadRequest_WhenMenuIsInvalid()
        {
            var menu = new MenuDTO { Code = "MNU0000001", Name = "Administrador", ModuleCode = "MOD0000001", CreatedBy = "USR0000001", IsActive = true };
            var validationErrors = new List<FluentValidation.Results.ValidationFailure>
            {
                new FluentValidation.Results.ValidationFailure("Name", "El campo Nombre es obligatorio.")
            };

            _validatorMock.Setup(v => v.ValidateAsync(menu, default))
                          .ReturnsAsync(new FluentValidation.Results.ValidationResult(validationErrors));

            var result = await _controller.Create(new HeaderDTO(), menu);

            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);

            var response = badRequestResult.Value as ResponseApi<MenuDTO>;
            Assert.IsNotNull(response);
        }

        [Test]
        public async Task GetByFilter_ShouldReturnOk_WhenFilterIsValid()
        {
            // Arrange
            var header = new HeaderDTO { UserCode = "USR0000001", ApplicationCode = "APP0000001" };
            string filterField = "Code";
            string filterValue = "MNU0000001";

            var filteredMenus = new List<MenuDTO>
            {
                new MenuDTO
                {
                    Code = "MNU0000001",
                    Name = "Dashboard",
                    ModuleCode = "MOD0000001",
                    CreatedBy = "admin",
                    IsActive = true
                }
            };

            _menuServiceMock
                .Setup(s => s.GetByFilterAsync(It.IsAny<Expression<Func<MenuDTO, bool>>>()))
                .ReturnsAsync(filteredMenus);

            // Act
            var result = await _controller.GetByFilter(header, filterField, filterValue);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var response = okResult.Value as ResponseApi<IEnumerable<MenuDTO>>;
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Data.Count());
            Assert.AreEqual("MNU0000001", response.Data.First().Code);
        }

        [Test]
        public async Task GetByMultipleFilters_ShouldReturnOk_WhenValidFilters()
        {
            var filters = new Dictionary<string, string>
            {
                { "Code", "MNU0000001" },
                { "Name", "Administrador" }
            };

            var expectedMenus = new List<MenuDTO>
            {
                new MenuDTO { Code = "MNU0000001", Name = "Administrador", ModuleCode = "MOD0000001", CreatedBy = "admin", IsActive = true }
            };

            _menuServiceMock.Setup(s =>
                s.GetByMultipleFiltersAsync(It.IsAny<List<Expression<Func<MenuDTO, bool>>>>()))
                .ReturnsAsync(expectedMenus);

            var result = await _controller.GetByMultipleFilters(new HeaderDTO(), filters);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var response = okResult.Value as ResponseApi<IEnumerable<MenuDTO>>;
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Data.Count());
            Assert.AreEqual("MNU0000001", response.Data.First().Code);
        }

        [Test]
        public async Task Update_ShouldReturnOk_WhenMenuIsValid()
        {
            // Arrange
            var header = new HeaderDTO { UserCode = "USR0000001", ApplicationCode = "APP0000001" };
            var menuDto = new MenuDTO
            {
                Code = "MNU0000001",
                Name = "Dashboard",
                ModuleCode = "MOD0000001",
                CreatedBy = "admin",
                UpdatedBy = "admin",
                CreatedAt = DateTime.UtcNow.AddMinutes(-10),
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _validatorMock.Setup(v => v.ValidateAsync(menuDto, default))
                          .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _menuServiceMock.Setup(s => s.UpdateAsync(header, menuDto))
                            .ReturnsAsync(menuDto);

            // Act
            var result = await _controller.Update(header, menuDto);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var response = okResult.Value as ResponseApi<MenuDTO>;
            Assert.IsNotNull(response);
            Assert.AreEqual("Menu actualizado exitosamente.", response.Message);
            Assert.AreEqual(menuDto.Code, response.Data.Code);
        }

        [Test]
        public async Task Deactivate_ShouldReturnOk_WhenMenuIsSuccessfullyDeleted()
        {
            // Arrange
            var header = new HeaderDTO { UserCode = "USR0000001", ApplicationCode = "APP0000001" };
            var code = "MNU0000001";

            _menuServiceMock.Setup(s => s.DeactivateAsync(header, code))
                            .ReturnsAsync(true);

            // Act
            var result = await _controller.Deactivate(header, code);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var response = okResult.Value as ResponseApi<bool>;
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Data);
            Assert.AreEqual("Menu eliminado exitosamente.", response.Message);
        }
    }
}