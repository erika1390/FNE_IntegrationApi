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
    public class RoleServiceTest
    {
        private Mock<IRoleRepository> _repositoryMock;
        private Mock<IMapper> _mapperMock;
        private Mock<ILogger<RoleService>> _loggerMock;
        private IRoleService _RoleService;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IRoleRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<RoleService>>();
            _RoleService = new RoleService(_repositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task CreateAsync_ShouldReturnCreatedRoleDTO()
        {
            var RoleDTO = new RoleDTO { RoleId = 1, Name = "Administrador", Code = "ROL0000001", CreatedBy = "System", IsActive = true };
            var Role = new Integration.Core.Entities.Security.Role { Id = 1, Name = "Administrador", Code = "ROL0000001", CreatedBy = "System"};

            _mapperMock.Setup(m => m.Map<Integration.Core.Entities.Security.Role>(RoleDTO)).Returns(Role);
            _repositoryMock.Setup(r => r.CreateAsync(Role)).ReturnsAsync(Role);
            _mapperMock.Setup(m => m.Map<RoleDTO>(Role)).Returns(RoleDTO);

            var result = await _RoleService.CreateAsync(RoleDTO);

            Assert.AreEqual(RoleDTO, result);
        }

        [Test]
        public async Task DeleteAsync_ShouldReturnTrue_WhenRoleIsDeleted()
        {
            int RoleId = 1;
            _repositoryMock.Setup(r => r.DeleteAsync(RoleId)).ReturnsAsync(true);

            var result = await _RoleService.DeactivateAsync(RoleId);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task DeleteAsync_ShouldReturnFalse_WhenRoleIsNotFound()
        {
            int RoleId = 1;
            _repositoryMock.Setup(r => r.DeleteAsync(RoleId)).ReturnsAsync(false);

            var result = await _RoleService.DeactivateAsync(RoleId);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetAllActiveAsync_ShouldReturnListOfRoleDTOs()
        {
            var Roles = new List<Integration.Core.Entities.Security.Role> { new Integration.Core.Entities.Security.Role { Id = 1, Name = "Administrador", Code = "ROL0000001", CreatedBy = "System" } };
            var RoleDTOs = new List<RoleDTO> { new RoleDTO { RoleId = 1, Name = "Administrador", Code = "ROL0000001", CreatedBy = "System", IsActive = true } };

            _repositoryMock.Setup(r => r.GetAllActiveAsync()).ReturnsAsync(Roles);
            _mapperMock.Setup(m => m.Map<IEnumerable<RoleDTO>>(Roles)).Returns(RoleDTOs);

            var result = await _RoleService.GetAllActiveAsync();

            Assert.AreEqual(RoleDTOs, result);
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnRoleDTO_WhenRoleExists()
        {
            int RoleId = 1;
            var Role = new Integration.Core.Entities.Security.Role { Id = RoleId, Name = "Administrador", Code = "ROL0000001", CreatedBy = "System" };
            var RoleDTO = new RoleDTO { RoleId = RoleId, Name = "Administrador", Code = "ROL0000001", CreatedBy = "System", IsActive = true };

            _repositoryMock.Setup(r => r.GetByIdAsync(RoleId)).ReturnsAsync(Role);
            _mapperMock.Setup(m => m.Map<RoleDTO>(Role)).Returns(RoleDTO);

            var result = await _RoleService.GetByIdAsync(RoleId);

            Assert.AreEqual(RoleDTO, result);
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnNull_WhenRoleDoesNotExist()
        {
            int RoleId = 1;
            _repositoryMock.Setup(r => r.GetByIdAsync(RoleId)).ReturnsAsync((Integration.Core.Entities.Security.Role)null);

            var result = await _RoleService.GetByIdAsync(RoleId);

            Assert.IsNull(result);
        }

        [Test]
        public async Task UpdateAsync_ShouldReturnUpdatedRoleDTO()
        {
            var RoleDTO = new RoleDTO { RoleId = 1, Name = "Administrador", Code = "ROL0000001", CreatedBy = "System", IsActive = true };
            var Role = new Integration.Core.Entities.Security.Role { Id = 1, Name = "Administrador", Code = "ROL0000001" , CreatedBy = "System" };

            _mapperMock.Setup(m => m.Map<Integration.Core.Entities.Security.Role>(RoleDTO)).Returns(Role);
            _repositoryMock.Setup(r => r.UpdateAsync(Role)).ReturnsAsync(Role);
            _mapperMock.Setup(m => m.Map<RoleDTO>(Role)).Returns(RoleDTO);

            var result = await _RoleService.UpdateAsync(RoleDTO);

            Assert.AreEqual(RoleDTO, result);
        }

        [Test]
        public async Task GetAllAsync_WithSinglePredicate_ShouldReturnFilteredRoleDTOs()
        {
            var Roles = new List<Integration.Core.Entities.Security.Role> { new Integration.Core.Entities.Security.Role { Id = 1, Name = "Administrador", Code = "ROL0000001", CreatedBy = "System" } };
            var RoleDTOs = new List<RoleDTO> { new RoleDTO { RoleId = 1, Name = "Administrador", Code = "ROL0000001", CreatedBy = "System", IsActive = true } };
            Expression<Func<RoleDTO, bool>> filter = dto => dto.Name == "Administrador";

            _repositoryMock.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Integration.Core.Entities.Security.Role, bool>>>())).ReturnsAsync(Roles);
            _mapperMock.Setup(m => m.Map<List<RoleDTO>>(Roles)).Returns(RoleDTOs);

            var result = await _RoleService.GetAllAsync(filter);

            Assert.AreEqual(RoleDTOs, result);
        }

        [Test]
        public async Task GetAllAsync_WithMultiplePredicates_ShouldReturnFilteredRoleDTOs()
        {
            var Roles = new List<Integration.Core.Entities.Security.Role> { new Integration.Core.Entities.Security.Role { Id = 1, Name = "Administrador", Code = "ROL0000001" , CreatedBy = "System" } };
            var RoleDTOs = new List<RoleDTO> { new RoleDTO { RoleId = 1, Name = "Administrador", Code = "ROL0000001", CreatedBy = "System", IsActive = true } };
            var predicates = new List<Expression<Func<RoleDTO, bool>>> { dto => dto.Name == "Administrador", dto => dto.Code == "ROL0000001" };

            _repositoryMock.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Integration.Core.Entities.Security.Role, bool>>>())).ReturnsAsync(Roles);
            _mapperMock.Setup(m => m.Map<List<RoleDTO>>(Roles)).Returns(RoleDTOs);

            var result = await _RoleService.GetAllAsync(predicates);

            Assert.AreEqual(RoleDTOs, result);
        }
    }
}
