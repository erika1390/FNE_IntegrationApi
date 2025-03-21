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
    public class RoleServiceTest
    {
        private Mock<IRoleRepository> _repositoryMock;
        private Mock<IApplicationRepository> _applicationRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private Mock<ILogger<RoleService>> _loggerMock;
        private IRoleService _roleService;
        private Mock<IUserRepository> _userRepositoryMock;
        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IRoleRepository>();
            _applicationRepositoryMock = new Mock<IApplicationRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<RoleService>>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _roleService = new RoleService(_repositoryMock.Object, _mapperMock.Object, _loggerMock.Object, _applicationRepositoryMock.Object, _userRepositoryMock.Object);
        }

        [Test]
        public async Task CreateAsync_ShouldReturnCreatedRoleDTO()
        {
            // Arrange
            var header = new HeaderDTO { ApplicationCode = "APP0000001", UserCode = "USR0000001" };
            var roleDTO = new RoleDTO { Name = "Administrador", Code = "ROL0000001", CreatedBy = "epulido", IsActive = true };

            var role = new Integration.Core.Entities.Security.Role
            {
                Id = 1,
                Name = "Administrador",
                Code = "ROL0000001",
                IsActive = true,
                CreatedBy = "epulido"
            };

            // ✅ Simular un usuario válido en el repositorio
            var user = new User
            {
                Code = "USR0000001",
                FirstName = "Erika",
                LastName = "Pulido Moreno",
                CreatedAt = DateTime.Now,
                CreatedBy = "epulido",
                IsActive = true,
                UserName = "epulido",
                Email = "epulido@minsalud.gov.co"
            };

            var application = new Integration.Core.Entities.Security.Application
            {
                Id = 100, // Simular un ID válido
                Code = "APP0000001",
                Name = "Test Application"
            };

            Assert.NotNull(user);
            Assert.NotNull(application);

            // Configurar simulación del repositorio de usuarios
            _userRepositoryMock.Setup(r => r.GetByCodeAsync(header.UserCode))
                               .ReturnsAsync(user);

            // Configurar simulación del repositorio de aplicaciones
            _applicationRepositoryMock.Setup(r => r.GetByCodeAsync(header.ApplicationCode))
                                      .ReturnsAsync(application);

            // Simulación del mapeo RoleDTO → Role
            _mapperMock.Setup(m => m.Map<Integration.Core.Entities.Security.Role>(roleDTO))
                       .Returns(role);

            var mappedRole = _mapperMock.Object.Map<Integration.Core.Entities.Security.Role>(roleDTO);
            Assert.NotNull(mappedRole);

            // Asignar el ApplicationId manualmente en la prueba
            mappedRole.ApplicationId = application.Id;
            mappedRole.CreatedBy = user.UserName;
            mappedRole.UpdatedBy = user.UserName;

            // Simulación de la creación del rol en el repositorio
            _repositoryMock.Setup(r => r.CreateAsync(mappedRole))
                               .ReturnsAsync(mappedRole);

            var createdRole = await _repositoryMock.Object.CreateAsync(mappedRole);
            Assert.NotNull(createdRole);

            // Simulación del mapeo Role → RoleDTO
            _mapperMock.Setup(m => m.Map<RoleDTO>(createdRole))
                       .Returns(roleDTO);

            // Act
            var result = await _roleService.CreateAsync(header, roleDTO);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(roleDTO.Name, result.Name);
            Assert.AreEqual(roleDTO.Code, result.Code);
            Assert.AreEqual(roleDTO.CreatedBy, result.CreatedBy);
            Assert.AreEqual(roleDTO.IsActive, result.IsActive);
        }

        [Test]
        public async Task DeleteAsync_ShouldReturnTrue_WhenRoleIsDeleted()
        {
            // Arrange
            var header = new HeaderDTO { ApplicationCode = "APP0000001", UserCode = "USR0000001" };
            string roleCode = "ROL0000001";

            // ✅ Simular un usuario válido en el repositorio
            var user = new User
            {
                Code = "USR0000001",
                FirstName = "Erika",
                LastName = "Pulido Moreno",
                CreatedAt = DateTime.Now,
                CreatedBy = "epulido",
                IsActive = true,
                UserName = "epulido",
                Email = "epulido@minsalud.gov.co"
            };
            _userRepositoryMock.Setup(r => r.GetByCodeAsync(header.UserCode)).ReturnsAsync(user);

            // ✅ Simular que el rol fue eliminado con éxito
            _repositoryMock.Setup(r => r.DeactivateAsync(roleCode, user.UserName)).ReturnsAsync(true);

            // Act
            var result = await _roleService.DeactivateAsync(header, roleCode);

            // Assert
            Assert.IsTrue(result);
        }


        [Test]
        public async Task DeleteAsync_ShouldReturnFalse_WhenRoleIsNotFound()
        {
            // Arrange
            var header = new HeaderDTO { ApplicationCode = "APP0000001", UserCode = "USR0000001" };
            string roleCode = "ROL0000001";

            // ✅ Simular un usuario válido en el repositorio
            var user = new User
            {
                Code = "USR0000001",
                FirstName = "Erika",
                LastName = "Pulido Moreno",
                CreatedAt = DateTime.Now,
                CreatedBy = "epulido",
                IsActive = true,
                UserName = "epulido",
                Email = "epulido@minsalud.gov.co"
            };
            _userRepositoryMock.Setup(r => r.GetByCodeAsync(header.UserCode)).ReturnsAsync(user);

            // ✅ Simular que el rol no fue encontrado
            _repositoryMock.Setup(r => r.DeactivateAsync(roleCode, user.UserName)).ReturnsAsync(false);

            // Act
            var result = await _roleService.DeactivateAsync(header, roleCode);

            // Assert
            Assert.IsFalse(result);
        }


        [Test]
        public async Task GetAllActiveAsync_ShouldReturnListOfRoleDTOs()
        {
            var Roles = new List<Integration.Core.Entities.Security.Role> { new Integration.Core.Entities.Security.Role { Id = 1, Name = "Administrador", Code = "ROL0000001", CreatedBy = "epulido" } };
            var RoleDTOs = new List<RoleDTO> { new RoleDTO { Name = "Administrador", Code = "ROL0000001", CreatedBy = "epulido", IsActive = true } };

            _repositoryMock.Setup(r => r.GetAllActiveAsync()).ReturnsAsync(Roles);
            _mapperMock.Setup(m => m.Map<IEnumerable<RoleDTO>>(Roles)).Returns(RoleDTOs);

            var result = await _roleService.GetAllActiveAsync();

            Assert.AreEqual(RoleDTOs, result);
        }

        [Test]
        public async Task GetByCodeAsync_ShouldReturnRoleDTO_WhenRoleExists()
        {
            string roleCode = "ROL0000001";
            var Role = new Integration.Core.Entities.Security.Role { Id = 1, Name = "Administrador", Code = "ROL0000001", CreatedBy = "epulido" };
            var RoleDTO = new RoleDTO { Name = "Administrador", Code = "ROL0000001", CreatedBy = "epulido", IsActive = true };

            _repositoryMock.Setup(r => r.GetByCodeAsync(roleCode)).ReturnsAsync(Role);
            _mapperMock.Setup(m => m.Map<RoleDTO>(Role)).Returns(RoleDTO);

            var result = await _roleService.GetByCodeAsync(roleCode);

            Assert.AreEqual(RoleDTO, result);
        }

        [Test]
        public async Task GetByCodeAsync_ShouldReturnNull_WhenRoleDoesNotExist()
        {
            string roleCode = "ROL0000001";
            _repositoryMock.Setup(r => r.GetByCodeAsync(roleCode)).ReturnsAsync((Integration.Core.Entities.Security.Role)null);

            var result = await _roleService.GetByCodeAsync(roleCode);

            Assert.IsNull(result);
        }

        [Test]
        public async Task UpdateAsync_ShouldReturnUpdatedRoleDTO()
        {
            // Arrange
            var header = new HeaderDTO { ApplicationCode = "APP0000001", UserCode = "USR0000001" };
            var roleDTO = new RoleDTO { Name = "Administrador", Code = "ROL0000001", CreatedBy = "epulido", IsActive = true };
            var role = new Integration.Core.Entities.Security.Role { Id = 1, Name = "Administrador", Code = "ROL0000001", CreatedBy = "epulido" };

            // ✅ Simular un usuario válido en el repositorio
            var user = new User
            {
                Code = "USR0000001",
                FirstName = "Erika",
                LastName = "Pulido Moreno",
                CreatedAt = DateTime.Now,
                CreatedBy = "epulido",
                IsActive = true,
                UserName = "epulido",
                Email = "epulido@minsalud.gov.co"
            };

            // ✅ Simular una aplicación válida en el repositorio
            var application = new Integration.Core.Entities.Security.Application
            {
                Id = 1,
                Code = "APP0000001",
                Name = "Aplicación de Seguridad"
            };

            Assert.NotNull(user, "El usuario no debería ser nulo.");
            _userRepositoryMock.Setup(r => r.GetByCodeAsync(header.UserCode)).ReturnsAsync(user);

            Assert.NotNull(application, "La aplicación no debería ser nula.");
            _applicationRepositoryMock.Setup(a => a.GetByCodeAsync(header.ApplicationCode)).ReturnsAsync(application);

            Assert.NotNull(roleDTO, "El roleDTO no debería ser nulo.");
            _mapperMock.Setup(m => m.Map<Integration.Core.Entities.Security.Role>(roleDTO)).Returns(role);
            Assert.NotNull(role, "El objeto Role no debería ser nulo.");

            _repositoryMock.Setup(r => r.UpdateAsync(role)).ReturnsAsync(role);
            _mapperMock.Setup(m => m.Map<RoleDTO>(role)).Returns(roleDTO);

            // Act
            var result = await _roleService.UpdateAsync(header, roleDTO);

            // Assert
            Assert.NotNull(result, "El resultado no debería ser nulo.");
            Assert.AreEqual(roleDTO, result);
        }

        [Test]
        public async Task GetAllAsync_WithSinglePredicate_ShouldReturnFilteredRoleDTOs()
        {
            // Arrange
            var roles = new List<Integration.Core.Entities.Security.Role>
            {
                new Integration.Core.Entities.Security.Role
                {
                    Id = 1,
                    Name = "Administrador",
                    Code = "ROL0000001",
                    ApplicationId = 1,
                    CreatedBy ="epulido"
                }
            };

            var roleDTOs = new List<RoleDTO>
            {
                new RoleDTO
                {
                    Name = "Administrador",
                    Code = "ROL0000001",
                    CreatedBy ="epulido",
                    IsActive = true
                }
            };

            var application = new Integration.Core.Entities.Security.Application
            {
                Id = 1,
                Code = "APP0000001",
                Name = "Integration"
            };

            _applicationRepositoryMock.Setup(a => a.GetByCodeAsync("APP0000001"))
                                      .ReturnsAsync(application);

            _repositoryMock.Setup(r => r.GetByFilterAsync(It.IsAny<Expression<Func<Integration.Core.Entities.Security.Role, bool>>>()))
                           .ReturnsAsync(roles);

            _mapperMock.Setup(m => m.Map<List<RoleDTO>>(roles))
                       .Returns(roleDTOs);

            Expression<Func<RoleDTO, bool>> filter = dto => dto.Name == "Administrador";

            // Act
            var result = await _roleService.GetByFilterAsync(filter);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.Count); // Se espera un solo resultado
            Assert.AreEqual(roleDTOs.First().Name, result.First().Name);
            Assert.AreEqual(roleDTOs.First().Code, result.First().Code);
        }


        [Test]
        public async Task GetAllAsync_WithMultiplePredicates_ShouldReturnFilteredRoleDTOs()
        {
            var roles = new List<Integration.Core.Entities.Security.Role>
            {
                new Integration.Core.Entities.Security.Role { Id = 1, Name = "Administrador", Code = "ROL0000001", ApplicationId = 1, CreatedBy = "epulido"}
            };

            var roleDTOs = new List<RoleDTO>
            {
                new RoleDTO { Name = "Administrador", Code = "ROL0000001", CreatedBy ="epulido", IsActive = true }
            };

            var predicates = new List<Expression<Func<RoleDTO, bool>>>
            {
                dto => dto.Name == "Administrador",
                dto => dto.Code == "ROL0000001"
            };

            _repositoryMock.Setup(r => r.GetByFilterAsync(It.IsAny<Expression<Func<Integration.Core.Entities.Security.Role, bool>>>()))
                           .ReturnsAsync(roles);

            _mapperMock.Setup(m => m.Map<List<RoleDTO>>(roles))
                       .Returns(roleDTOs);

            var result = await _roleService.GetByMultipleFiltersAsync(predicates);

            Assert.AreEqual(roleDTOs, result);
        }
    }
}
