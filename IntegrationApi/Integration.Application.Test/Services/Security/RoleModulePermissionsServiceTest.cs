using AutoMapper;

using Integration.Application.Services.Security;
using Integration.Core.Entities.Security;
using Integration.Infrastructure.Interfaces.Security;
using Integration.Shared.DTO.Header;
using Integration.Shared.DTO.Security;

using Microsoft.Extensions.Logging;

using Moq;

namespace Integration.Application.Test.Services.Security
{
    [TestFixture]
    public class RoleModulePermissionsServiceTest
    {
        private Mock<IRoleModulePermissionsRepository> _repositoryMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IRoleRepository> _roleRepositoryMock;
        private Mock<IModuleRepository> _moduleRepositoryMock;
        private Mock<IPermissionRepository> _permissionsRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private Mock<ILogger<RoleModulePermissionsService>> _loggerMock;
        private RoleModulePermissionsService _service;

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<IRoleModulePermissionsRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _roleRepositoryMock = new Mock<IRoleRepository>();
            _moduleRepositoryMock = new Mock<IModuleRepository>();
            _permissionsRepositoryMock = new Mock<IPermissionRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<RoleModulePermissionsService>>();

            _service = new RoleModulePermissionsService(
                _repositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object,
                _userRepositoryMock.Object,
                _roleRepositoryMock.Object,
                _moduleRepositoryMock.Object,
                _permissionsRepositoryMock.Object
            );
        }

        [Test]
        public async Task CreateAsync_ShouldReturnCreatedRoleModulePermissionsDTO()
        {
            // Arrange
            var header = new HeaderDTO { UserCode = "USR0000001" };
            var roleModulePermissionsDTO = new RoleModulePermissionsDTO
            {
                RoleCode = "ROL0000001",
                ModuleCode = "MOD0000001",
                PermissionCode = "PER0000001",
                CreatedBy = "epulido"
            };

            var user = new User
            {
                Id = 1,
                Code = "USR0000001",
                FirstName = "Erika",
                LastName = "Pulido Moreno",
                CreatedAt = DateTime.Now,
                CreatedBy = "epulido",
                IsActive = true,
                UserName = "epulido",
                Email = "epulido@minsalud.gov.co"
            };
            var role = new Role
            {
                Id = 1,
                Code = "ROL0000001",
                Name = "Administrador",
                CreatedAt = DateTime.Now,
                CreatedBy = "epulido",
                IsActive = true
            };
            var module = new Module {
                Id = 1, 
                Code = "MOD0000001",
                Name = "Gestión de aplicaciones",
            };
            var permission = new Permission { 
                Id = 1, 
                Code = "PER0000001" 
            };
            var roleModulePermissions = new RoleModulePermissions { RoleId = 1, ModuleId = 1, PermissionId = 1 };

            _userRepositoryMock.Setup(r => r.GetByCodeAsync(header.UserCode)).ReturnsAsync(user);
            _roleRepositoryMock.Setup(r => r.GetByCodeAsync(roleModulePermissionsDTO.RoleCode)).ReturnsAsync(role);
            _moduleRepositoryMock.Setup(r => r.GetByCodeAsync(roleModulePermissionsDTO.ModuleCode)).ReturnsAsync(module);
            _permissionsRepositoryMock.Setup(r => r.GetByCodeAsync(roleModulePermissionsDTO.PermissionCode)).ReturnsAsync(permission);
            _mapperMock.Setup(m => m.Map<RoleModulePermissions>(roleModulePermissionsDTO)).Returns(roleModulePermissions);
            _repositoryMock.Setup(r => r.CreateAsync(roleModulePermissions)).ReturnsAsync(roleModulePermissions);
            _mapperMock.Setup(m => m.Map<RoleModulePermissionsDTO>(roleModulePermissions)).Returns(roleModulePermissionsDTO);

            // Act
            var result = await _service.CreateAsync(header, roleModulePermissionsDTO);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(roleModulePermissionsDTO.RoleCode, result.RoleCode);
        }

        [Test]
        public void CreateAsync_ShouldThrowException_WhenUserNotFound()
        {
            // Arrange
            var header = new HeaderDTO { UserCode = "USR0000001" };
            var roleModulePermissionsDTO = new RoleModulePermissionsDTO
            {
                RoleCode = "ROL0000001",
                ModuleCode = "MOD0000001",
                PermissionCode = "PER0000001",
                CreatedBy = "epulido"
            };

            _userRepositoryMock.Setup(r => r.GetByCodeAsync(header.UserCode)).ReturnsAsync((User)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(() => _service.CreateAsync(header, roleModulePermissionsDTO));
            Assert.AreEqual("No se encontró el usuario con código USR0000001.", ex.Message);
        }

        [Test]
        public async Task DeactivateAsync_ShouldReturnTrue()
        {
            // Arrange
            var header = new HeaderDTO { UserCode = "USR0000001" };
            var roleModulePermissionsDTO = new RoleModulePermissionsDTO
            {
                RoleCode = "ROL0000001",
                ModuleCode = "MOD0000001",
                PermissionCode = "PER0000001",
                CreatedBy = "epulido"
            };

            var user = new User
            {
                Id = 1,
                Code = "USR0000001",
                FirstName = "Erika",
                LastName = "Pulido Moreno",
                CreatedAt = DateTime.Now,
                CreatedBy = "epulido",
                IsActive = true,
                UserName = "epulido",
                Email = "epulido@minsalud.gov.co"
            };
            var roleModulePermissions = new RoleModulePermissions { RoleId = 1, ModuleId = 1, PermissionId = 1 };

            _userRepositoryMock.Setup(r => r.GetByCodeAsync(header.UserCode)).ReturnsAsync(user);
            _mapperMock.Setup(m => m.Map<RoleModulePermissions>(roleModulePermissionsDTO)).Returns(roleModulePermissions);
            _repositoryMock.Setup(r => r.DeactivateAsync(roleModulePermissions)).ReturnsAsync(true);

            // Act
            var result = await _service.DeactivateAsync(header, roleModulePermissionsDTO);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task GetAllActiveAsync_ShouldReturnListOfRoleModulePermissionsDTO()
        {
            // Arrange
            var roleModulePermissionsList = new List<RoleModulePermissions>
            {
                new RoleModulePermissions { RoleId = 1, ModuleId = 1, PermissionId = 1, IsActive = true },
                new RoleModulePermissions { RoleId = 2, ModuleId = 2, PermissionId = 2, IsActive = true }
            };

            _repositoryMock.Setup(r => r.GetAllActiveAsync()).ReturnsAsync(roleModulePermissionsList);
            _mapperMock.Setup(m => m.Map<IEnumerable<RoleModulePermissionsDTO>>(roleModulePermissionsList))
                .Returns(roleModulePermissionsList.Select(r => new RoleModulePermissionsDTO { RoleCode = "ROL0000001", CreatedBy = "epulido" }));

            // Act
            var result = await _service.GetAllActiveAsync();

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(2, result.Count());
        }
    }
}