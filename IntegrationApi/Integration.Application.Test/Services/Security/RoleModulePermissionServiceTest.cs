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
    public class RoleModulePermissionServiceTest
    {
        private Mock<IRoleModulePermissionRepository> _repositoryMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IRoleRepository> _roleRepositoryMock;
        private Mock<IModuleRepository> _moduleRepositoryMock;
        private Mock<IPermissionRepository> _permissionsRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private Mock<ILogger<RoleModulePermissionService>> _loggerMock;
        private IRoleModulePermissionService _service;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IRoleModulePermissionRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _roleRepositoryMock = new Mock<IRoleRepository>();
            _moduleRepositoryMock = new Mock<IModuleRepository>();
            _permissionsRepositoryMock = new Mock<IPermissionRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<RoleModulePermissionService>>();

            _service = new RoleModulePermissionService(
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
            var header = new HeaderDTO { UserCode = "USR0000001" };
            var roleModulePermissionsDTO = new RoleModulePermissionDTO
            {
                RoleCode = "ROL0000001",
                ModuleCode = "MOD0000001",
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
            _mapperMock.Setup(m => m.Map<RoleModulePermissionDTO>(roleModulePermissions)).Returns(roleModulePermissionsDTO);

            var result = await _service.CreateAsync(header, roleModulePermissionsDTO);

            Assert.NotNull(result);
            Assert.AreEqual(roleModulePermissionsDTO.RoleCode, result.RoleCode);
        }

        [Test]
        public async Task DeactivateAsync_ShouldReturnTrue_WhenRoleModulePermissionIsSuccessfullyDeactivated()
        {
            // Arrange
            var header = new HeaderDTO { UserCode = "USR0000001" };
            var dto = new RoleModulePermissionDTO
            {
                RoleCode = "ROL0000001",
                ModuleCode = "MOD0000001",
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
            var module = new Module { 
                Id = 1, 
                Code = "MOD0000001",
                Name = "Gestión de aplicaciones"
            };
            var permission = new Permission { Id = 1, Code = "PER0000001" };
            var entity = new RoleModulePermissions();

            _userRepositoryMock.Setup(x => x.GetByCodeAsync(header.UserCode)).ReturnsAsync(user);
            _roleRepositoryMock.Setup(x => x.GetByCodeAsync(dto.RoleCode)).ReturnsAsync(role);
            _moduleRepositoryMock.Setup(x => x.GetByCodeAsync(dto.ModuleCode)).ReturnsAsync(module);
            _permissionsRepositoryMock.Setup(x => x.GetByCodeAsync(dto.PermissionCode)).ReturnsAsync(permission);
            _mapperMock.Setup(x => x.Map<RoleModulePermissions>(dto)).Returns(entity);
            _repositoryMock.Setup(x => x.DeactivateAsync(It.IsAny<RoleModulePermissions>())).ReturnsAsync(true);

            // Act
            var result = await _service.DeactivateAsync(header, dto);

            // Assert
            Assert.IsTrue(result);
            _repositoryMock.Verify(x => x.DeactivateAsync(It.Is<RoleModulePermissions>(
                r => r.RoleId == role.Id &&
                     r.ModuleId == module.Id &&
                     r.PermissionId == permission.Id &&
                     r.UpdatedBy == user.UserName)), Times.Once);
        }

        [Test]
        public async Task GetAllActiveAsync_ShouldReturnListOfRoleModulePermissionsDTO()
        {
            var roleModulePermissionsList = new List<RoleModulePermissions>
            {
                new RoleModulePermissions { RoleId = 1, ModuleId = 1, PermissionId = 1, IsActive = true },
                new RoleModulePermissions { RoleId = 2, ModuleId = 2, PermissionId = 2, IsActive = true }
            };

            _repositoryMock.Setup(r => r.GetAllActiveAsync()).ReturnsAsync(roleModulePermissionsList);
            _mapperMock.Setup(m => m.Map<IEnumerable<RoleModulePermissionDTO>>(roleModulePermissionsList))
                .Returns(roleModulePermissionsList.Select(r => new RoleModulePermissionDTO {
                    RoleCode = "ROL0000001",
                    ModuleCode = "MOD0000001",
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

            Expression<Func<RoleModulePermissionDTO, bool>> predicate = dto => dto.RoleCode == roleCode;

            var role = new Role { 
                Id = 1, 
                Code = roleCode,
                CreatedBy = "epulido",
                Name = "Administrador"
            };
            var roleModuleEntities = new List<RoleModulePermissions>
            {
                new RoleModulePermissions { RoleId = 1, ModuleId = 1, PermissionId = 1 }
            };
            var roleModuleDTOs = new List<RoleModulePermissionDTO>
            {
                new RoleModulePermissionDTO { 
                    RoleCode = roleCode, 
                    ModuleCode = "MOD0000001", 
                    PermissionCode = "PER0000001",
                    CreatedBy = "epulido"
                }
            };

            _roleRepositoryMock.Setup(r => r.GetByCodeAsync(roleCode)).ReturnsAsync(role);
            _repositoryMock.Setup(r => r.GetByFilterAsync(It.IsAny<Expression<Func<RoleModulePermissions, bool>>>()))
                .ReturnsAsync(roleModuleEntities);
            _mapperMock.Setup(m => m.Map<List<RoleModulePermissionDTO>>(roleModuleEntities)).Returns(roleModuleDTOs);

            // Act
            var result = await _service.GetByFilterAsync(predicate);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(roleCode, result.First().RoleCode);
        }

        [Test]
        public async Task GetByMultipleFiltersAsync_ShouldReturnFilteredDTOs_WhenRoleModuleAndPermissionExist()
        {
            // Arrange
            var roleCode = "ROL0001";
            var moduleCode = "MOD0001";
            var permissionCode = "PER0001";

            var predicates = new List<Expression<Func<RoleModulePermissionDTO, bool>>>
        {
            dto => dto.RoleCode == roleCode,
            dto => dto.ModuleCode == moduleCode,
            dto => dto.PermissionCode == permissionCode
        };

            var role = new Role { 
                Id = 1, 
                Code = roleCode,
                CreatedBy = "epulido",
                Name = "Administrador"
            };
            var module = new Module { 
                Id = 2, 
                Code = moduleCode,
                Name = "Gestión de aplicaciones"
            };
            var permission = new Permission { Id = 3, Code = permissionCode };

            var entities = new List<RoleModulePermissions>
            {
                new RoleModulePermissions { RoleId = 1, ModuleId = 2, PermissionId = 3 }
            };

            var dtos = new List<RoleModulePermissionDTO>
            {
                new RoleModulePermissionDTO { 
                    RoleCode = roleCode, 
                    ModuleCode = moduleCode, 
                    PermissionCode = permissionCode,
                    CreatedBy = "epulido"
                }
            };

            _roleRepositoryMock.Setup(r => r.GetByCodeAsync(roleCode)).ReturnsAsync(role);
            _moduleRepositoryMock.Setup(r => r.GetByCodeAsync(moduleCode)).ReturnsAsync(module);
            _permissionsRepositoryMock.Setup(r => r.GetByCodeAsync(permissionCode)).ReturnsAsync(permission);
            _repositoryMock.Setup(r => r.GetByFilterAsync(It.IsAny<Expression<Func<RoleModulePermissions, bool>>>()))
                               .ReturnsAsync(entities);
            _mapperMock.Setup(m => m.Map<List<RoleModulePermissionDTO>>(entities)).Returns(dtos);

            // Act
            var result = await _service.GetByMultipleFiltersAsync(predicates);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(roleCode, result[0].RoleCode);
            Assert.AreEqual(moduleCode, result[0].ModuleCode);
            Assert.AreEqual(permissionCode, result[0].PermissionCode);
        }

        [Test]
        public async Task GetByCodesAsync_ShouldReturnMappedDTO_WhenRoleModulePermissionExists()
        {
            // Arrange
            var dtoInput = new RoleModulePermissionDTO
            {
                RoleCode = "ROL0000001",
                ModuleCode = "MOD0000001",
                PermissionCode = "PER0000001",
                CreatedBy = "epulido"
            };

            var role = new Role { 
                Id = 1, 
                Code = "ROL0000001",
                CreatedBy = "epulido",
                Name = "Administrador"
            };
            var module = new Module { 
                Id = 2, 
                Code = "MOD0000001",
                Name = "Gestión de aplicaciones"
            };
            var permission = new Permission { 
                Id = 3, 
                Code = "PER0000001"
            };

            var entityInput = new RoleModulePermissions(); // Intermedio
            var foundEntity = new RoleModulePermissions { RoleId = 1, ModuleId = 2, PermissionId = 3 };
            var expectedDTO = new RoleModulePermissionDTO
            {
                RoleCode = "ROL0000001",
                ModuleCode = "MOD0000001",
                PermissionCode = "PER0000001",
                CreatedBy = "epulido"
            };

            _mapperMock.Setup(m => m.Map<RoleModulePermissions>(dtoInput)).Returns(entityInput);
            _roleRepositoryMock.Setup(r => r.GetByCodeAsync(dtoInput.RoleCode)).ReturnsAsync(role);
            _moduleRepositoryMock.Setup(m => m.GetByCodeAsync(dtoInput.ModuleCode)).ReturnsAsync(module);
            _permissionsRepositoryMock.Setup(p => p.GetByCodeAsync(dtoInput.PermissionCode)).ReturnsAsync(permission);

            _repositoryMock.Setup(r => r.GetByRoleIdModuleIdPermissionsIdAsync(It.Is<RoleModulePermissions>(
                rm => rm.RoleId == role.Id && rm.ModuleId == module.Id && rm.PermissionId == permission.Id)))
                .ReturnsAsync(foundEntity);

            _mapperMock.Setup(m => m.Map<RoleModulePermissionDTO>(foundEntity)).Returns(expectedDTO);

            // Act
            var result = await _service.GetByCodesAsync(dtoInput);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(dtoInput.RoleCode, result.RoleCode);
            Assert.AreEqual(dtoInput.ModuleCode, result.ModuleCode);
            Assert.AreEqual(dtoInput.PermissionCode, result.PermissionCode);
        }
        [Test]
        public async Task UpdateAsync_ShouldReturnUpdatedDTO_WhenSuccessful()
        {
            // Arrange
            var header = new HeaderDTO { UserCode = "USR0000001" };
            var dto = new RoleModulePermissionDTO
            {
                RoleCode = "ROL0000001",
                ModuleCode = "MOD0000001",
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
            var module = new Module { 
                Id = 2, 
                Code = "MOD0000001",
                Name = "Gestión de aplicaciones"
            };
            var permission = new Permission { 
                Id = 3, 
                Code = "PER0000001" 
            };

            var mappedEntity = new RoleModulePermissions();
            var updatedEntity = new RoleModulePermissions { RoleId = 1, ModuleId = 2, PermissionId = 3 };
            var expectedDTO = new RoleModulePermissionDTO
            {
                RoleCode = "ROL0000001",
                ModuleCode = "MOD0000001",
                PermissionCode = "PER0000001",
                CreatedBy = "epulido"
            };

            _userRepositoryMock.Setup(r => r.GetByCodeAsync(header.UserCode)).ReturnsAsync(user);
            _roleRepositoryMock.Setup(r => r.GetByCodeAsync(dto.RoleCode)).ReturnsAsync(role);
            _moduleRepositoryMock.Setup(r => r.GetByCodeAsync(dto.ModuleCode)).ReturnsAsync(module);
            _permissionsRepositoryMock.Setup(r => r.GetByCodeAsync(dto.PermissionCode)).ReturnsAsync(permission);

            _mapperMock.Setup(m => m.Map<RoleModulePermissions>(dto)).Returns(mappedEntity);
            _repositoryMock.Setup(r => r.UpdateAsync(mappedEntity)).ReturnsAsync(updatedEntity);
            _mapperMock.Setup(m => m.Map<RoleModulePermissionDTO>(updatedEntity)).Returns(expectedDTO);

            // Act
            var result = await _service.UpdateAsync(header, dto);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("ROL0000001", result.RoleCode);
            Assert.AreEqual("MOD0000001", result.ModuleCode);
            Assert.AreEqual("PER0000001", result.PermissionCode);
        }

        [Test]
        public async Task UpdateAsync_ShouldReturnNull_WhenRepositoryReturnsNull()
        {
            // Arrange
            var header = new HeaderDTO { UserCode = "USR0000001" };
            var dto = new RoleModulePermissionDTO
            {
                RoleCode = "ROL0000001",
                ModuleCode = "MOD0000001",
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
            var module = new Module { 
                Id = 2,
                Code = "MOD0000001",
                Name = "Gestión de aplicaciones"
            };
            var permission = new Permission { 
                Id = 3,
                Code = "PER0000001"
            };

            _userRepositoryMock.Setup(r => r.GetByCodeAsync(header.UserCode)).ReturnsAsync(user);
            _roleRepositoryMock.Setup(r => r.GetByCodeAsync(dto.RoleCode)).ReturnsAsync(role);
            _moduleRepositoryMock.Setup(r => r.GetByCodeAsync(dto.ModuleCode)).ReturnsAsync(module);
            _permissionsRepositoryMock.Setup(r => r.GetByCodeAsync(dto.PermissionCode)).ReturnsAsync(permission);

            _mapperMock.Setup(m => m.Map<RoleModulePermissions>(dto)).Returns(new RoleModulePermissions());
            _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<RoleModulePermissions>())).ReturnsAsync((RoleModulePermissions)null);

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
            var dto = new RoleModulePermissionDTO
            {
                RoleCode = "ROL0000001",
                ModuleCode = "MOD0000001",
                PermissionCode = "PER0000001",
                CreatedBy = "epulido"
            };

            _userRepositoryMock.Setup(r => r.GetByCodeAsync(header.UserCode)).ReturnsAsync((User)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(header, dto));

            // Verifica el mensaje de la excepción externa
            Assert.That(ex.Message, Is.EqualTo("Error al actualizar el permiso"));

            // Verifica que la excepción interna contenga el mensaje real
            Assert.That(ex.InnerException?.Message, Does.Contain("No se encontró el usuario"));
        }

    }
}