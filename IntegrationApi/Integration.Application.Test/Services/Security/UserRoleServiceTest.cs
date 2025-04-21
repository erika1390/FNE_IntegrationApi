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
    public class UserRoleServiceTest
    {
        private Mock<IUserRoleRepository> _userRoleRepositoryMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IRoleRepository> _roleRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private Mock<ILogger<UserRoleService>> _loggerMock;
        private IUserRoleService _service;

        [SetUp]
        public void SetUp()
        {
            _userRoleRepositoryMock = new Mock<IUserRoleRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _roleRepositoryMock = new Mock<IRoleRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<UserRoleService>>();

            _service = new UserRoleService(
                _userRoleRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object,
                _userRepositoryMock.Object,
                _roleRepositoryMock.Object
            );
        }

        [Test]
        public async Task CreateAsync_ShouldReturnCreatedUserRoleDTO()
        {
            var header = new HeaderDTO { 
                UserCode = "USR0000001", 
                ApplicationCode= "APP0000001" 
            };
            var userRoleDTO = new UserRoleDTO { 
                UserCode = "USR0000001", 
                RoleCode = "ROL0000001" ,
                CreatedBy = "epulido"
            };
            var user = new User { 
                Id = 1, 
                Code = "USR0000001", 
                UserName = "epulido" ,
                CreatedBy = "epulido",
                FirstName = "Erika",
                LastName = "Pulido",
            };
            var role = new Role { 
                Id = 1, 
                Code = "ROL0000001",
                CreatedBy = "epulido",
                Name = "Administrador"
            };
            var userRole = new UserRole { 
                Id = 1, 
                UserId = 1, 
                RoleId = 1,
                CreatedBy = "epulido"
            };

            _userRepositoryMock.Setup(r => r.GetByCodeAsync(header.UserCode)).ReturnsAsync(user);
            _roleRepositoryMock.Setup(r => r.GetByCodeAsync(userRoleDTO.RoleCode)).ReturnsAsync(role);
            _mapperMock.Setup(m => m.Map<UserRole>(userRoleDTO)).Returns(userRole);
            _userRoleRepositoryMock.Setup(r => r.CreateAsync(userRole)).ReturnsAsync(userRole);
            _mapperMock.Setup(m => m.Map<UserRoleDTO>(userRole)).Returns(userRoleDTO);

            var result = await _service.CreateAsync(header, userRoleDTO);

            Assert.NotNull(result);
            Assert.AreEqual("USR0000001", result.UserCode);
            Assert.AreEqual("ROL0000001", result.RoleCode);
        }

        [Test]
        public async Task DeactivateAsync_ShouldReturnTrue()
        {
            var header = new HeaderDTO
            {
                UserCode = "USR0000001",
                ApplicationCode = "APP0000001"
            };
            var user = new User {
                Id = 1,
                Code = "USR0000001",
                UserName = "epulido",
                CreatedBy = "epulido",
                FirstName = "Erika",
                LastName = "Pulido",
            };
            var role = new Role {
                Id = 1,
                Code = "ROL0000001",
                CreatedBy = "epulido",
                Name = "Administrador"
            };

            _userRepositoryMock.Setup(r => r.GetByCodeAsync(header.UserCode)).ReturnsAsync(user);
            _roleRepositoryMock.Setup(r => r.GetByCodeAsync("ROL0000001")).ReturnsAsync(role);
            _userRoleRepositoryMock.Setup(r => r.DeactivateAsync(user.Id, role.Id, user.UserName)).ReturnsAsync(true);

            var result = await _service.DeactivateAsync(header, "USR0000001", "ROL0000001");

            Assert.IsTrue(result);
        }

        [Test]
        public async Task GetAllActiveAsync_ShouldReturnUserRoleDTOList()
        {
            var userRoles = new List<UserRole> { 
                new UserRole { 
                    Id = 1, 
                    UserId = 1, 
                    RoleId = 1, 
                    IsActive = true,
                    CreatedBy = "epulido"
                } 
            };
            var userRolesDTOs = new List<UserRoleDTO> { 
                new UserRoleDTO { 
                    UserCode = "USR0000001", 
                    RoleCode = "ROL0000001",
                    CreatedBy = "epulido"
                } 
            };

            _userRoleRepositoryMock.Setup(r => r.GetAllActiveAsync()).ReturnsAsync(userRoles);
            _mapperMock.Setup(m => m.Map<IEnumerable<UserRoleDTO>>(userRoles)).Returns(userRolesDTOs);

            var result = await _service.GetAllActiveAsync();

            Assert.NotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public async Task GetByUserCodeRoleCodeAsync_ShouldReturnUserRoleDTO()
        {
            var user = new User {
                Id = 1,
                Code = "USR0000001",
                UserName = "epulido",
                CreatedBy = "epulido",
                FirstName = "Erika",
                LastName = "Pulido",
            };
            var role = new Role {
                Id = 1,
                Code = "ROL0000001",
                CreatedBy = "epulido",
                Name = "Administrador"
            };
            var userRole = new UserRole {
                Id = 1,
                UserId = 1,
                RoleId = 1,
                IsActive = true,
                CreatedBy = "epulido"
            };
            var userRoleDTO = new UserRoleDTO {
                UserCode = "USR0000001",
                RoleCode = "ROL0000001",
                CreatedBy = "epulido"
            };

            _userRepositoryMock.Setup(r => r.GetByCodeAsync("USR0000001")).ReturnsAsync(user);
            _roleRepositoryMock.Setup(r => r.GetByCodeAsync("ROL0000001")).ReturnsAsync(role);
            _userRoleRepositoryMock.Setup(r => r.GetByUserIdRoleIdAsync(user.Id, role.Id)).ReturnsAsync(userRole);
            _mapperMock.Setup(m => m.Map<UserRoleDTO>(userRole)).Returns(userRoleDTO);

            var result = await _service.GetByUserCodeRoleCodeAsync("USR0000001", "ROL0000001");

            Assert.NotNull(result);
            Assert.AreEqual("USR0000001", result.UserCode);
            Assert.AreEqual("ROL0000001", result.RoleCode);
        }

        [Test]
        public async Task Should_Return_EmptyList_When_Role_Not_Found()
        {
            // Arrange
            Expression<Func<UserRoleDTO, bool>> predicate = u => u.RoleCode == "ROL0000001";
            _roleRepositoryMock.Setup(r => r.GetByCodeAsync("ROL0000001"))
                         .ReturnsAsync((Role)null);

            // Act
            var result = await _service.GetByFilterAsync(predicate);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task Should_Filter_InMemory_When_No_User_Or_Role_Code()
        {
            // Arrange
            var entities = new List<UserRole>
            {
                new UserRole { Id = 1, CreatedBy = "epulido" }
            };

            var dtos = new List<UserRoleDTO>
            {
                new UserRoleDTO { UserCode = "USR0000001", RoleCode = "ROL0000001", CreatedBy = "epulido" }
            };

            // ⚠️ El predicate no debe contener UserCode ni RoleCode para que el filtro se aplique en memoria
            Expression<Func<UserRoleDTO, bool>> predicate = u => u.CreatedBy == "epulido";

            _userRoleRepositoryMock.Setup(r => r.GetByFilterAsync(It.IsAny<Expression<Func<UserRole, bool>>>()))
                                   .ReturnsAsync(entities);

            _mapperMock.Setup(m => m.Map<List<UserRoleDTO>>(entities))
                       .Returns(dtos);

            // Act
            var result = await _service.GetByFilterAsync(predicate);

            // Assert
            Assert.IsNotNull(result, "El resultado no debe ser nulo.");
            Assert.AreEqual(1, result.Count, "Debe haber exactamente un resultado.");
            Assert.AreEqual("epulido", result.First().CreatedBy);
        }
        [Test]
        public async Task GetByMultipleFiltersAsync_ShouldReturnFilteredUserRoles_WhenRoleAndUserCodeProvided()
        {
            // Arrange
            var role = new Role { Id = 1, Code = "ROL0000001", Name = "Administrador", CreatedBy= "epulido" };
            var user = new User { Id = 1, Code = "USR0000001", UserName = "Pulido", CreatedBy= "epulido", FirstName="Erika",LastName= "Pulido" };

            var entities = new List<UserRole>
            {
                new UserRole { Id = 1, RoleId = 1, UserId = 1, CreatedBy = "epulido" }
            };

            var dtos = new List<UserRoleDTO>
            {
                new UserRoleDTO { UserCode = "USR0000001", RoleCode = "ROL0000001", CreatedBy = "epulido" }
            };

            _roleRepositoryMock.Setup(r => r.GetByCodeAsync("ROL0000001")).ReturnsAsync(role);
            _userRepositoryMock.Setup(r => r.GetByCodeAsync("USR0000001")).ReturnsAsync(user);
            _userRoleRepositoryMock.Setup(r => r.GetByFilterAsync(It.IsAny<Expression<Func<UserRole, bool>>>()))
                                   .ReturnsAsync(entities);
            _mapperMock.Setup(m => m.Map<List<UserRoleDTO>>(entities)).Returns(dtos);

            var predicates = new List<Expression<Func<UserRoleDTO, bool>>>
            {
                dto => dto.RoleCode == "ROL0000001",
                dto => dto.UserCode == "USR0000001"
            };

            // Act
            var result = await _service.GetByMultipleFiltersAsync(predicates);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("USR0000001", result.First().UserCode);
            Assert.AreEqual("ROL0000001", result.First().RoleCode);
        }

        [Test]
        public async Task GetByMultipleFiltersAsync_ShouldReturnEmpty_WhenRoleNotFound()
        {
            // Arrange
            _roleRepositoryMock.Setup(r => r.GetByCodeAsync("ROL0000001")).ReturnsAsync((Role)null);

            var predicates = new List<Expression<Func<UserRoleDTO, bool>>>
            {
                dto => dto.RoleCode == "ROL0000001"
            };

            // Act
            var result = await _service.GetByMultipleFiltersAsync(predicates);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetByMultipleFiltersAsync_ShouldApplyInMemoryFilter_WhenNoDbFiltersDetected()
        {
            // Arrange
            var entities = new List<UserRole>
            {
                new UserRole { Id = 1, CreatedBy = "epulido" },
                new UserRole { Id = 2, CreatedBy = "otro" } // Este no debe pasar el filtro
            };

            var dtos = new List<UserRoleDTO>
            {
                new UserRoleDTO { UserCode = "USR0000001", RoleCode = "ROL0000001", CreatedBy = "epulido" },
                new UserRoleDTO { UserCode = "USR0000002", RoleCode = "ROL0000002", CreatedBy = "otro" }
            };

            _userRoleRepositoryMock.Setup(r => r.GetByFilterAsync(It.IsAny<Expression<Func<UserRole, bool>>>()))
                                   .ReturnsAsync(entities);

            _mapperMock.Setup(m => m.Map<List<UserRoleDTO>>(entities))
                       .Returns(dtos);

            var predicates = new List<Expression<Func<UserRoleDTO, bool>>>
            {
                dto => dto.CreatedBy == "epulido"
            };

            // Act
            var result = await _service.GetByMultipleFiltersAsync(predicates);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("epulido", result.First().CreatedBy);
        }

        [Test]
        public async Task UpdateAsync_ShouldReturnUpdatedDto_WhenValidInput()
        {
            // Arrange
            var header = new HeaderDTO { UserCode = "USR0000001" };
            var userRoleDTO = new UserRoleDTO { UserCode = "USR0000001", RoleCode = "ROL0000001", CreatedBy = "epulido" };

            var userHeader = new User { Id = 1, UserName = "epulido", Code = "USR0000001", CreatedBy="system", FirstName="Erika", LastName="Pulido" };
            var userBody = new User { Id = 1, UserName = "epulido", Code = "USR0000001", CreatedBy="system", FirstName="Erika", LastName="Pulido" };
            var roleBody = new Role { Id = 1, Code = "ROL0000001", CreatedBy="system", Name= "Administrador" };
            var userRoleEntity = new UserRole { Id = 1, UserId = 1, RoleId = 1, CreatedBy = "epulido" };

            _userRepositoryMock.Setup(x => x.GetByCodeAsync("USR0000001")).ReturnsAsync(userHeader); // para header
            _userRepositoryMock.Setup(x => x.GetByCodeAsync(userRoleDTO.UserCode)).ReturnsAsync(userBody); // para body
            _roleRepositoryMock.Setup(x => x.GetByCodeAsync(userRoleDTO.RoleCode)).ReturnsAsync(roleBody);

            _userRoleRepositoryMock.Setup(x => x.GetByUserIdRoleIdAsync(1, 1)) // ✅ corregido aquí
                                   .ReturnsAsync(userRoleEntity);

            _mapperMock.Setup(x => x.Map<UserRole>(userRoleDTO)).Returns(new UserRole { CreatedBy = "epulido" });
            _userRoleRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<UserRole>())).ReturnsAsync(userRoleEntity);
            _mapperMock.Setup(x => x.Map<UserRoleDTO>(userRoleEntity)).Returns(userRoleDTO);

            // Act
            var result = await _service.UpdateAsync(header, userRoleDTO);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("USR0000001", result.UserCode);
            Assert.AreEqual("ROL0000001", result.RoleCode);
        }

        [Test]
        public void UpdateAsync_ShouldThrowException_WhenHeaderUserNotFound()
        {
            // Arrange
            var header = new HeaderDTO { UserCode = "USR0000001" };
            var userRoleDTO = new UserRoleDTO { UserCode = "USR0000001", RoleCode = "ROL0000001", CreatedBy="epulido" };

            _userRepositoryMock.Setup(x => x.GetByCodeAsync("USR0000001")).ReturnsAsync((User)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(header, userRoleDTO));
            Assert.That(ex.Message, Does.Contain("No se encontró el usuario"));
        }

        [Test]
        public async Task UpdateAsync_ShouldReturnNull_WhenUpdateFails()
        {
            // Arrange
            var header = new HeaderDTO { UserCode = "USR0000001" };
            var userRoleDTO = new UserRoleDTO { UserCode = "USR0000001", RoleCode = "ROL0000001", CreatedBy = "epulido" };
            var userHeader = new User { Id = 1, UserName = "epulido", Code = "USR0000001", CreatedBy="system", FirstName="Erika", LastName="Pulido" };
            var userBody = new User { Id = 1, UserName = "epulido", Code = "USR0000001", CreatedBy = "system", FirstName="Erika", LastName="Pulido" };
            var roleBody = new Role { Id = 1, Code = "ROL0000001", CreatedBy="epulido", Name= "Administrador" };
            var userRoleEntity = new UserRole { Id = 1, UserId = 1, RoleId = 1, CreatedBy = "epulido" };

            _userRepositoryMock.Setup(x => x.GetByCodeAsync(header.UserCode)).ReturnsAsync(userHeader);
            _userRepositoryMock.Setup(x => x.GetByCodeAsync(userRoleDTO.UserCode)).ReturnsAsync(userBody);
            _roleRepositoryMock.Setup(x => x.GetByCodeAsync(userRoleDTO.RoleCode)).ReturnsAsync(roleBody);
            _userRoleRepositoryMock.Setup(x => x.GetByUserIdRoleIdAsync(1, 1)).ReturnsAsync(userRoleEntity); // ✅ corregido

            _mapperMock.Setup(x => x.Map<UserRole>(userRoleDTO)).Returns(new UserRole { CreatedBy = "epulido" });
            _userRoleRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<UserRole>())).ReturnsAsync((UserRole)null);

            // Act
            var result = await _service.UpdateAsync(header, userRoleDTO);

            // Assert
            Assert.IsNull(result);
        }
    }
}