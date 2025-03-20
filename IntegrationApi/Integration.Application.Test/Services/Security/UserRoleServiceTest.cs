using AutoMapper;

using Integration.Application.Interfaces.Security;
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

            var result = await _service.DeactivateAsync(header, "USR0000001", "ROL0000001", "epulido");

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
    }
}