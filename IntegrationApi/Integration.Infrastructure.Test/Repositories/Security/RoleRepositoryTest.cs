using Integration.Core.Entities.Security;
using Integration.Infrastructure.Interfaces.Security;
using Moq;
using System.Linq.Expressions;
namespace Integration.Infrastructure.Test.Repositories.Security
{
    [TestFixture]
    public class RoleRepositoryTest
    {
        private Mock<IRoleRepository> _repository;

        [SetUp]
        public void Setup()
        {
            _repository = new Mock<IRoleRepository>();
        }

        [Test]
        public async Task GetByCodeAsync_ShouldReturnCorrectRole()
        {
            // Arrange
            var role = new Role { Id = 1, Code = "ROL0000001", Name = "Administrador", IsActive = true, CreatedBy = "epulido" };
            _repository.Setup(repo => repo.GetByCodeAsync("ROL0000001"))
                .ReturnsAsync(role);

            // Act
            var result = await _repository.Object.GetByCodeAsync("ROL0000001");

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("ROL0000001", result.Code);
            Assert.AreEqual("Administrador", result.Name);
        }

        [Test]
        public async Task GetByCodeAsync_ShouldReturnNullForNonExistentCode()
        {
            // Arrange
            _repository.Setup(repo => repo.GetByCodeAsync("INVALID_CODE"))
                .ReturnsAsync((Role)null);

            // Act
            var result = await _repository.Object.GetByCodeAsync("INVALID_CODE");

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetAllAsync_WithPredicate_ShouldReturnFilteredRoles()
        {
            // Arrange
            var roles = new List<Role>
            {
                new Role { Id = 1, Code = "ROL0000001", Name = "Administrador", IsActive = true, CreatedBy = "epulido" },
                new Role { Id = 2, Code = "ROL0000002", Name = "Contratista", IsActive = false, CreatedBy = "epulido" }
            };

            Expression<Func<Role, bool>> predicate = role => role.IsActive;
            var expectedResults = roles.Where(predicate.Compile()).ToList();

            _repository.Setup(repo => repo.GetAllAsync(predicate))
                .ReturnsAsync(expectedResults);

            // Act
            var result = await _repository.Object.GetAllAsync(predicate);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(expectedResults.Count, result.Count);
            Assert.IsTrue(result.All(role => role.IsActive));
        }

        [Test]
        public async Task GetAllAsync_WithPredicates_ShouldReturnFilteredRoles()
        {
            // Arrange
            var roles = new List<Role>
            {
                new Role { Id = 1, Code = "ROL0000001", Name = "Administrador", IsActive = true, CreatedBy = "epulido" },
                new Role { Id = 2, Code = "ROL0000002", Name = "Contratista", IsActive = false, CreatedBy = "epulido" }
            };

            var predicates = new List<Expression<Func<Role, bool>>>
            {
                role => role.IsActive,
                role => role.Code.Contains("ROL000000")
            };

            var expectedResults = roles.Where(role => predicates.All(p => p.Compile()(role))).ToList();

            _repository.Setup(repo => repo.GetAllAsync(predicates))
                .ReturnsAsync(expectedResults);

            // Act
            var result = await _repository.Object.GetAllAsync(predicates);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(expectedResults.Count, result.Count);
            Assert.IsTrue(result.All(role => role.IsActive));
        }

        [Test]
        public async Task UpdateAsync_ShouldReturnUpdatedRole()
        {
            // Arrange
            var role = new Role { Id = 1, Code = "ROL0000001", Name = "Administrador", IsActive = true, CreatedBy = "epulido" };

            _repository.Setup(repo => repo.UpdateAsync(It.IsAny<Role>()))
                .ReturnsAsync((Role r) => r);

            // Act
            var result = await _repository.Object.UpdateAsync(role);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("Administrador", result.Name);
        }

        [Test]
        public async Task DeactivateAsync_ShouldReturnTrue()
        {
            // Arrange
            _repository.Setup(repo => repo.DeactivateAsync("ROL0000001", "epulido"))
                .ReturnsAsync(true);

            // Act
            var result = await _repository.Object.DeactivateAsync("ROL0000001", "epulido");

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task GetAllActiveAsync_ShouldReturnActiveRoles()
        {
            // Arrange
            var activeRoles = new List<Role>
            {
                new Role { Id = 1, Code = "ROL0000001", Name = "Administrador", IsActive = true, CreatedBy = "epulido" },
                new Role { Id = 2, Code = "ROL0000002", Name = "Contratista", IsActive = true, CreatedBy = "epulido" }
            };

            _repository.Setup(repo => repo.GetAllActiveAsync())
                .ReturnsAsync(activeRoles);

            // Act
            var result = await _repository.Object.GetAllActiveAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsTrue(result.All(role => role.IsActive));
        }

        [Test]
        public async Task CreateAsync_ShouldReturnCreatedRole()
        {
            // Arrange
            var role = new Role { Id = 1, Code = "ROL0000001", Name = "Administrador", IsActive = true, CreatedBy = "epulido" };

            _repository.Setup(repo => repo.CreateAsync(It.IsAny<Role>()))
                .ReturnsAsync((Role r) => r);

            // Act
            var result = await _repository.Object.CreateAsync(role);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("ROL0000001", result.Code);
        }
    }
}