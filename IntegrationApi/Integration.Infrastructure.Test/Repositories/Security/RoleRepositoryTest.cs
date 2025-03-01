using Integration.Core.Entities.Security;
using Integration.Infrastructure.Interfaces.Security;
using Moq;
using System.Linq.Expressions;
namespace Integration.Infrastructure.Test.Repositories.Security
{
    public class RoleRepositoryTest
    {
        private Mock<IRoleRepository> _mock;
        [SetUp]
        public void Setup()
        {
            _mock = new Mock<IRoleRepository>();
        }
        [Test]
        public async Task GetByIdAsync_ShouldReturnRole()
        {
            // Arrange
            var Role = new Role
            {
                Id = 1,
                Code = "ROL0000001",
                Name = "Administrador",
                ApplicationId = 1,
                CreatedBy = "System",
                CreatedAt = DateTime.Now,
                IsActive = true
            };
            _mock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(Role);
            // Act
            var result = await _mock.Object.GetByIdAsync(1);
            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.Id);
        }
        [Test]
        public async Task GetAllAsync_WithPredicate_ShouldReturnFilteredRoles()
        {
            // Arrange
            var Roles = new List<Role>
            {
                new Role { Id = 1, Code = "ROL0000001", Name = "Administrador", ApplicationId = 1, CreatedBy = "System", CreatedAt = DateTime.Now, IsActive = true},
                new Role { Id = 2, Code = "ROL0000002", Name = "Contratista", ApplicationId = 1, CreatedBy = "System", CreatedAt = DateTime.Now, IsActive = true}
            };
            Expression<Func<Role, bool>> predicate = app => app.IsActive;
            _mock.Setup(repo => repo.GetAllAsync(predicate))
                .ReturnsAsync(Roles.Where(predicate.Compile()).ToList());
            // Act
            var result = await _mock.Object.GetAllAsync(predicate);
            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.First().IsActive);
        }
        [Test]
        public async Task GetAllAsync_WithPredicates_ShouldReturnFilteredRoles()
        {
            // Arrange
            var Roles = new List<Role>
            {
                 new Role { Id = 1, Code = "ROL0000001", Name = "Administrador", ApplicationId = 1, CreatedBy = "System", CreatedAt = DateTime.Now, IsActive = true},
                new Role { Id = 2, Code = "ROL0000002", Name = "Contratista", ApplicationId = 1, CreatedBy = "System", CreatedAt = DateTime.Now, IsActive = true}
            };
            var predicates = new List<Expression<Func<Role, bool>>>
            {
                app => app.IsActive,
                app => app.Code.Contains("ROL000000")
            };
            _mock.Setup(repo => repo.GetAllAsync(predicates))
                .ReturnsAsync(Roles.Where(app => predicates.All(p => p.Compile()(app))).ToList());
            // Act
            var result = await _mock.Object.GetAllAsync(predicates);
            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.First().IsActive);
        }
        [Test]
        public async Task CreateAsync_ShouldReturnCreatedRole()
        {
            // Arrange
            var Role = new Role { Id = 1, Code = "ROL0000001", Name = "Administrador", ApplicationId = 1, CreatedBy = "System", CreatedAt = DateTime.Now, IsActive = true };
            _mock.Setup(repo => repo.CreateAsync(Role))
                .ReturnsAsync(Role);
            // Act
            var result = await _mock.Object.CreateAsync(Role);
            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.Id);
        }
        [Test]
        public async Task UpdateAsync_ShouldReturnUpdatedRole()
        {
            // Arrange
            var Role = new Role { Id = 1, Code = "ROL0000001", Name = "Administrador", ApplicationId = 1, CreatedBy = "System", CreatedAt = DateTime.Now, IsActive = true };
            _mock.Setup(repo => repo.UpdateAsync(Role)).ReturnsAsync(Role);
            // Act
            var result = await _mock.Object.UpdateAsync(Role);
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
        public async Task GetAllActiveAsync_ShouldReturnActiveRoles()
        {
            // Arrange
            var activeRoles = new List<Role>
            {
                new Role { Id = 1, Code = "ROL0000001", Name = "Administrador", ApplicationId = 1, CreatedBy = "System", CreatedAt = DateTime.Now, IsActive = true},
                new Role { Id = 2, Code = "ROL0000002", Name = "Contratista", ApplicationId = 1, CreatedBy = "System", CreatedAt = DateTime.Now, IsActive = true}
            };
            _mock.Setup(repo => repo.GetAllActiveAsync()).ReturnsAsync(activeRoles);
            // Act
            var result = await _mock.Object.GetAllActiveAsync();
            // Assert
            Assert.NotNull(result);
            Assert.IsTrue(result.All(app => app.IsActive));
        }
    }
}
