using AutoMapper;

using Integration.Application.Services.Security;
using Integration.Core.Entities.Security;
using Integration.Infrastructure.Interfaces.Security;
using Integration.Shared.DTO.Header;
using Integration.Shared.DTO.Security;

using Microsoft.Extensions.Logging;

using Moq;

using System.Linq.Expressions;

namespace Integration.Application.Test.Services.Security
{
    [TestFixture]
    public class MenuServiceTest
    {
        private Mock<IMenuRepository> _menuRepositoryMock;
        private Mock<IModuleRepository> _moduleRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private Mock<ILogger<MenuService>> _loggerMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private MenuService _menuService;

        [SetUp]
        public void SetUp()
        {
            _menuRepositoryMock = new Mock<IMenuRepository>();
            _moduleRepositoryMock = new Mock<IModuleRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<MenuService>>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _menuService = new MenuService(
                _menuRepositoryMock.Object,
                _moduleRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object,
                _userRepositoryMock.Object
            );
        }

        [Test]
        public async Task CreateAsync_ShouldCreateMenu_WhenValidDataIsProvided()
        {
            // Arrange
            var header = new HeaderDTO { UserCode = "USR0000001" };
            var menuDTO = new MenuDTO { Code = "MNU0000001", Name = "Administrador", ModuleCode = "MOD0000001", CreatedBy = "USR0000001", IsActive = true };
            var user = new User { Id = 1, Code = "USR0000001", UserName = "epulido", IsActive = true, CreatedBy = "epulido", FirstName = "Erika", LastName = "Pulido" };
            var module = new Module { Id = 1, Code = "MOD0000001", Name = "Aplicaciones", IsActive = true };
            var menuEntity = new Menu { Id = 1, Code = "MNU0000001", Name = "Administrador" };
            var menu = new Menu
            {
                Code = "MNU0001",
                Name = "Gestión de Usuarios" 
            };
            _userRepositoryMock.Setup(x => x.GetByCodeAsync(header.UserCode)).ReturnsAsync(user);
            _moduleRepositoryMock.Setup(x => x.GetByCodeAsync(menuDTO.ModuleCode)).ReturnsAsync(module);
            _menuRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Menu>())).ReturnsAsync(menuEntity);
            _mapperMock.Setup(x => x.Map<Menu>(menuDTO)).Returns(menu);
            _mapperMock.Setup(x => x.Map<MenuDTO>(menuEntity)).Returns(menuDTO);

            // Act
            var result = await _menuService.CreateAsync(header, menuDTO);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(menuDTO.Code, result.Code);
            _userRepositoryMock.Verify(x => x.GetByCodeAsync(header.UserCode), Times.Once);
            _moduleRepositoryMock.Verify(x => x.GetByCodeAsync(menuDTO.ModuleCode), Times.Once);
            _menuRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<Menu>()), Times.Once);
        }

        [Test]
        public async Task DeactivateAsync_ShouldReturnTrue_WhenMenuIsDeactivated()
        {
            // Arrange
            var header = new HeaderDTO { UserCode = "USR0000001" };
            var user = new User { Id = 1, Code = "USR0000001", UserName = "epulido", IsActive = true, CreatedBy = "epulido", FirstName = "Erika", LastName = "Pulido" };

            _userRepositoryMock.Setup(x => x.GetByCodeAsync(header.UserCode)).ReturnsAsync(user);
            _menuRepositoryMock.Setup(x => x.DeactivateAsync("MNU0000001", user.UserName)).ReturnsAsync(true);

            // Act
            var result = await _menuService.DeactivateAsync(header, "MNU0000001");

            // Assert
            Assert.IsTrue(result);
            _userRepositoryMock.Verify(x => x.GetByCodeAsync(header.UserCode), Times.Once);
            _menuRepositoryMock.Verify(x => x.DeactivateAsync("MNU0000001", user.UserName), Times.Once);
        }

        [Test]
        public async Task GetAllActiveAsync_ShouldReturnMenus_WhenMenusExist()
        {
            // Arrange
            var menus = new List<Menu>
            {
                new Menu { Code = "MNU0000001", Name = "Administrador", CreatedBy = "USR0000001", IsActive = true },
                new Menu { Code = "MNU0000002", Name = "Gestionar Aplicación", CreatedBy = "USR0000001", IsActive = true }
            };
            var menuDTOs = menus.Select(m => new MenuDTO { Code = m.Code, Name = m.Name, ModuleCode= "MOD0000001", CreatedBy= "USR0000001" });

            _menuRepositoryMock.Setup(x => x.GetAllActiveAsync()).ReturnsAsync(menus);
            _mapperMock.Setup(x => x.Map<IEnumerable<MenuDTO>>(menus)).Returns(menuDTOs);

            // Act
            var result = await _menuService.GetAllActiveAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            _menuRepositoryMock.Verify(x => x.GetAllActiveAsync(), Times.Once);
        }

        [Test]
        public async Task GetByCodeAsync_ShouldReturnMenu_WhenMenuExists()
        {
            // Arrange
            var menu = new Menu { Code = "MNU0000001", Name = "Administrador", CreatedBy = "USR0000001", IsActive = true };
            var menuDTO = new MenuDTO { Code = "MNU0000001", Name = "Administrador", CreatedBy = "USR0000001", IsActive = true, ModuleCode = "MOD0000001" };

            _menuRepositoryMock.Setup(x => x.GetByCodeAsync("MNU0000001")).ReturnsAsync(menu);
            _mapperMock.Setup(x => x.Map<MenuDTO>(menu)).Returns(menuDTO);

            // Act
            var result = await _menuService.GetByCodeAsync("MNU0000001");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("MNU0000001", result.Code);
            _menuRepositoryMock.Verify(x => x.GetByCodeAsync("MNU0000001"), Times.Once);
        }

        [Test]
        public async Task GetByCodeAsync_ShouldReturnNull_WhenMenuDoesNotExist()
        {
            // Arrange
            _menuRepositoryMock.Setup(x => x.GetByCodeAsync("MNU0000001")).ReturnsAsync((Menu)null);

            // Act
            var result = await _menuService.GetByCodeAsync("MNU0000001");

            // Assert
            Assert.IsNull(result);
            _menuRepositoryMock.Verify(x => x.GetByCodeAsync("MNU0000001"), Times.Once);
        }

        [Test]
        public async Task GetByFilterAsync_ShouldReturnMenus_WhenFilterByModuleCode()
        {
            // Arrange
            var moduleCode = "MNU0000001";
            var module = new Module { Id = 1, Code = moduleCode, Name = "Configuración" };
            var menus = new List<Menu>
            {
                new Menu { Code = "MNU0000001", Name = "Administrador", CreatedBy = "USR0000001", IsActive = true },
                new Menu { Code = "MNU0000002", Name = "Gestionar Aplicación", CreatedBy = "USR0000001", IsActive = true }
            };
            var menuDTOs = menus.Select(m => new MenuDTO { Code = m.Code, Name = m.Name, ModuleCode = moduleCode, CreatedBy = "USR0000001" });

            _moduleRepositoryMock.Setup(x => x.GetByCodeAsync(moduleCode)).ReturnsAsync(module);
            _menuRepositoryMock.Setup(x => x.GetByFilterAsync(It.IsAny<Expression<Func<Menu, bool>>>()))
                .ReturnsAsync(menus);
            _mapperMock.Setup(x => x.Map<List<MenuDTO>>(menus)).Returns(menuDTOs.ToList());

            // ✅ Usa valor literal para que funcione con IsFilteringByModuleCode
            Expression<Func<MenuDTO, bool>> predicate = m => m.ModuleCode == "MNU0000001";

            // Act
            var result = await _menuService.GetByFilterAsync(predicate);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            _moduleRepositoryMock.Verify(x => x.GetByCodeAsync(moduleCode), Times.Once);
            _menuRepositoryMock.Verify(x => x.GetByFilterAsync(It.IsAny<Expression<Func<Menu, bool>>>()), Times.Once);
        }

        [Test]
        public async Task GetByFilterAsync_ShouldReturnEmptyList_WhenModuleCodeDoesNotExist()
        {
            // Arrange
            var moduleCode = "MOD0000001";

            // Configura el mock para devolver null cuando se llama con el moduleCode
            _moduleRepositoryMock.Setup(x => x.GetByCodeAsync(moduleCode)).ReturnsAsync((Module)null);

            // Define el filtro directamente con literal
            Expression<Func<MenuDTO, bool>> predicate = m => m.ModuleCode == "MOD0000001";

            // Act
            var result = await _menuService.GetByFilterAsync(predicate);

            // Assert
            Assert.IsNotNull(result); // Verifica que el resultado no sea null
            Assert.IsEmpty(result); // Verifica que el resultado sea una lista vacía
            _moduleRepositoryMock.Verify(x => x.GetByCodeAsync(moduleCode), Times.Once); // Verifica que GetByCodeAsync fue llamado una vez
            _menuRepositoryMock.Verify(x => x.GetByFilterAsync(It.IsAny<Expression<Func<Menu, bool>>>()), Times.Never); // Verifica que GetByFilterAsync no fue llamado
        }


        [Test]
        public async Task GetByFilterAsync_ShouldReturnMenus_WhenNoModuleCodeFilterIsApplied()
        {
            // Arrange
            var menus = new List<Menu>
            {
                new Menu { Code = "MNU0000001", Name = "Menu Administrador" },
                new Menu { Code = "MNU0000002", Name = "Menu Gestión" }
            };

            var menuDTOs = menus.Select(m => new MenuDTO
            {
                Code = m.Code,
                Name = m.Name,
                ModuleCode = "MOD0000001",
                CreatedBy = "USR0000001"
            });

            _menuRepositoryMock.Setup(x => x.GetByFilterAsync(It.IsAny<Expression<Func<Menu, bool>>>()))
                .ReturnsAsync(menus);

            _mapperMock.Setup(x => x.Map<List<MenuDTO>>(menus)).Returns(menuDTOs.ToList());

            Expression<Func<MenuDTO, bool>> predicate = m => m.Name.Contains("Menu");

            // Act
            var result = await _menuService.GetByFilterAsync(predicate);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            _menuRepositoryMock.Verify(x => x.GetByFilterAsync(It.IsAny<Expression<Func<Menu, bool>>>()), Times.Once);
        }


        [Test]
        public async Task GetByFilterAsync_ShouldReturnEmptyList_WhenNoMenusMatchFilter()
        {
            // Arrange
            var menus = new List<Menu>();
            _menuRepositoryMock.Setup(x => x.GetByFilterAsync(It.IsAny<Expression<Func<Menu, bool>>>()))
                .ReturnsAsync(menus);
            _mapperMock.Setup(x => x.Map<List<MenuDTO>>(menus)).Returns(new List<MenuDTO>());

            Expression<Func<MenuDTO, bool>> predicate = m => m.Name == "NonExistentMenu";

            // Act
            var result = await _menuService.GetByFilterAsync(predicate);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
            _menuRepositoryMock.Verify(x => x.GetByFilterAsync(It.IsAny<Expression<Func<Menu, bool>>>()), Times.Once);
        }

        [Test]
        public async Task GetByMultipleFiltersAsync_ShouldReturnFilteredMenus_WhenModuleCodeAndOtherFiltersAreProvided()
        {
            // Arrange
            var moduleCode = "MOD0000001";
            var module = new Module { Id = 1, Code = moduleCode, Name = "Configuración" };

            var menus = new List<Menu>
            {
                new Menu { Id = 1, Code = "MNU0000001", Name = "Configuración", ModuleId = 1, IsActive = true },
                new Menu { Id = 2, Code = "MNU0000002", Name = "Usuarios", ModuleId = 1, IsActive = true },
                new Menu { Id = 3, Code = "MNU0000003", Name = "Logs", ModuleId = 2, IsActive = true } // otro módulo
            };

            var menuDTOs = menus
                .Where(m => m.ModuleId == 1) // Solo los del módulo correcto
                .Select(m => new MenuDTO
                {
                    Code = m.Code,
                    Name = m.Name,
                    ModuleCode = moduleCode,
                    CreatedBy = "admin",
                    IsActive = true
                }).ToList();

            _moduleRepositoryMock.Setup(repo => repo.GetByCodeAsync(moduleCode)).ReturnsAsync(module);

            _menuRepositoryMock.Setup(repo => repo.GetByFilterAsync(It.IsAny<Expression<Func<Menu, bool>>>()))
                               .ReturnsAsync(menus.Where(m => m.ModuleId == 1).ToList());

            _mapperMock.Setup(mapper => mapper.Map<List<MenuDTO>>(It.IsAny<List<Menu>>()))
                       .Returns(menuDTOs);

            // Predicados
            var predicates = new List<Expression<Func<MenuDTO, bool>>>
            {
                m => m.ModuleCode == "MOD0000001",
                m => m.Name.Contains("Usu")
            };

            // Act
            var result = await _menuService.GetByMultipleFiltersAsync(predicates);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count); // solo "Usuarios"
            Assert.AreEqual("Usuarios", result.First().Name);
            _moduleRepositoryMock.Verify(x => x.GetByCodeAsync(moduleCode), Times.Once);
            _menuRepositoryMock.Verify(x => x.GetByFilterAsync(It.IsAny<Expression<Func<Menu, bool>>>()), Times.Once);
        }
        [Test]
        public async Task UpdateAsync_ShouldReturnUpdatedMenu_WhenMenuExists()
        {
            // Arrange
            var header = new HeaderDTO { UserCode = "USR0000001" };
            var menuDTO = new MenuDTO
            {
                Code = "MNU0000001",
                Name = "Dashboard",
                ModuleCode = "MOD0000001",
                UpdatedBy = "admin",
                CreatedBy = "admin",
                IsActive = true,
                CreatedAt = DateTime.UtcNow.AddMinutes(-10),
                UpdatedAt = DateTime.UtcNow
            };

            var user = new User
            {
                Id = 1,
                Code = "USR0000001",
                UserName = "epulido",
                CreatedBy = "epulido",
                FirstName = "Erika",
                LastName = "Pulido"
            };

            var module = new Module { Id = 1, Code = "MOD0000001", Name = "" };
            var existingMenu = new Menu { Id = 99, Code = "MNU0000001", Name = "Old", ModuleId = 1 };

            var updatedMenu = new Menu
            {
                Id = 99,
                Code = "MNU0000001",
                Name = "Dashboard",
                ModuleId = 1,
                UpdatedBy = "admin"
            };

            _userRepositoryMock.Setup(r => r.GetByCodeAsync(header.UserCode)).ReturnsAsync(user);
            _moduleRepositoryMock.Setup(r => r.GetByCodeAsync(menuDTO.ModuleCode)).ReturnsAsync(module);
            _menuRepositoryMock.Setup(r => r.GetByCodeAsync(menuDTO.Code)).ReturnsAsync(existingMenu);
            _mapperMock.Setup(m => m.Map<Menu>(menuDTO)).Returns(new Menu { Code = "MNU0000001", Name = "Dashboard", ModuleId = 1 });
            _menuRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Menu>())).ReturnsAsync(updatedMenu);
            _mapperMock.Setup(m => m.Map<MenuDTO>(updatedMenu)).Returns(menuDTO);

            // Act
            var result = await _menuService.UpdateAsync(header, menuDTO);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(menuDTO.Code, result.Code);
            Assert.AreEqual(menuDTO.Name, result.Name);
        }
    }
}