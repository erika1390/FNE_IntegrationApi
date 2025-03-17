using AutoMapper;
using Integration.Application.Interfaces.Security;
using Integration.Application.Services.Security;
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

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IRoleRepository>();
            _applicationRepositoryMock = new Mock<IApplicationRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<RoleService>>();
            _roleService = new RoleService(_repositoryMock.Object, _mapperMock.Object, _loggerMock.Object, _applicationRepositoryMock.Object);
        }

        [Test]
        public async Task CreateAsync_ShouldReturnCreatedRoleDTO()
        {
            var header = new HeaderDTO { ApplicationCode = "APP0000001", UserCode = "USR0000001" };
            var roleDTO = new RoleDTO { Name = "Administrador", Code = "ROL0000001", CreatedBy = "System", IsActive = true };
            var role = new Integration.Core.Entities.Security.Role { Id = 1, Name = "Administrador", Code = "ROL0000001", CreatedBy = "System"};

            _mapperMock.Setup(m => m.Map<Integration.Core.Entities.Security.Role>(roleDTO)).Returns(role);
            _repositoryMock.Setup(r => r.CreateAsync(role)).ReturnsAsync(role);
            _mapperMock.Setup(m => m.Map<RoleDTO>(role)).Returns(roleDTO);

            var result = await _roleService.CreateAsync(header, roleDTO);

            Assert.AreEqual(roleDTO, result);
        }

        [Test]
        public async Task DeleteAsync_ShouldReturnTrue_WhenRoleIsDeleted()
        {
            var header = new HeaderDTO { ApplicationCode = "APP0000001", UserCode = "USR0000001" };
            string roleCode = "ROL0000001";
            _repositoryMock.Setup(r => r.DeactivateAsync(roleCode)).ReturnsAsync(true);

            var result = await _roleService.DeactivateAsync(header, roleCode);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task DeleteAsync_ShouldReturnFalse_WhenRoleIsNotFound()
        {
            var header = new HeaderDTO { ApplicationCode = "APP0000001", UserCode = "USR0000001" };
            string roleCode = "ROL0000001";
            _repositoryMock.Setup(r => r.DeactivateAsync(roleCode)).ReturnsAsync(false);

            var result = await _roleService.DeactivateAsync(header, roleCode);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetAllActiveAsync_ShouldReturnListOfRoleDTOs()
        {
            var Roles = new List<Integration.Core.Entities.Security.Role> { new Integration.Core.Entities.Security.Role { Id = 1, Name = "Administrador", Code = "ROL0000001", CreatedBy = "System" } };
            var RoleDTOs = new List<RoleDTO> { new RoleDTO { Name = "Administrador", Code = "ROL0000001", CreatedBy = "System", IsActive = true } };

            _repositoryMock.Setup(r => r.GetAllActiveAsync()).ReturnsAsync(Roles);
            _mapperMock.Setup(m => m.Map<IEnumerable<RoleDTO>>(Roles)).Returns(RoleDTOs);

            var result = await _roleService.GetAllActiveAsync();

            Assert.AreEqual(RoleDTOs, result);
        }

        [Test]
        public async Task GetByCodeAsync_ShouldReturnRoleDTO_WhenRoleExists()
        {
            string roleCode = "ROL0000001";
            var Role = new Integration.Core.Entities.Security.Role { Id = 1, Name = "Administrador", Code = "ROL0000001", CreatedBy = "System" };
            var RoleDTO = new RoleDTO { Name = "Administrador", Code = "ROL0000001", CreatedBy = "System", IsActive = true };

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
            var header = new HeaderDTO { ApplicationCode = "APP0000001", UserCode = "USR0000001" };
            var roleDTO = new RoleDTO { Name = "Administrador", Code = "ROL0000001", CreatedBy = "System", IsActive = true };
            var role = new Integration.Core.Entities.Security.Role { Id = 1, Name = "Administrador", Code = "ROL0000001" , CreatedBy = "System" };

            _mapperMock.Setup(m => m.Map<Integration.Core.Entities.Security.Role>(roleDTO)).Returns(role);
            _repositoryMock.Setup(r => r.UpdateAsync(role)).ReturnsAsync(role);
            _mapperMock.Setup(m => m.Map<RoleDTO>(role)).Returns(roleDTO);

            var result = await _roleService.UpdateAsync(header, roleDTO);

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
                    Name = "System",
                    Code = "ROL0000001",
                    ApplicationId = 1,
                    CreatedBy ="System"
                }
            };

            var roleDTOs = new List<RoleDTO>
            {
                new RoleDTO
                {
                    Name = "System",
                    Code = "ROL0000001",
                    ApplicationCode = "APP0000001",
                    CreatedBy ="System",
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

            _repositoryMock.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Integration.Core.Entities.Security.Role, bool>>>()))
                           .ReturnsAsync(roles);

            _mapperMock.Setup(m => m.Map<List<RoleDTO>>(roles))
                       .Returns(roleDTOs);

            // Filtramos por el nombre "System" que SÍ existe en la lista de roles simulada.
            Expression<Func<RoleDTO, bool>> filter = dto => dto.Name == "System";

            // Act
            var result = await _roleService.GetAllAsync(filter);

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
                new Integration.Core.Entities.Security.Role { Id = 1, Name = "System", Code = "ROL0000001", ApplicationId = 1, CreatedBy = "System"}
            };

            var roleDTOs = new List<RoleDTO>
            {
                new RoleDTO { Name = "System", Code = "ROL0000001", ApplicationCode = "APP0000001", CreatedBy ="System", IsActive = true }
            };

            var predicates = new List<Expression<Func<RoleDTO, bool>>>
            {
                dto => dto.Name == "System",
                dto => dto.Code == "ROL0000001"
            };

            _repositoryMock.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Integration.Core.Entities.Security.Role, bool>>>()))
                           .ReturnsAsync(roles);

            _mapperMock.Setup(m => m.Map<List<RoleDTO>>(roles))
                       .Returns(roleDTOs);

            var result = await _roleService.GetAllAsync(predicates);

            Assert.AreEqual(roleDTOs, result);
        }
    }
}
