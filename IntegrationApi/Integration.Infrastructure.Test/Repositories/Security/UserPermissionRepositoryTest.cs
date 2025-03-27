using Integration.Infrastructure.Interfaces.Security;
using Integration.Shared.DTO.Security;

using Moq;

namespace Integration.Infrastructure.Test.Repositories.Security
{
    [TestFixture]
    public class UserPermissionRepositoryTest
    {
        private Mock<IUserPermissionRepository> _mock;

        [SetUp]
        public void SetUp()
        {
            _mock = new Mock<IUserPermissionRepository>();
        }

        [Test]
        public async Task GetAllActiveByUserIdAsync_ShouldReturnPermissions_WhenUserHasPermissions()
        {
            // Arrange
            var expectedPermissions = new List<UserPermissionDTOResponse>
            {
                new UserPermissionDTOResponse
                {
                    CodeUser = "USR0000001",
                    UserName = "testuser",
                    CodeRole = "ROL0000001",
                    Role = "Admin",
                    CodeModule = "MOD0000001",
                    Module = "Module1",
                    CodePermission = "PERM0000001",
                    Permission = "Permission1"
                }
            };

            _mock.Setup(repo => repo.GetAllActiveByUserIdAsync("USR0000001", 1))
                 .ReturnsAsync(expectedPermissions);

            // Act
            var result = await _mock.Object.GetAllActiveByUserIdAsync("USR0000001", 1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            var permission = result.First();
            Assert.AreEqual("USR0000001", permission.CodeUser);
            Assert.AreEqual("testuser", permission.UserName);
            Assert.AreEqual("ROL0000001", permission.CodeRole);
            Assert.AreEqual("Admin", permission.Role);
            Assert.AreEqual("MOD0000001", permission.CodeModule);
            Assert.AreEqual("Module1", permission.Module);
            Assert.AreEqual("PERM0000001", permission.CodePermission);
            Assert.AreEqual("Permission1", permission.Permission);
        }

        [Test]
        public async Task GetAllActiveByUserIdAsync_ShouldReturnEmpty_WhenUserHasNoPermissions()
        {
            // Arrange
            _mock.Setup(repo => repo.GetAllActiveByUserIdAsync("USR_NO_PERMS", 1))
                 .ReturnsAsync(new List<UserPermissionDTOResponse>());

            // Act
            var result = await _mock.Object.GetAllActiveByUserIdAsync("USR_NO_PERMS", 1);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetAllActiveByUserIdAsync_ShouldThrowException_WhenCalledWithNull()
        {
            // Arrange
            _mock.Setup(repo => repo.GetAllActiveByUserIdAsync(null, 1))
                 .ThrowsAsync(new ArgumentNullException("userCode"));

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentNullException>(() => _mock.Object.GetAllActiveByUserIdAsync(null, 1));
            Assert.That(ex.ParamName, Is.EqualTo("userCode"));
        }
    }
}