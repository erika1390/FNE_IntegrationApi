using AutoMapper;
using Integration.Application.Interfaces.Security;
using Integration.Application.Services.Security;
using Integration.Infrastructure.Interfaces.Security;
using Integration.Shared.DTO.Security;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;
namespace Integration.Application.Test.Services.Security
{
    [TestFixture]
    public class RoleModuleServiceTest
    {
        private Mock<IRoleModuleRepository> _repositoryMock;
        private Mock<IMapper> _mapperMock;
        private Mock<ILogger<RoleModuleService>> _loggerMock;
        private IRoleModuleService _RoleService;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IRoleModuleRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<RoleModuleService>>();
            _RoleService = new RoleModuleService(_repositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
        }
        [Test]
        public async Task CreateAsync_ShouldReturnCreatedRoleModuleDTO()
        {
            var roleModuleDTO = new RoleModuleDTO {                
                RoleModuleId = 1,
                RoleId = 1,
                ModuleId = 1,
                PermissionId = 1,
                CreatedBy = "System",
                CreatedAt = DateTime.Now,
                IsActive = true
            };
            var roleModule = new Integration.Core.Entities.Security.RoleModule {
                Id = 1,
                RoleId = 1,
                ModuleId = 1,
                PermissionId = 1,
                CreatedBy = "System",
                CreatedAt = DateTime.Now,
                IsActive = true
            };
            _mapperMock.Setup(m => m.Map<Integration.Core.Entities.Security.RoleModule>(roleModuleDTO)).Returns(roleModule);
            _repositoryMock.Setup(r => r.CreateAsync(roleModule)).ReturnsAsync(roleModule);
            _mapperMock.Setup(m => m.Map<RoleModuleDTO>(roleModule)).Returns(roleModuleDTO);

            var result = await _RoleService.CreateAsync(roleModuleDTO);

            Assert.AreEqual(roleModuleDTO, result);
        }

        [Test]
        public async Task DeleteAsync_ShouldReturnTrue_WhenRoleModuleIsDeleted()
        {
            int roleModuleId = 1;
            _repositoryMock.Setup(r => r.DeleteAsync(roleModuleId)).ReturnsAsync(true);
            var result = await _RoleService.DeleteAsync(roleModuleId);
            Assert.IsTrue(result);
        }

        [Test]
        public async Task DeleteAsync_ShouldReturnFalse_WhenRoleModuleIsNotFound()
        {
            int roleModuleId = 1;
            _repositoryMock.Setup(r => r.DeleteAsync(roleModuleId)).ReturnsAsync(false);

            var result = await _RoleService.DeleteAsync(roleModuleId);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetAllActiveAsync_ShouldReturnListOfRoleModuleDTOs()
        {
            var roleModule = new List<Integration.Core.Entities.Security.RoleModule> { 
                new Integration.Core.Entities.Security.RoleModule {
                    Id = 1,
                    RoleId = 1,
                    ModuleId = 1,
                    PermissionId = 1,
                    CreatedBy = "System",
                    CreatedAt = DateTime.Now,
                    IsActive = true
                } 
            };
            var roleModuleDTOs = new List<RoleModuleDTO> { 
                new RoleModuleDTO {
                    RoleModuleId = 1,
                    RoleId = 1,
                    ModuleId = 1,
                    PermissionId = 1,
                    CreatedBy = "System",
                    CreatedAt = DateTime.Now,
                    IsActive = true
                } 
            };

            _repositoryMock.Setup(r => r.GetAllActiveAsync()).ReturnsAsync(roleModule);
            _mapperMock.Setup(m => m.Map<IEnumerable<RoleModuleDTO>>(roleModule)).Returns(roleModuleDTOs);

            var result = await _RoleService.GetAllActiveAsync();

            Assert.AreEqual(roleModuleDTOs, result);
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnRoleModuleDTO_WhenRoleModuleExists()
        {
            int roleModuleId = 1;
            var roleModule = new Integration.Core.Entities.Security.RoleModule {
                Id = 1,
                RoleId = 1,
                ModuleId = 1,
                PermissionId = 1,
                CreatedBy = "System",
                CreatedAt = DateTime.Now,
                IsActive = true
            };
            var roleModuleDTO = new RoleModuleDTO {
                RoleModuleId = 1,
                RoleId = 1,
                ModuleId = 1,
                PermissionId = 1,
                CreatedBy = "System",
                CreatedAt = DateTime.Now,
                IsActive = true
            };

            _repositoryMock.Setup(r => r.GetByIdAsync(roleModuleId)).ReturnsAsync(roleModule);
            _mapperMock.Setup(m => m.Map<RoleModuleDTO>(roleModule)).Returns(roleModuleDTO);

            var result = await _RoleService.GetByIdAsync(roleModuleId);

            Assert.AreEqual(roleModuleDTO, result);
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnNull_WhenRoleModuleDoesNotExist()
        {
            int roleModuleId = 1;
            _repositoryMock.Setup(r => r.GetByIdAsync(roleModuleId)).ReturnsAsync((Integration.Core.Entities.Security.RoleModule)null);

            var result = await _RoleService.GetByIdAsync(roleModuleId);

            Assert.IsNull(result);
        }

        [Test]
        public async Task UpdateAsync_ShouldReturnUpdatedRoleModuleDTO()
        {
            var roleModuleDTO = new RoleModuleDTO {
                RoleModuleId = 1,
                RoleId = 1,
                ModuleId = 1,
                PermissionId = 1,
                CreatedBy = "System",
                CreatedAt = DateTime.Now,
                IsActive = true
            };
            var roleModule = new Integration.Core.Entities.Security.RoleModule {
                Id = 1,
                RoleId = 1,
                ModuleId = 1,
                PermissionId = 1,
                CreatedBy = "System",
                CreatedAt = DateTime.Now,
                IsActive = true
            };
            _mapperMock.Setup(m => m.Map<Integration.Core.Entities.Security.RoleModule>(roleModuleDTO)).Returns(roleModule);
            _repositoryMock.Setup(r => r.UpdateAsync(roleModule)).ReturnsAsync(roleModule);
            _mapperMock.Setup(m => m.Map<RoleModuleDTO>(roleModule)).Returns(roleModuleDTO);

            var result = await _RoleService.UpdateAsync(roleModuleDTO);

            Assert.AreEqual(roleModuleDTO, result);
        }

        [Test]
        public async Task GetAllAsync_WithSinglePredicate_ShouldReturnFilteredRoleModuleDTOs()
        {
            var roleModules = new List<Integration.Core.Entities.Security.RoleModule> { 
                new Integration.Core.Entities.Security.RoleModule {
                    Id = 1,
                    RoleId = 1,
                    ModuleId = 1,
                    PermissionId = 1,
                    CreatedBy = "System",
                    CreatedAt = DateTime.Now,
                    IsActive = true
                } 
            };
            var roleModuleDTOs = new List<RoleModuleDTO> { 
                new RoleModuleDTO {
                    RoleModuleId = 1,
                    RoleId = 1,
                    ModuleId = 1,
                    PermissionId = 1,
                    CreatedBy = "System",
                    CreatedAt = DateTime.Now,
                    IsActive = true
                } 
            };
            Expression<Func<RoleModuleDTO, bool>> filter = dto => dto.RoleId == 1;

            _repositoryMock.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Integration.Core.Entities.Security.RoleModule, bool>>>())).ReturnsAsync(roleModules);
            _mapperMock.Setup(m => m.Map<List<RoleModuleDTO>>(roleModules)).Returns(roleModuleDTOs);

            var result = await _RoleService.GetAllAsync(filter);

            Assert.AreEqual(roleModuleDTOs, result);
        }

        [Test]
        public async Task GetAllAsync_WithMultiplePredicates_ShouldReturnFilteredRoleModuleDTOs()
        {
            var roleModule = new List<Integration.Core.Entities.Security.RoleModule> { 
                new Integration.Core.Entities.Security.RoleModule {
                    Id = 1,
                    RoleId = 1,
                    ModuleId = 1,
                    PermissionId = 1,
                    CreatedBy = "System",
                    CreatedAt = DateTime.Now,
                    IsActive = true
                } 
            };
            var roleModuleDTOs = new List<RoleModuleDTO> { 
                new RoleModuleDTO {
                    RoleModuleId = 1,
                    RoleId = 1,
                    ModuleId = 1,
                    PermissionId = 1,
                    CreatedBy = "System",
                    CreatedAt = DateTime.Now,
                    IsActive = true
                }
            };
            var predicates = new List<Expression<Func<RoleModuleDTO, bool>>> { dto => dto.RoleId == 1 };

            _repositoryMock.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Integration.Core.Entities.Security.RoleModule, bool>>>())).ReturnsAsync(roleModule);
            _mapperMock.Setup(m => m.Map<List<RoleModuleDTO>>(roleModule)).Returns(roleModuleDTOs);

            var result = await _RoleService.GetAllAsync(predicates);

            Assert.AreEqual(roleModuleDTOs, result);
        }
    }
}