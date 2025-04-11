using AutoMapper;

using Integration.Application.Interfaces.Security;
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
    public class RoleMenuPermissionServiceTest
    {
        private Mock<IRoleMenuPermissionRepository> _repositoryMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IRoleRepository> _roleRepositoryMock;
        private Mock<IMenuRepository> _menuRepositoryMock;
        private Mock<IPermissionRepository> _permissionsRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private Mock<ILogger<RoleMenuPermissionService>> _loggerMock;
        private IRoleMenuPermissionService _service;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IRoleMenuPermissionRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _roleRepositoryMock = new Mock<IRoleRepository>();
            _menuRepositoryMock = new Mock<IMenuRepository>();
            _permissionsRepositoryMock = new Mock<IPermissionRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<RoleMenuPermissionService>>();

            _service = new RoleMenuPermissionService(
                _repositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object,
                _userRepositoryMock.Object,
                _roleRepositoryMock.Object,
                _menuRepositoryMock.Object,
                _permissionsRepositoryMock.Object
            );
        }

        [Test]
        public async Task CreateAsync_ShouldReturnCreatedRoleMenuPermissionsDTO()
        {
            var header = new HeaderDTO { UserCode = "USR0000001" };
            var roleMenuPermissionsDTO = new RoleMenuPermissionDTO
            {
                RoleCode = "ROL0000001",
                MenuCode = "MEN0000001",
                PermissionCode = "PER0000001",
                CreatedBy = "epulido"
            };

            var user = new User {
                Code = "USR0000001", 
                UserName = "epulido",
                CreatedBy = "epulido",
                UpdatedBy = "epulido",
                FirstName = "Erika",
                LastName = "Pulido",
            };
            var role = new Role { 
                Id = 1, 
                Code = "ROL0000001",
                CreatedBy = "epulido",
                Name = "Administrador",
            };
            var menu = new Menu { 
                Id = 1, 
                Code = "MEN0000001",
                Name = "Aplicaciones",
            };
            var permission = new Permission { 
                Id = 1,
                Code = "PER0000001" 
            };
            var roleMenuPermissions = new RoleMenuPermission { RoleId = 1, MenuId = 1, PermissionId = 1 };

            _userRepositoryMock.Setup(r => r.GetByCodeAsync(header.UserCode)).ReturnsAsync(user);
            _roleRepositoryMock.Setup(r => r.GetByCodeAsync(roleMenuPermissionsDTO.RoleCode)).ReturnsAsync(role);
            _menuRepositoryMock.Setup(r => r.GetByCodeAsync(roleMenuPermissionsDTO.MenuCode)).ReturnsAsync(menu);
            _permissionsRepositoryMock.Setup(r => r.GetByCodeAsync(roleMenuPermissionsDTO.PermissionCode)).ReturnsAsync(permission);
            _mapperMock.Setup(m => m.Map<RoleMenuPermission>(roleMenuPermissionsDTO)).Returns(roleMenuPermissions);
            _repositoryMock.Setup(r => r.CreateAsync(roleMenuPermissions)).ReturnsAsync(roleMenuPermissions);
            _mapperMock.Setup(m => m.Map<RoleMenuPermissionDTO>(roleMenuPermissions)).Returns(roleMenuPermissionsDTO);

            var result = await _service.CreateAsync(header, roleMenuPermissionsDTO);

            Assert.NotNull(result);
            Assert.AreEqual(roleMenuPermissionsDTO.RoleCode, result.RoleCode);
        }

        [Test]
        public async Task DeactivateAsync_ShouldReturnTrue_WhenRoleMenuPermissionIsSuccessfullyDeactivated()
        {
            // Arrange
            var header = new HeaderDTO { UserCode = "USR0000001" };
            var dto = new RoleMenuPermissionDTO
            {
                RoleCode = "ROL0000001",
                MenuCode = "MEN0000001",
                PermissionCode = "PER0000001",
                CreatedBy = "epulido"
            };

            var user = new User { 
                Id = 1, 
                Code = "USR0000001", 
                UserName = "epulido",
                CreatedBy = "epulido",
                UpdatedBy = "epulido",
                FirstName = "Erika",
                LastName = "Pulido"
            };
            var role = new Role { 
                Id = 1, 
                Code = "ROL0000001",
                CreatedBy = "epulido",
                Name = "Administrador"
            };
            var menu = new Menu
            {
                Id = 1,
                Code = "MEN0000001",
                Name = "Aplicaciones",
            };
            var permission = new Permission { Id = 1, Code = "PER0000001" };
            var entity = new RoleMenuPermission();

            _userRepositoryMock.Setup(x => x.GetByCodeAsync(header.UserCode)).ReturnsAsync(user);
            _roleRepositoryMock.Setup(x => x.GetByCodeAsync(dto.RoleCode)).ReturnsAsync(role);
            _menuRepositoryMock.Setup(x => x.GetByCodeAsync(dto.MenuCode)).ReturnsAsync(menu);
            _permissionsRepositoryMock.Setup(x => x.GetByCodeAsync(dto.PermissionCode)).ReturnsAsync(permission);
            _mapperMock.Setup(x => x.Map<RoleMenuPermission>(dto)).Returns(entity);
            _repositoryMock.Setup(x => x.DeactivateAsync(It.IsAny<RoleMenuPermission>())).ReturnsAsync(true);

            // Act
            var result = await _service.DeactivateAsync(header, dto);

            // Assert
            Assert.IsTrue(result);
            _repositoryMock.Verify(x => x.DeactivateAsync(It.Is<RoleMenuPermission>(
                r => r.RoleId == role.Id &&
                     r.MenuId == menu.Id &&
                     r.PermissionId == permission.Id &&
                     r.UpdatedBy == user.UserName)), Times.Once);
        }

        [Test]
        public async Task GetAllActiveAsync_ShouldReturnListOfRoleMenuPermissionsDTO()
        {
            var roleMenuPermissionsList = new List<RoleMenuPermission>
            {
                new RoleMenuPermission { RoleId = 1, MenuId = 1, PermissionId = 1, IsActive = true },
                new RoleMenuPermission { RoleId = 2, MenuId = 2, PermissionId = 2, IsActive = true }
            };

            _repositoryMock.Setup(r => r.GetAllActiveAsync()).ReturnsAsync(roleMenuPermissionsList);
            _mapperMock.Setup(m => m.Map<IEnumerable<RoleMenuPermissionDTO>>(roleMenuPermissionsList))
                .Returns(roleMenuPermissionsList.Select(r => new RoleMenuPermissionDTO {
                    RoleCode = "ROL0000001",
                    MenuCode = "MEN0000001",
                    PermissionCode = "PER0000001",
                    CreatedBy = "epulido"
                }));

            var result = await _service.GetAllActiveAsync();

            Assert.NotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task GetByFilterAsync_ShouldReturnFilteredList_ByRoleCode()
        {
            // Arrange
            var roleCode = "ROL0000001";

            Expression<Func<RoleMenuPermissionDTO, bool>> predicate = dto => dto.RoleCode == roleCode;

            var role = new Role { 
                Id = 1, 
                Code = roleCode,
                CreatedBy = "epulido",
                Name = "Administrador"
            };
            var roleMenuEntities = new List<RoleMenuPermission>
            {
                new RoleMenuPermission { RoleId = 1, MenuId = 1, PermissionId = 1 }
            };
            var roleMenuDTOs = new List<RoleMenuPermissionDTO>
            {
                new RoleMenuPermissionDTO { 
                    RoleCode = roleCode, 
                    MenuCode = "MEN0000001", 
                    PermissionCode = "PER0000001",
                    CreatedBy = "epulido"
                }
            };

            _roleRepositoryMock.Setup(r => r.GetByCodeAsync(roleCode)).ReturnsAsync(role);
            _repositoryMock.Setup(r => r.GetByFilterAsync(It.IsAny<Expression<Func<RoleMenuPermission, bool>>>()))
                .ReturnsAsync(roleMenuEntities);
            _mapperMock.Setup(m => m.Map<List<RoleMenuPermissionDTO>>(roleMenuEntities)).Returns(roleMenuDTOs);

            // Act
            var result = await _service.GetByFilterAsync(predicate);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(roleCode, result.First().RoleCode);
        }

        [Test]
        public async Task GetByMultipleFiltersAsync_ShouldReturnFilteredDTOs_WhenRoleMenuAndPermissionExist()
        {
            // Arrange
            var roleCode = "ROL0000001";
            var menuCode = "MEN0000001";
            var permissionCode = "PER0000001";

            var predicates = new List<Expression<Func<RoleMenuPermissionDTO, bool>>>
        {
            dto => dto.RoleCode == roleCode,
            dto => dto.MenuCode == menuCode,
            dto => dto.PermissionCode == permissionCode
        };

            var role = new Role { 
                Id = 1, 
                Code = roleCode,
                CreatedBy = "epulido",
                Name = "Administrador"
            };
            var menu = new Menu { 
                Id = 2, 
                Code = menuCode,
                Name = "Gestión de aplicaciones"
            };
            var permission = new Permission { Id = 3, Code = permissionCode };

            var entities = new List<RoleMenuPermission>
            {
                new RoleMenuPermission { RoleId = 1, MenuId = 2, PermissionId = 3 }
            };

            var dtos = new List<RoleMenuPermissionDTO>
            {
                new RoleMenuPermissionDTO { 
                    RoleCode = roleCode, 
                    MenuCode = menuCode, 
                    PermissionCode = permissionCode,
                    CreatedBy = "epulido"
                }
            };

            _roleRepositoryMock.Setup(r => r.GetByCodeAsync(roleCode)).ReturnsAsync(role);
            _menuRepositoryMock.Setup(r => r.GetByCodeAsync(menuCode)).ReturnsAsync(menu);
            _permissionsRepositoryMock.Setup(r => r.GetByCodeAsync(permissionCode)).ReturnsAsync(permission);
            _repositoryMock.Setup(r => r.GetByFilterAsync(It.IsAny<Expression<Func<RoleMenuPermission, bool>>>()))
                               .ReturnsAsync(entities);
            _mapperMock.Setup(m => m.Map<List<RoleMenuPermissionDTO>>(entities)).Returns(dtos);

            // Act
            var result = await _service.GetByMultipleFiltersAsync(predicates);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(roleCode, result[0].RoleCode);
            Assert.AreEqual(menuCode, result[0].MenuCode);
            Assert.AreEqual(permissionCode, result[0].PermissionCode);
        }

        [Test]
        public async Task GetByCodesAsync_ShouldReturnMappedDTO_WhenRoleMenuPermissionExists()
        {
            // Arrange
            var dtoInput = new RoleMenuPermissionDTO
            {
                RoleCode = "ROL0000001",
                MenuCode = "MNU0000002",
                PermissionCode = "PER0000001",
                CreatedBy = "epulido"
            };

            var role = new Role { 
                Id = 1, 
                Code = "ROL0000001",
                CreatedBy = "epulido",
                Name = "Administrador"
            };
            var menu = new Menu { 
                Id = 2, 
                Code = "MNU0000002",
                Name = "Gestión de aplicaciones"
            };
            var permission = new Permission { 
                Id = 3, 
                Code = "PER0000001"
            };

            var entityInput = new RoleMenuPermission(); // Intermedio
            var foundEntity = new RoleMenuPermission { RoleId = 1, MenuId = 2, PermissionId = 3 };
            var expectedDTO = new RoleMenuPermissionDTO
            {
                RoleCode = "ROL0000001",
                MenuCode = "MNU0000002",
                PermissionCode = "PER0000001",
                CreatedBy = "epulido"
            };

            _mapperMock.Setup(m => m.Map<RoleMenuPermission>(dtoInput)).Returns(entityInput);
            _roleRepositoryMock.Setup(r => r.GetByCodeAsync(dtoInput.RoleCode)).ReturnsAsync(role);
            _menuRepositoryMock.Setup(m => m.GetByCodeAsync(dtoInput.MenuCode)).ReturnsAsync(menu);
            _permissionsRepositoryMock.Setup(p => p.GetByCodeAsync(dtoInput.PermissionCode)).ReturnsAsync(permission);

            _repositoryMock.Setup(r => r.GetByRoleIdMenuIdPermissionsIdAsync(It.Is<RoleMenuPermission>(
                rm => rm.RoleId == role.Id && rm.MenuId == menu.Id && rm.PermissionId == permission.Id)))
                .ReturnsAsync(foundEntity);

            _mapperMock.Setup(m => m.Map<RoleMenuPermissionDTO>(foundEntity)).Returns(expectedDTO);

            // Act
            var result = await _service.GetByCodesAsync(dtoInput);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(dtoInput.RoleCode, result.RoleCode);
            Assert.AreEqual(dtoInput.MenuCode, result.MenuCode);
            Assert.AreEqual(dtoInput.PermissionCode, result.PermissionCode);
        }
        [Test]
        public async Task UpdateAsync_ShouldReturnUpdatedDTO_WhenSuccessful()
        {
            // Arrange
            var header = new HeaderDTO { UserCode = "USR0000001" };
            var dto = new RoleMenuPermissionDTO
            {
                RoleCode = "ROL0000001",
                MenuCode = "MNU0000002",
                PermissionCode = "PER0000001",
                CreatedBy = "epulido"
            };

            var user = new User { 
                Id = 1, 
                Code = "USR0000001", 
                UserName = "epulido",
                CreatedBy = "epulido",
                UpdatedBy = "epulido",
                FirstName = "Erika",
                LastName = "Pulido"
            };
            var role = new Role { 
                Id = 1, 
                Code = "ROL0000001",
                CreatedBy = "epulido",
                Name = "Administrador"
            };
            var menu = new Menu { 
                Id = 2, 
                Code = "MNU0000002",
                Name = "Gestión de aplicaciones"
            };
            var permission = new Permission { 
                Id = 3, 
                Code = "PER0000001" 
            };

            var mappedEntity = new RoleMenuPermission();
            var updatedEntity = new RoleMenuPermission { RoleId = 1, MenuId = 2, PermissionId = 3 };
            var expectedDTO = new RoleMenuPermissionDTO
            {
                RoleCode = "ROL0000001",
                MenuCode = "MNU0000002",
                PermissionCode = "PER0000001",
                CreatedBy = "epulido"
            };

            _userRepositoryMock.Setup(r => r.GetByCodeAsync(header.UserCode)).ReturnsAsync(user);
            _roleRepositoryMock.Setup(r => r.GetByCodeAsync(dto.RoleCode)).ReturnsAsync(role);
            _menuRepositoryMock.Setup(r => r.GetByCodeAsync(dto.MenuCode)).ReturnsAsync(menu);
            _permissionsRepositoryMock.Setup(r => r.GetByCodeAsync(dto.PermissionCode)).ReturnsAsync(permission);

            _mapperMock.Setup(m => m.Map<RoleMenuPermission>(dto)).Returns(mappedEntity);
            _repositoryMock.Setup(r => r.UpdateAsync(mappedEntity)).ReturnsAsync(updatedEntity);
            _mapperMock.Setup(m => m.Map<RoleMenuPermissionDTO>(updatedEntity)).Returns(expectedDTO);

            // Act
            var result = await _service.UpdateAsync(header, dto);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("ROL0000001", result.RoleCode);
            Assert.AreEqual("MNU0000002", result.MenuCode);
            Assert.AreEqual("PER0000001", result.PermissionCode);
        }

        [Test]
        public async Task UpdateAsync_ShouldReturnNull_WhenRepositoryReturnsNull()
        {
            // Arrange
            var header = new HeaderDTO { UserCode = "USR0000001" };
            var dto = new RoleMenuPermissionDTO
            {
                RoleCode = "ROL0000001",
                MenuCode = "MEN0000001",
                PermissionCode = "PER0000001",
                CreatedBy = "epulido"
            };

            var user = new User { 
                Id = 1, 
                UserName = "epulido",
                CreatedBy = "epulido",
                UpdatedBy = "epulido",
                FirstName = "Erika",
                LastName = "Pulido",
                Code = "USR0000001"
            };
            var role = new Role { 
                Id = 1,
                Code = "ROL0000001",
                CreatedBy = "epulido",
                Name = "Administrador"
            };
            var menu = new Menu
            {
                Id = 2,
                Code = "MEN0000001",
                Name = "Gestión de aplicaciones"
            };
            var permission = new Permission { 
                Id = 3,
                Code = "PER0000001"
            };

            _userRepositoryMock.Setup(r => r.GetByCodeAsync(header.UserCode)).ReturnsAsync(user);
            _roleRepositoryMock.Setup(r => r.GetByCodeAsync(dto.RoleCode)).ReturnsAsync(role);
            _menuRepositoryMock.Setup(r => r.GetByCodeAsync(dto.MenuCode)).ReturnsAsync(menu);
            _permissionsRepositoryMock.Setup(r => r.GetByCodeAsync(dto.PermissionCode)).ReturnsAsync(permission);

            _mapperMock.Setup(m => m.Map<RoleMenuPermission>(dto)).Returns(new RoleMenuPermission());
            _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<RoleMenuPermission>())).ReturnsAsync((RoleMenuPermission)null);

            // Act
            var result = await _service.UpdateAsync(header, dto);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void UpdateAsync_ShouldThrowException_WhenUserNotFound()
        {
            // Arrange
            var header = new HeaderDTO { UserCode = "USR0000001" };
            var dto = new RoleMenuPermissionDTO
            {
                RoleCode = "ROL0000001",
                MenuCode = "MNU0000002",
                PermissionCode = "PER0000001",
                CreatedBy = "epulido"
            };

            _userRepositoryMock.Setup(r => r.GetByCodeAsync(header.UserCode)).ReturnsAsync((User)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(header, dto));

            Assert.That(ex.Message, Is.EqualTo("Error al actualizar el RoleMenuPermission"));
            Assert.That(ex.InnerException?.Message, Does.Contain("No se encontró el usuario"));
        }
    }
}