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
    public class ModuleServiceTest
    {
        private Mock<IModuleRepository> _repositoryMock;
        private Mock<IMapper> _mapperMock;
        private Mock<ILogger<ModuleService>> _loggerMock;
        private IModuleService _moduleService;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IModuleRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<ModuleService>>();
            _moduleService = new ModuleService(_repositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task CreateAsync_ShouldReturnCreatedModuleDTO()
        {
            var moduleDTO = new ModuleDTO { ModuleId = 1, Name = "TestModule", Code = "TST", ApplicationId = 1, CreatedBy = "User", IsActive = true };
            var module = new Integration.Core.Entities.Security.Module { Id = 1, Name = "TestModule", Code = "TST", ApplicationId = 1 };

            _mapperMock.Setup(m => m.Map<Integration.Core.Entities.Security.Module>(moduleDTO)).Returns(module);
            _repositoryMock.Setup(r => r.CreateAsync(module)).ReturnsAsync(module);
            _mapperMock.Setup(m => m.Map<ModuleDTO>(module)).Returns(moduleDTO);

            var result = await _moduleService.CreateAsync(moduleDTO);

            Assert.AreEqual(moduleDTO, result);
        }

        [Test]
        public async Task DeleteAsync_ShouldReturnTrue_WhenModuleIsDeleted()
        {
            int moduleId = 1;
            _repositoryMock.Setup(r => r.DeleteAsync(moduleId)).ReturnsAsync(true);

            var result = await _moduleService.DeleteAsync(moduleId);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task DeleteAsync_ShouldReturnFalse_WhenModuleIsNotFound()
        {
            int moduleId = 1;
            _repositoryMock.Setup(r => r.DeleteAsync(moduleId)).ReturnsAsync(false);

            var result = await _moduleService.DeleteAsync(moduleId);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetAllActiveAsync_ShouldReturnListOfModuleDTOs()
        {
            var modules = new List<Integration.Core.Entities.Security.Module> { new Integration.Core.Entities.Security.Module { Id = 1, Name = "TestModule", Code = "TST", ApplicationId = 1 } };
            var moduleDTOs = new List<ModuleDTO> { new ModuleDTO { ModuleId = 1, Name = "TestModule", Code = "TST", ApplicationId = 1, CreatedBy = "User", IsActive = true } };

            _repositoryMock.Setup(r => r.GetAllActiveAsync()).ReturnsAsync(modules);
            _mapperMock.Setup(m => m.Map<IEnumerable<ModuleDTO>>(modules)).Returns(moduleDTOs);

            var result = await _moduleService.GetAllActiveAsync();

            Assert.AreEqual(moduleDTOs, result);
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnModuleDTO_WhenModuleExists()
        {
            int moduleId = 1;
            var module = new Integration.Core.Entities.Security.Module { Id = moduleId, Name = "TestModule", Code = "TST", ApplicationId = 1 };
            var moduleDTO = new ModuleDTO { ModuleId = moduleId, Name = "TestModule", Code = "TST", ApplicationId = 1, CreatedBy = "User", IsActive = true };

            _repositoryMock.Setup(r => r.GetByIdAsync(moduleId)).ReturnsAsync(module);
            _mapperMock.Setup(m => m.Map<ModuleDTO>(module)).Returns(moduleDTO);

            var result = await _moduleService.GetByIdAsync(moduleId);

            Assert.AreEqual(moduleDTO, result);
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnNull_WhenModuleDoesNotExist()
        {
            int moduleId = 1;
            _repositoryMock.Setup(r => r.GetByIdAsync(moduleId)).ReturnsAsync((Integration.Core.Entities.Security.Module)null);

            var result = await _moduleService.GetByIdAsync(moduleId);

            Assert.IsNull(result);
        }

        [Test]
        public async Task UpdateAsync_ShouldReturnUpdatedModuleDTO()
        {
            var moduleDTO = new ModuleDTO { ModuleId = 1, Name = "UpdatedModule", Code = "UPD", ApplicationId = 1, CreatedBy = "User", IsActive = true };
            var module = new Integration.Core.Entities.Security.Module { Id = 1, Name = "UpdatedModule", Code = "UPD", ApplicationId = 1 };

            _mapperMock.Setup(m => m.Map<Integration.Core.Entities.Security.Module>(moduleDTO)).Returns(module);
            _repositoryMock.Setup(r => r.UpdateAsync(module)).ReturnsAsync(module);
            _mapperMock.Setup(m => m.Map<ModuleDTO>(module)).Returns(moduleDTO);

            var result = await _moduleService.UpdateAsync(moduleDTO);

            Assert.AreEqual(moduleDTO, result);
        }

        [Test]
        public async Task GetAllAsync_WithSinglePredicate_ShouldReturnFilteredModuleDTOs()
        {
            var modules = new List<Integration.Core.Entities.Security.Module> { new Integration.Core.Entities.Security.Module { Id = 1, Name = "TestModule", Code = "TST", ApplicationId = 1 } };
            var moduleDTOs = new List<ModuleDTO> { new ModuleDTO { ModuleId = 1, Name = "TestModule", Code = "TST", ApplicationId = 1, CreatedBy = "User", IsActive = true } };
            Expression<Func<ModuleDTO, bool>> filter = dto => dto.Name == "TestModule";

            _repositoryMock.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Integration.Core.Entities.Security.Module, bool>>>())).ReturnsAsync(modules);
            _mapperMock.Setup(m => m.Map<List<ModuleDTO>>(modules)).Returns(moduleDTOs);

            var result = await _moduleService.GetAllAsync(filter);

            Assert.AreEqual(moduleDTOs, result);
        }

        [Test]
        public async Task GetAllAsync_WithMultiplePredicates_ShouldReturnFilteredModuleDTOs()
        {
            var modules = new List<Integration.Core.Entities.Security.Module> { new Integration.Core.Entities.Security.Module { Id = 1, Name = "TestModule", Code = "TST", ApplicationId = 1 } };
            var moduleDTOs = new List<ModuleDTO> { new ModuleDTO { ModuleId = 1, Name = "TestModule", Code = "TST", ApplicationId = 1, CreatedBy = "User", IsActive = true } };
            var predicates = new List<Expression<Func<ModuleDTO, bool>>> { dto => dto.Name == "TestModule", dto => dto.Code == "TST" };

            _repositoryMock.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Integration.Core.Entities.Security.Module, bool>>>())).ReturnsAsync(modules);
            _mapperMock.Setup(m => m.Map<List<ModuleDTO>>(modules)).Returns(moduleDTOs);

            var result = await _moduleService.GetAllAsync(predicates);

            Assert.AreEqual(moduleDTOs, result);
        }
    }
}