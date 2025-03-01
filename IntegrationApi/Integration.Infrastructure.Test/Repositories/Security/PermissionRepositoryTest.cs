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
        public async Task GetByIdAsync_ShouldReturnPermission()
        {
            // Arrange
            var Permission = new Permission
            {
                Id = 1,
                Code = "PER0000001",
                Name = "Consultar Aplicacion",
                IsActive = true
            };
            _mock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(Permission);
            // Act
            var result = await _mock.Object.GetByIdAsync(1);
            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.Id);
        }
        [Test]
        public async Task GetAllAsync_WithPredicate_ShouldReturnFilteredPermissions()
        {
            // Arrange
            var Permissions = new List<Permission>
            {
                new Permission { Id = 1, Code = "PER0000001", Name = "Consultar Aplicacion", IsActive = true },
                new Permission { Id = 2, Code = "PER0000002", Name = "Crear Aplicacion", IsActive = true }
            };
            Expression<Func<Permission, bool>> predicate = app => app.IsActive;
            _mock.Setup(repo => repo.GetAllAsync(predicate))
                .ReturnsAsync(Permissions.Where(predicate.Compile()).ToList());
            // Act
            var result = await _mock.Object.GetAllAsync(predicate);
            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.First().IsActive);
        }
        [Test]
        public async Task GetAllAsync_WithPredicates_ShouldReturnFilteredPermissions()
        {
            // Arrange
            var Permissions = new List<Permission>
            {
                 new Permission { Id = 1, Code = "PER0000001", Name = "Consultar Aplicacion", IsActive = true },
                new Permission { Id = 2, Code = "PER0000002", Name = "Crear Aplicacion", IsActive = true }
            };
            var predicates = new List<Expression<Func<Permission, bool>>>
            {
                app => app.IsActive,
                app => app.Name.Contains("Aplicacion")
            };
            _mock.Setup(repo => repo.GetAllAsync(predicates))
                .ReturnsAsync(Permissions.Where(app => predicates.All(p => p.Compile()(app))).ToList());
            // Act
            var result = await _mock.Object.GetAllAsync(predicates);
            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.First().IsActive);
        }
        [Test]
        public async Task CreateAsync_ShouldReturnCreatedPermission()
        {
            // Arrange
            var Permission = new Permission { Id = 1, Code = "PER0000001", Name = "Consultar Aplicacion", IsActive = true };
            _mock.Setup(repo => repo.CreateAsync(Permission))
                .ReturnsAsync(Permission);
            // Act
            var result = await _mock.Object.CreateAsync(Permission);
            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.Id);
        }
        [Test]
        public async Task UpdateAsync_ShouldReturnUpdatedPermission()
        {
            // Arrange
            var Permission = new Permission { Id = 1, Code = "PER0000001", Name = "Consultar Aplicacion", IsActive = false };
            _mock.Setup(repo => repo.UpdateAsync(Permission)).ReturnsAsync(Permission);
            // Act
            var result = await _mock.Object.UpdateAsync(Permission);
            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.Id);
        }
        [Test]
        public async Task DeleteAsync_ShouldReturnTrue()
        {
            // Arrange
            _mock.Setup(repo => repo.DeleteAsync(1)).ReturnsAsync(true);
            // Act
            var result = await _mock.Object.DeleteAsync(1);
            // Assert
            Assert.IsTrue(result);
        }
        [Test]
        public async Task GetAllActiveAsync_ShouldReturnActivePermissions()
        {
            // Arrange
            var activePermissions = new List<Permission>
            {
                new Permission { Id = 1, Code = "PER0000001", Name = "Consultar Aplicacion", IsActive = true },
                new Permission { Id = 2, Code = "PER0000002", Name = "Crear Aplicacion", IsActive = true }
            };
            _mock.Setup(repo => repo.GetAllActiveAsync()).ReturnsAsync(activePermissions);
            // Act
            var result = await _mock.Object.GetAllActiveAsync();
            // Assert
            Assert.NotNull(result);
            Assert.IsTrue(result.All(app => app.IsActive));
        }
    }
}
