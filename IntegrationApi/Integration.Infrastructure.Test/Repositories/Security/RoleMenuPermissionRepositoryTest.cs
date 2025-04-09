using Integration.Core.Entities.Security;
using Integration.Infrastructure.Interfaces.Security;

using Moq;

using System.Linq.Expressions;

namespace Integration.Infrastructure.Test.Repositories.Security
{
    [TestFixture]
    public class RoleMenuPermissionRepositoryTest
    {
        private Mock<IRoleMenuPermissionRepository> _mock;

        [SetUp]
        public void Setup()
        {
            _mock = new Mock<IRoleMenuPermissionRepository>();
        }

        [Test]
        public async Task CreateAsync_ShouldReturnCreatedRoleMenuPermissions()
        {
            // Arrange
            var roleMenuPermission = new RoleMenuPermission { RoleId = 1, MenuId = 1, PermissionId = 1, IsActive = true };

            _mock.Setup(repo => repo.CreateAsync(It.IsAny<RoleMenuPermission>()))
                .ReturnsAsync((RoleMenuPermission perm) => perm);

            // Act
            var result = await _mock.Object.CreateAsync(roleMenuPermission);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.RoleId);
        }

        [Test]
        public async Task DeactivateAsync_ShouldReturnTrue()
        {
            // Arrange
            var roleMenuPermission = new RoleMenuPermission { RoleId = 1, MenuId = 1, PermissionId = 1 };

            _mock.Setup(repo => repo.DeactivateAsync(roleMenuPermission))
                .ReturnsAsync(true);

            // Act
            var result = await _mock.Object.DeactivateAsync(roleMenuPermission);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task GetAllActiveAsync_ShouldReturnActiveRoleMenuPermissions()
        {
            // Arrange
            var activeRoleMenuPermissions = new List<RoleMenuPermission>
            {
                new RoleMenuPermission { RoleId = 1, MenuId = 1, PermissionId = 1, IsActive = true },
                new RoleMenuPermission { RoleId = 2, MenuId = 2, PermissionId = 2, IsActive = true }
            };

            _mock.Setup(repo => repo.GetAllActiveAsync())
                .ReturnsAsync(activeRoleMenuPermissions);

            // Act
            var result = await _mock.Object.GetAllActiveAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsTrue(result.All(rmp => rmp.IsActive));
        }

        [Test]
        public async Task GetAllAsync_WithPredicate_ShouldReturnFilteredRoleMenuPermissions()
        {
            // Arrange
            var roleMenuPermissions = new List<RoleMenuPermission>
            {
                new RoleMenuPermission { RoleId = 1, MenuId = 1, PermissionId = 1, IsActive = true },
                new RoleMenuPermission { RoleId = 2, MenuId = 2, PermissionId = 2, IsActive = false },
                new RoleMenuPermission { RoleId = 3, MenuId = 3, PermissionId = 3, IsActive = false }
            };

            Expression<Func<RoleMenuPermission, bool>> predicate = rmp => rmp.IsActive;
            var expectedResults = roleMenuPermissions.Where(predicate.Compile()).ToList();

            _mock.Setup(repo => repo.GetByFilterAsync(predicate))
                .ReturnsAsync(expectedResults);

            // Act
            var result = await _mock.Object.GetByFilterAsync(predicate);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(expectedResults.Count, result.Count);
            Assert.IsTrue(result.All(rmp => rmp.IsActive));
        }

        [Test]
        public async Task GetByRoleIdMenuIdPermissionsIdAsync_ShouldReturnCorrectRoleMenuPermissions()
        {
            // Arrange
            var roleMenuPermission = new RoleMenuPermission { RoleId = 1, MenuId = 1, PermissionId = 1, IsActive = true };

            _mock.Setup(repo => repo.GetByRoleIdMenuIdPermissionsIdAsync(roleMenuPermission))
                .ReturnsAsync(roleMenuPermission);

            // Act
            var result = await _mock.Object.GetByRoleIdMenuIdPermissionsIdAsync(roleMenuPermission);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.RoleId);
        }

        [Test]
        public async Task GetByRoleIdMenuIdPermissionsIdAsync_ShouldReturnNullForNonExistentRoleMenuPermissions()
        {
            // Arrange
            var roleMenuPermission = new RoleMenuPermission { RoleId = 99, MenuId = 99, PermissionId = 99 };

            _mock.Setup(repo => repo.GetByRoleIdMenuIdPermissionsIdAsync(roleMenuPermission))
                .ReturnsAsync((RoleMenuPermission)null);

            // Act
            var result = await _mock.Object.GetByRoleIdMenuIdPermissionsIdAsync(roleMenuPermission);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task UpdateAsync_ShouldReturnUpdatedRoleMenuPermissions()
        {
            // Arrange
            var roleMenuPermission = new RoleMenuPermission { RoleId = 1, MenuId = 1, PermissionId = 1, IsActive = true };

            _mock.Setup(repo => repo.UpdateAsync(It.IsAny<RoleMenuPermission>()))
                .ReturnsAsync((RoleMenuPermission perm) => perm);

            // Act
            var result = await _mock.Object.UpdateAsync(roleMenuPermission);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.RoleId);
        }

        [Test]
        public async Task UpdateAsync_ShouldReturnNullForNonExistentRoleMenuPermissions()
        {
            // Arrange
            var roleMenuPermission = new RoleMenuPermission { RoleId = 99, MenuId = 99, PermissionId = 99 };

            _mock.Setup(repo => repo.UpdateAsync(roleMenuPermission))
                .ReturnsAsync((RoleMenuPermission)null);

            // Act
            var result = await _mock.Object.UpdateAsync(roleMenuPermission);

            // Assert
            Assert.IsNull(result);
        }
    }
}