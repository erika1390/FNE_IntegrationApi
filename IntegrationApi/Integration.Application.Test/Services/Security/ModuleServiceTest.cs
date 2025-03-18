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
    public class ModuleServiceTest
    {
        private Mock<IModuleRepository> _repositoryMock;
        private Mock<IApplicationRepository> _applicationRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private Mock<ILogger<ModuleService>> _loggerMock;
        private IModuleService _moduleService;
        private Mock<IUserRepository> _userRepositoryMock;
        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IModuleRepository>();
            _applicationRepositoryMock = new Mock<IApplicationRepository>(); // Nueva dependencia
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<ModuleService>>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _moduleService = new ModuleService(
                _repositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object,
                _applicationRepositoryMock.Object, 
                _userRepositoryMock.Object
            );
        }

        [Test]
        public async Task CreateAsync_ShouldReturnCreatedModuleDTO()
        {
            var header = new HeaderDTO { ApplicationCode = "APP0000001", UserCode = "USR0000001" };
            var moduleDTO = new ModuleDTO
            {
                Name = "Aplicaciones",
                Code = "MOD0000001",
                CreatedBy = "System",
                IsActive = true
            };

            var application = new Integration.Core.Entities.Security.Application { Id = 1, Code = "APP0000001", Name = "Integration" };
            var module = new Integration.Core.Entities.Security.Module
            {
                Id = 1,
                Name = "Aplicaciones",
                Code = "MOD0000001",
                ApplicationId = 1
            };

            var user = new User
            {
                Code = "USR0000001",
                FirstName = "Erika",
                LastName = "Pulido Moreno",
                CreatedAt = DateTime.Now,
                CreatedBy = "System",
                IsActive = true,
                UserName = "epulido",
                Email = "epulido@minsalud.gov.co"
            };

            // ✅ Mockear la consulta del usuario para que devuelva un usuario válido
            _userRepositoryMock.Setup(u => u.GetByCodeAsync(header.UserCode))
                               .ReturnsAsync(user);

            _applicationRepositoryMock.Setup(a => a.GetByCodeAsync(header.ApplicationCode))
                                      .ReturnsAsync(application);

            _mapperMock.Setup(m => m.Map<Integration.Core.Entities.Security.Module>(moduleDTO))
                       .Returns(module);

            _repositoryMock.Setup(r => r.CreateAsync(module))
                           .ReturnsAsync(module);

            _mapperMock.Setup(m => m.Map<ModuleDTO>(module))
                       .Returns(moduleDTO);

            var result = await _moduleService.CreateAsync(header, moduleDTO);

            Assert.AreEqual(moduleDTO, result);
        }

        [Test]
        public async Task DeactivateAsync_ShouldReturnTrue_WhenModuleIsDeleted()
        {
            // Arrange
            var header = new HeaderDTO { ApplicationCode = "APP0000001", UserCode = "USR0000001" };
            string moduleCode = "MOD0000001";

            // ✅ Simular un usuario válido en el repositorio
            var user = new User
            {
                Code = "USR0000001",
                FirstName = "Erika",
                LastName = "Pulido Moreno",
                CreatedAt = DateTime.Now,
                CreatedBy = "System",
                IsActive = true,
                UserName = "epulido",
                Email = "epulido@minsalud.gov.co"
            };
            _userRepositoryMock.Setup(r => r.GetByCodeAsync(header.UserCode)).ReturnsAsync(user);

            // ✅ Simular que el módulo fue desactivado con éxito
            _repositoryMock.Setup(r => r.DeactivateAsync(moduleCode, user.UserName)).ReturnsAsync(true);

            // Act
            var result = await _moduleService.DeactivateAsync(header, moduleCode);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task DeactivateAsync_ShouldReturnFalse_WhenModuleIsNotFound()
        {
            // Arrange
            var header = new HeaderDTO { ApplicationCode = "APP0000001", UserCode = "USR0000001" };
            string moduleCode = "MOD0000001";

            // ✅ Simular un usuario válido en el repositorio
            var user = new User
            {
                Code = "USR0000001",
                FirstName = "Erika",
                LastName = "Pulido Moreno",
                CreatedAt = DateTime.Now,
                CreatedBy = "System",
                IsActive = true,
                UserName = "epulido",
                Email = "epulido@minsalud.gov.co"
            };
            _userRepositoryMock.Setup(r => r.GetByCodeAsync(header.UserCode)).ReturnsAsync(user);

            // ✅ Simular que el módulo no fue encontrado
            _repositoryMock.Setup(r => r.DeactivateAsync(moduleCode, user.UserName)).ReturnsAsync(false);

            // Act
            var result = await _moduleService.DeactivateAsync(header, moduleCode);

            // Assert
            Assert.IsFalse(result);
        }


        [Test]
        public async Task GetAllActiveAsync_ShouldReturnListOfModuleDTOs()
        {
            var modules = new List<Integration.Core.Entities.Security.Module>
            {
                new Integration.Core.Entities.Security.Module { Id = 1, Name = "Aplicaciones", Code = "MOD0000001", ApplicationId = 1 }
            };

            var moduleDTOs = new List<ModuleDTO>
            {
                new ModuleDTO { Name = "Aplicaciones", Code = "MOD0000001", CreatedBy = "System", IsActive = true }
            };

            _repositoryMock.Setup(r => r.GetAllActiveAsync()).ReturnsAsync(modules);
            _mapperMock.Setup(m => m.Map<IEnumerable<ModuleDTO>>(modules)).Returns(moduleDTOs);

            var result = await _moduleService.GetAllActiveAsync();

            Assert.AreEqual(moduleDTOs, result);
        }
        [Test]
        public async Task GetByCodeAsync_ShouldReturnModuleDTO_WhenApplicationExists()
        {
            var module = new Integration.Core.Entities.Security.Module { Id = 1, Name = "Aplicaciones", Code = "MOD0000001" };
            var moduleDTO = new ModuleDTO { Name = "Aplicaciones", Code = "MOD0000001", CreatedBy = "System", IsActive = true};

            _repositoryMock.Setup(r => r.GetByCodeAsync("MOD0000001")).ReturnsAsync(module);
            _mapperMock.Setup(m => m.Map<ModuleDTO>(module)).Returns(moduleDTO);

            var result = await _moduleService.GetByCodeAsync("MOD0000001");

            Assert.AreEqual(moduleDTO, result);
        }

        [Test]
        public async Task GetByCodeAsync_ShouldReturnNull_WhenModuleDoesNotExist()
        {
            _repositoryMock.Setup(r => r.GetByCodeAsync("MOD0000001")).ReturnsAsync((Integration.Core.Entities.Security.Module)null);

            var result = await _moduleService.GetByCodeAsync("MOD0000001");

            Assert.IsNull(result);
        }
        [Test]
        public async Task UpdateAsync_ShouldReturnUpdatedModuleDTO()
        {
            // Arrange
            var header = new HeaderDTO { ApplicationCode = "APP0000001", UserCode = "USR0000001" };
            var moduleDTO = new ModuleDTO
            {
                Name = "Aplicaciones",
                Code = "MOD0000001",
                CreatedBy = "System",
                IsActive = true
            };

            var application = new Integration.Core.Entities.Security.Application { Id = 1, Code = "APP0000001", Name = "Integration" };
            var moduleExist = new Integration.Core.Entities.Security.Module { Id = 1, Code = "MOD0000001", ApplicationId = 1, Name = "Aplicaciones" };

            // ✅ Simular un usuario válido en el repositorio
            var user = new User
            {
                Code = "USR0000001",
                FirstName = "Erika",
                LastName = "Pulido Moreno",
                CreatedAt = DateTime.Now,
                CreatedBy = "System",
                IsActive = true,
                UserName = "epulido",
                Email = "epulido@minsalud.gov.co"
            };
            _userRepositoryMock.Setup(r => r.GetByCodeAsync(header.UserCode)).ReturnsAsync(user);

            _applicationRepositoryMock.Setup(a => a.GetByCodeAsync(header.ApplicationCode))
                                      .ReturnsAsync(application);

            _repositoryMock.Setup(r => r.GetByCodeAsync(moduleDTO.Code))
                           .ReturnsAsync(moduleExist);

            var module = new Integration.Core.Entities.Security.Module
            {
                Id = 1,
                Code = "UPD",
                Name = "Aplicaciones",
                ApplicationId = application.Id
            };

            _mapperMock.Setup(m => m.Map<Integration.Core.Entities.Security.Module>(moduleDTO))
                       .Returns(module);

            _repositoryMock.Setup(r => r.UpdateAsync(module))
                           .ReturnsAsync(module);

            _mapperMock.Setup(m => m.Map<ModuleDTO>(module))
                       .Returns(moduleDTO);

            // Act
            var result = await _moduleService.UpdateAsync(header, moduleDTO);

            // Assert
            Assert.AreEqual(moduleDTO, result);
        }

        [Test]
        public async Task GetAllAsync_WithSinglePredicate_ShouldReturnFilteredModuleDTOs()
        {
            var modules = new List<Integration.Core.Entities.Security.Module>
            {
                new Integration.Core.Entities.Security.Module { Id = 1, Name = "Aplicaciones", Code = "MOD0000001", ApplicationId = 1 }
            };

            var moduleDTOs = new List<ModuleDTO>
            {
                new ModuleDTO { Name = "Aplicaciones", Code = "MOD0000001", CreatedBy = "System", IsActive = true }
            };

            var application = new Integration.Core.Entities.Security.Application { Id = 1, Code = "APP0000001", Name = "Integration" };

            _applicationRepositoryMock.Setup(a => a.GetByCodeAsync("APP0000001"))
                                      .ReturnsAsync(application);

            _repositoryMock.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Integration.Core.Entities.Security.Module, bool>>>()))
                           .ReturnsAsync(modules);

            _mapperMock.Setup(m => m.Map<List<ModuleDTO>>(modules))
                       .Returns(moduleDTOs);

            Expression<Func<ModuleDTO, bool>> filter = dto => dto.Name == "Aplicaciones";

            var result = await _moduleService.GetAllAsync(filter);

            Assert.AreEqual(moduleDTOs, result);
        }

        [Test]
        public async Task GetAllAsync_WithMultiplePredicates_ShouldReturnFilteredModuleDTOs()
        {
            var modules = new List<Integration.Core.Entities.Security.Module>
            {
                new Integration.Core.Entities.Security.Module { Id = 1, Name = "Aplicaciones", Code = "MOD0000001", ApplicationId = 1 }
            };

            var moduleDTOs = new List<ModuleDTO>
            {
                new ModuleDTO { Name = "Aplicaciones", Code = "MOD0000001", CreatedBy = "System", IsActive = true }
            };

            var predicates = new List<Expression<Func<ModuleDTO, bool>>>
            {
                dto => dto.Name == "Aplicaciones",
                dto => dto.Code == "MOD0000001"
            };

            _repositoryMock.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Integration.Core.Entities.Security.Module, bool>>>()))
                           .ReturnsAsync(modules);

            _mapperMock.Setup(m => m.Map<List<ModuleDTO>>(modules))
                       .Returns(moduleDTOs);

            var result = await _moduleService.GetAllAsync(predicates);

            Assert.AreEqual(moduleDTOs, result);
        }
    }
}