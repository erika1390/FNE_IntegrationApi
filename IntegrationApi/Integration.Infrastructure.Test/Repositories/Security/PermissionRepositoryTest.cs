using Integration.Core.Entities.Security;
using Integration.Infrastructure.Interfaces.Security;
using Moq;
using System.Linq.Expressions;
namespace Integration.Infrastructure.Test.Repositories.Security
{
    [TestFixture]
    public class PermissionRepositoryTest
    {
        private Mock<IPermissionRepository> _mock;

        [SetUp]
        public void Setup()
        {
            _mock = new Mock<IPermissionRepository>();
        }

        [Test]
        public async Task GetByCodeAsync_ShouldReturnCorrectPermission()
        {
            // Arrange
            var permission = new Permission { Id = 1, Code = "PER0000001", Name = "Consultar", IsActive = true };
            _mock.Setup(repo => repo.GetByCodeAsync("PER0000001"))
                .ReturnsAsync(permission);

            // Act
            var result = await _mock.Object.GetByCodeAsync("PER0000001");

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("PER0000001", result.Code);
            Assert.AreEqual("Consultar", result.Name);
        }

        [Test]
        public async Task GetByCodeAsync_ShouldReturnNullForNonExistentCode()
        {
            // Arrange
            _mock.Setup(repo => repo.GetByCodeAsync("INVALID_CODE"))
                .ReturnsAsync((Permission)null);

            // Act
            var result = await _mock.Object.GetByCodeAsync("INVALID_CODE");

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetAllAsync_WithPredicate_ShouldReturnFilteredPermissions()
        {
            // Arrange
            var permissions = new List<Permission>
            {
                new Permission { Id = 1, Code = "PER0000001", Name = "Consultar", IsActive = true },
                new Permission { Id = 2, Code = "PER0000002", Name = "Crear", IsActive = false },
                new Permission { Id = 3, Code = "PER0000003", Name = "Modificar", IsActive = false }
            };

            Expression<Func<Permission, bool>> predicate = perm => perm.IsActive;
            var expectedResults = permissions.Where(predicate.Compile()).ToList();

            _mock.Setup(repo => repo.GetAllAsync(predicate))
                .ReturnsAsync(expectedResults);

            // Act
            var result = await _mock.Object.GetAllAsync(predicate);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(expectedResults.Count, result.Count);
            Assert.IsTrue(result.All(perm => perm.IsActive));
        }

        [Test]
        public async Task GetAllAsync_WithPredicates_ShouldReturnFilteredPermissions()
        {
            // Arrange
            var permissions = new List<Permission>
            {
                new Permission { Id = 1, Code = "PER0000001", Name = "Consultar", IsActive = true },
                new Permission { Id = 2, Code = "PER0000002", Name = "Crear", IsActive = false },
                new Permission { Id = 3, Code = "PER0000003", Name = "Modificar", IsActive = false }
            };

            var predicates = new List<Expression<Func<Permission, bool>>>
            {
                perm => perm.IsActive,
                perm => perm.Code.Contains("PER")
            };

            var expectedResults = permissions.Where(perm => predicates.All(p => p.Compile()(perm))).ToList();

            _mock.Setup(repo => repo.GetAllAsync(predicates))
                .ReturnsAsync(expectedResults);

            // Act
            var result = await _mock.Object.GetAllAsync(predicates);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(expectedResults.Count, result.Count);
            Assert.IsTrue(result.All(perm => perm.IsActive));
        }

        [Test]
        public async Task UpdateAsync_ShouldReturnUpdatedPermission()
        {
            // Arrange
            var permission = new Permission { Id = 1, Code = "PER0000001", Name = "Consultar", IsActive = true };

            _mock.Setup(repo => repo.UpdateAsync(It.IsAny<Permission>()))
                .ReturnsAsync((Permission perm) => perm);

            // Act
            var result = await _mock.Object.UpdateAsync(permission);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("Consultar", result.Name);
        }

        [Test]
        public async Task DeactivateAsync_ShouldReturnTrue()
        {
            // Arrange
            _mock.Setup(repo => repo.DeactivateAsync("PER0000001", "epulido"))
                .ReturnsAsync(true);

            // Act
            var result = await _mock.Object.DeactivateAsync("PER0000001", "epulido");

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task GetAllActiveAsync_ShouldReturnActivePermissions()
        {
            // Arrange
            var activePermissions = new List<Permission>
            {
                new Permission { Id = 1, Code = "PER0000001", Name = "Consultar", IsActive = true },
                new Permission { Id = 2, Code = "PER0000002", Name = "Crear", IsActive = true }
            };

            _mock.Setup(repo => repo.GetAllActiveAsync())
                .ReturnsAsync(activePermissions);

            // Act
            var result = await _mock.Object.GetAllActiveAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsTrue(result.All(perm => perm.IsActive));
        }

        [Test]
        public async Task CreateAsync_ShouldReturnCreatedPermission()
        {
            // Arrange
            var permission = new Permission { Id = 1, Code = "PER0000001", Name = "Consultar", IsActive = true };

            _mock.Setup(repo => repo.CreateAsync(It.IsAny<Permission>()))
                .ReturnsAsync((Permission perm) => perm);

            // Act
            var result = await _mock.Object.CreateAsync(permission);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("PER0000001", result.Code);
        }
    }
}