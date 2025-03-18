using Integration.Core.Entities.Security;
using Integration.Infrastructure.Interfaces.Security;

using Moq;

using System.Linq.Expressions;

namespace Integration.Infrastructure.Test.Repositories.Security
{
    [TestFixture]
    public class RoleModulePermissionRepositoryTest
    {
        private Mock<IRoleModulePermissionRepository> _mock;

        [SetUp]
        public void Setup()
        {
            _mock = new Mock<IRoleModulePermissionRepository>();
        }

        [Test]
        public async Task CreateAsync_ShouldReturnCreatedRoleModulePermissions()
        {
            // Arrange
            var roleModulePermission = new RoleModulePermissions { RoleId = 1, ModuleId = 1, PermissionId = 1, IsActive = true };

            _mock.Setup(repo => repo.CreateAsync(It.IsAny<RoleModulePermissions>()))
                .ReturnsAsync((RoleModulePermissions perm) => perm);

            // Act
            var result = await _mock.Object.CreateAsync(roleModulePermission);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.RoleId);
        }

        [Test]
        public async Task DeactivateAsync_ShouldReturnTrue()
        {
            // Arrange
            var roleModulePermission = new RoleModulePermissions { RoleId = 1, ModuleId = 1, PermissionId = 1 };

            _mock.Setup(repo => repo.DeactivateAsync(roleModulePermission))
                .ReturnsAsync(true);

            // Act
            var result = await _mock.Object.DeactivateAsync(roleModulePermission);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task GetAllActiveAsync_ShouldReturnActiveRoleModulePermissions()
        {
            // Arrange
            var activeRoleModulePermissions = new List<RoleModulePermissions>
            {
                new RoleModulePermissions { RoleId = 1, ModuleId = 1, PermissionId = 1, IsActive = true },
                new RoleModulePermissions { RoleId = 2, ModuleId = 2, PermissionId = 2, IsActive = true }
            };

            _mock.Setup(repo => repo.GetAllActiveAsync())
                .ReturnsAsync(activeRoleModulePermissions);

            // Act
            var result = await _mock.Object.GetAllActiveAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsTrue(result.All(rmp => rmp.IsActive));
        }

        [Test]
        public async Task GetAllAsync_WithPredicate_ShouldReturnFilteredRoleModulePermissions()
        {
            // Arrange
            var roleModulePermissions = new List<RoleModulePermissions>
            {
                new RoleModulePermissions { RoleId = 1, ModuleId = 1, PermissionId = 1, IsActive = true },
                new RoleModulePermissions { RoleId = 2, ModuleId = 2, PermissionId = 2, IsActive = false },
                new RoleModulePermissions { RoleId = 3, ModuleId = 3, PermissionId = 3, IsActive = false }
            };

            Expression<Func<RoleModulePermissions, bool>> predicate = rmp => rmp.IsActive;
            var expectedResults = roleModulePermissions.Where(predicate.Compile()).ToList();

            _mock.Setup(repo => repo.GetAllAsync(predicate))
                .ReturnsAsync(expectedResults);

            // Act
            var result = await _mock.Object.GetAllAsync(predicate);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(expectedResults.Count, result.Count);
            Assert.IsTrue(result.All(rmp => rmp.IsActive));
        }

        [Test]
        public async Task GetByRoleIdModuleIdPermissionsIdAsync_ShouldReturnCorrectRoleModulePermissions()
        {
            // Arrange
            var roleModulePermission = new RoleModulePermissions { RoleId = 1, ModuleId = 1, PermissionId = 1, IsActive = true };

            _mock.Setup(repo => repo.GetByRoleIdModuleIdPermissionsIdAsync(roleModulePermission))
                .ReturnsAsync(roleModulePermission);

            // Act
            var result = await _mock.Object.GetByRoleIdModuleIdPermissionsIdAsync(roleModulePermission);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.RoleId);
        }

        [Test]
        public async Task GetByRoleIdModuleIdPermissionsIdAsync_ShouldReturnNullForNonExistentRoleModulePermissions()
        {
            // Arrange
            var roleModulePermission = new RoleModulePermissions { RoleId = 99, ModuleId = 99, PermissionId = 99 };

            _mock.Setup(repo => repo.GetByRoleIdModuleIdPermissionsIdAsync(roleModulePermission))
                .ReturnsAsync((RoleModulePermissions)null);

            // Act
            var result = await _mock.Object.GetByRoleIdModuleIdPermissionsIdAsync(roleModulePermission);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task UpdateAsync_ShouldReturnUpdatedRoleModulePermissions()
        {
            // Arrange
            var roleModulePermission = new RoleModulePermissions { RoleId = 1, ModuleId = 1, PermissionId = 1, IsActive = true };

            _mock.Setup(repo => repo.UpdateAsync(It.IsAny<RoleModulePermissions>()))
                .ReturnsAsync((RoleModulePermissions perm) => perm);

            // Act
            var result = await _mock.Object.UpdateAsync(roleModulePermission);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.RoleId);
        }

        [Test]
        public async Task UpdateAsync_ShouldReturnNullForNonExistentRoleModulePermissions()
        {
            // Arrange
            var roleModulePermission = new RoleModulePermissions { RoleId = 99, ModuleId = 99, PermissionId = 99 };

            _mock.Setup(repo => repo.UpdateAsync(roleModulePermission))
                .ReturnsAsync((RoleModulePermissions)null);

            // Act
            var result = await _mock.Object.UpdateAsync(roleModulePermission);

            // Assert
            Assert.IsNull(result);
        }
    }
}