using Integration.Core.Entities.Security;
using Integration.Infrastructure.Interfaces.Security;
using Moq;
using System.Linq.Expressions;
namespace Integration.Infrastructure.Test.Repositories.Security
{
    [TestFixture]
    public class RoleRepositoryTest
    {
        private Mock<IRoleRepository> _mock;

        [SetUp]
        public void Setup()
        {
            _mock = new Mock<IRoleRepository>();
        }

        [Test]
        public async Task GetByCodeAsync_ShouldReturnCorrectRole()
        {
            // Arrange
            var role = new Role { Id = 1, Code = "ROL0000001", Name = "System", IsActive = true, CreatedBy = "System" };
            _mock.Setup(repo => repo.GetByCodeAsync("ROL0000001"))
                .ReturnsAsync(role);

            // Act
            var result = await _mock.Object.GetByCodeAsync("ROL0000001");

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("ROL0000001", result.Code);
            Assert.AreEqual("System", result.Name);
        }

        [Test]
        public async Task GetByCodeAsync_ShouldReturnNullForNonExistentCode()
        {
            // Arrange
            _mock.Setup(repo => repo.GetByCodeAsync("INVALID_CODE"))
                .ReturnsAsync((Role)null);

            // Act
            var result = await _mock.Object.GetByCodeAsync("INVALID_CODE");

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetAllAsync_WithPredicate_ShouldReturnFilteredRoles()
        {
            // Arrange
            var roles = new List<Role>
            {
                new Role { Id = 1, Code = "ROL0000001", Name = "System", IsActive = true, CreatedBy = "System" },
                new Role { Id = 2, Code = "ROL0000002", Name = "Administrador", IsActive = false, CreatedBy = "System" }
            };

            Expression<Func<Role, bool>> predicate = role => role.IsActive;
            var expectedResults = roles.Where(predicate.Compile()).ToList();

            _mock.Setup(repo => repo.GetAllAsync(predicate))
                .ReturnsAsync(expectedResults);

            // Act
            var result = await _mock.Object.GetAllAsync(predicate);

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
                new Role { Id = 1, Code = "ROL0000001", Name = "System", IsActive = true, CreatedBy = "System" },
                new Role { Id = 2, Code = "ROL0000002", Name = "Administrador", IsActive = false, CreatedBy = "System" }
            };

            var predicates = new List<Expression<Func<Role, bool>>>
            {
                role => role.IsActive,
                role => role.Code.Contains("System")
            };

            var expectedResults = roles.Where(role => predicates.All(p => p.Compile()(role))).ToList();

            _mock.Setup(repo => repo.GetAllAsync(predicates))
                .ReturnsAsync(expectedResults);

            // Act
            var result = await _mock.Object.GetAllAsync(predicates);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(expectedResults.Count, result.Count);
            Assert.IsTrue(result.All(role => role.IsActive));
        }

        [Test]
        public async Task UpdateAsync_ShouldReturnUpdatedRole()
        {
            // Arrange
            var role = new Role { Id = 1, Code = "ROL0000001", Name = "System", IsActive = true, CreatedBy = "System" };

            _mock.Setup(repo => repo.UpdateAsync(It.IsAny<Role>()))
                .ReturnsAsync((Role r) => r);

            // Act
            var result = await _mock.Object.UpdateAsync(role);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("System", result.Name);
        }

        [Test]
        public async Task DeactivateAsync_ShouldReturnTrue()
        {
            // Arrange
            _mock.Setup(repo => repo.DeactivateAsync("ROL0000001", "epulido"))
                .ReturnsAsync(true);

            // Act
            var result = await _mock.Object.DeactivateAsync("ROL0000001", "epulido");

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task GetAllActiveAsync_ShouldReturnActiveRoles()
        {
            // Arrange
            var activeRoles = new List<Role>
            {
                new Role { Id = 1, Code = "ROL0000001", Name = "System", IsActive = true, CreatedBy = "System" },
                new Role { Id = 2, Code = "ROL0000002", Name = "Administrador", IsActive = true, CreatedBy = "System" }
            };

            _mock.Setup(repo => repo.GetAllActiveAsync())
                .ReturnsAsync(activeRoles);

            // Act
            var result = await _mock.Object.GetAllActiveAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsTrue(result.All(role => role.IsActive));
        }

        [Test]
        public async Task CreateAsync_ShouldReturnCreatedRole()
        {
            // Arrange
            var role = new Role { Id = 1, Code = "ROL0000001", Name = "System", IsActive = true, CreatedBy = "System" };

            _mock.Setup(repo => repo.CreateAsync(It.IsAny<Role>()))
                .ReturnsAsync((Role r) => r);

            // Act
            var result = await _mock.Object.CreateAsync(role);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("ROL0000001", result.Code);
        }
    }
}