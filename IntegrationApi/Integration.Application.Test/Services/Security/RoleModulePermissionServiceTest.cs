using AutoMapper;

using Integration.Application.Interfaces.Security;
using Integration.Application.Services.Security;
using Integration.Core.Entities.Security;
using Integration.Infrastructure.Interfaces.Security;
using Integration.Shared.DTO.Header;
using Integration.Shared.DTO.Security;

using Microsoft.Extensions.Logging;

using Moq;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

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
    }
}