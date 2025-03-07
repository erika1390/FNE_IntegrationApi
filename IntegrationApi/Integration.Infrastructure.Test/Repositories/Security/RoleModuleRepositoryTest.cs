using Integration.Core.Entities.Security;
using Integration.Infrastructure.Interfaces.Security;
using Moq;
using System.Linq.Expressions;
namespace Integration.Infrastructure.Test.Repositories.Security
{
    [TestFixture]
    public class RoleModuleRepositoryTest
    {
        private Mock<IRoleModuleRepository> _mock;
        [SetUp]
        public void Setup()
        {
            _mock = new Mock<IRoleModuleRepository>();
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnRoleModule()
        {
            // Arrange
            var roleModule = new RoleModule
            {
                Id = 1,
                RoleId = 1,
                ModuleId = 1,
                PermissionId = 1,
                IsActive = true
            };
            _mock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(roleModule);
            // Act
            var result = await _mock.Object.GetByIdAsync(1);
            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.Id);
        }
        [Test]
        public async Task GetAllAsync_WithPredicate_ShouldReturnFilteredRoleModules()
        {
            // Arrange
            var roleModules = new List<RoleModule>
            {
                new RoleModule { 
                    Id = 1,
                    RoleId = 1,
                    ModuleId = 1,
                    PermissionId = 1,
                    IsActive = true 
                },
                new RoleModule {
                    Id = 1,
                    RoleId = 1,
                    ModuleId = 1,
                    PermissionId = 2,
                    IsActive = true
                }
            };
            Expression<Func<RoleModule, bool>> predicate = app => app.IsActive;
            _mock.Setup(repo => repo.GetAllAsync(predicate))
                .ReturnsAsync(roleModules.Where(predicate.Compile()).ToList());
            // Act
            var result = await _mock.Object.GetAllAsync(predicate);
            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.First().IsActive);
        }
        [Test]
        public async Task GetAllAsync_WithPredicates_ShouldReturnFilteredRoleModules()
        {
            // Arrange
            var roleModules = new List<RoleModule>
            {
                new RoleModule {
                    Id = 1,
                    RoleId = 1,
                    ModuleId = 1,
                    PermissionId = 1,
                    IsActive = true
                },
                new RoleModule {
                    Id = 1,
                    RoleId = 1,
                    ModuleId = 1,
                    PermissionId = 2,
                    IsActive = true
                }
            };
            var predicates = new List<Expression<Func<RoleModule, bool>>>
            {
                app => app.IsActive,
                app => app.RoleId.Equals(1)
            };
            _mock.Setup(repo => repo.GetAllAsync(predicates))
                .ReturnsAsync(roleModules.Where(app => predicates.All(p => p.Compile()(app))).ToList());
            // Act
            var result = await _mock.Object.GetAllAsync(predicates);
            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.First().IsActive);
        }
        [Test]
        public async Task CreateAsync_ShouldReturnCreatedRoleModule()
        {
            // Arrange
            var roleModule = new RoleModule {
                Id = 1,
                RoleId = 1,
                ModuleId = 1,
                PermissionId = 1,
                IsActive = true
            };
            _mock.Setup(repo => repo.CreateAsync(roleModule))
                .ReturnsAsync(roleModule);
            // Act
            var result = await _mock.Object.CreateAsync(roleModule);
            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.Id);
        }
        [Test]
        public async Task UpdateAsync_ShouldReturnUpdatedRoleModule()
        {
            // Arrange
            var roleModule = new RoleModule {
                Id = 1,
                RoleId = 1,
                ModuleId = 1,
                PermissionId = 1,
                IsActive = true
            };
            _mock.Setup(repo => repo.UpdateAsync(roleModule)).ReturnsAsync(roleModule);
            // Act
            var result = await _mock.Object.UpdateAsync(roleModule);
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
        public async Task GetAllActiveAsync_ShouldReturnActiveRoleModules()
        {
            // Arrange
            var activeRoleModules = new List<RoleModule>
            {
                new RoleModule {
                    Id = 1,
                    RoleId = 1,
                    ModuleId = 1,
                    PermissionId = 1,
                    IsActive = true
                },
                new RoleModule {
                    Id = 1,
                    RoleId = 1,
                    ModuleId = 1,
                    PermissionId = 2,
                    IsActive = true
                }
            };
            _mock.Setup(repo => repo.GetAllActiveAsync()).ReturnsAsync(activeRoleModules);
            // Act
            var result = await _mock.Object.GetAllActiveAsync();
            // Assert
            Assert.NotNull(result);
            Assert.IsTrue(result.All(app => app.IsActive));
        }
    }
}