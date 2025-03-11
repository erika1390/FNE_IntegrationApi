using Integration.Core.Entities.Security;
using Integration.Infrastructure.Interfaces.Security;
using Moq;
using System.Linq.Expressions;
namespace Integration.Infrastructure.Test.Repositories.Security
{
    [TestFixture]
    public class ModuleRepositoryTest
    {
        private Mock<IModuleRepository> _mock;
        [SetUp]
        public void Setup()
        {
            _mock = new Mock<IModuleRepository>();
        }
        [Test]
        public async Task GetAllAsync_WithPredicate_ShouldReturnFilteredModules()
        {
            // Arrange
            var modules = new List<Module>
            {
                new Module { Id = 1, Code = "MOD0000001", Name = "Aplicaciones", IsActive = true },
                new Module { Id = 2, Code = "MOD0000002", Name = "Modulos", IsActive = false }
            };
            Expression<Func<Module, bool>> predicate = app => app.IsActive;
            _mock.Setup(repo => repo.GetAllAsync(predicate))
                .ReturnsAsync(modules.Where(predicate.Compile()).ToList());
            // Act
            var result = await _mock.Object.GetAllAsync(predicate);
            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result.First().IsActive);
        }
        [Test]
        public async Task GetAllAsync_WithPredicates_ShouldReturnFilteredModules()
        {
            // Arrange
            var modules = new List<Module>
            {
                new Module { Id = 1, Code = "MOD0000001", Name = "Aplicaciones", IsActive = true },
                new Module { Id = 2, Code = "MOD0000002", Name = "Modulos", IsActive = false }
            };
            var predicates = new List<Expression<Func<Module, bool>>>
            {
                app => app.IsActive,
                app => app.Name.Contains("Aplicaciones")
            };
            _mock.Setup(repo => repo.GetAllAsync(predicates))
                .ReturnsAsync(modules.Where(app => predicates.All(p => p.Compile()(app))).ToList());
            // Act
            var result = await _mock.Object.GetAllAsync(predicates);
            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result.First().IsActive);
        }
        [Test]
        public async Task CreateAsync_ShouldReturnCreatedModule()
        {
            // Arrange
            var module = new Module { Id = 1, Code = "MOD0000001", Name = "Aplicaciones", IsActive = true };
            _mock.Setup(repo => repo.CreateAsync(module))
                .ReturnsAsync(module);
            // Act
            var result = await _mock.Object.CreateAsync(module);
            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.Id);
        }
        [Test]
        public async Task UpdateAsync_ShouldReturnUpdatedModule()
        {
            // Arrange
            var module = new Module { Id = 1, Code = "MOD0000001", Name = "Aplicaciones", IsActive = true };
            _mock.Setup(repo => repo.UpdateAsync(module)).ReturnsAsync(module);
            // Act
            var result = await _mock.Object.UpdateAsync(module);
            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.Id);
        }
        [Test]
        public async Task DeleteAsync_ShouldReturnTrue()
        {
            // Arrange
            _mock.Setup(repo => repo.DeleteAsync("MOD0000001")).ReturnsAsync(true);
            // Act
            var result = await _mock.Object.DeleteAsync("MOD0000001");
            // Assert
            Assert.IsTrue(result);
        }
        [Test]
        public async Task GetAllActiveAsync_ShouldReturnActiveModules()
        {
            // Arrange
            var activeModules = new List<Module>
            {
                new Module { Id = 1, Code = "MOD0000001", Name = "Aplicaciones", IsActive = true },
                new Module { Id = 2, Code = "MOD0000002", Name = "Modulos", IsActive = true }
            };
            _mock.Setup(repo => repo.GetAllActiveAsync()).ReturnsAsync(activeModules);
            // Act
            var result = await _mock.Object.GetAllActiveAsync();
            // Assert
            Assert.NotNull(result);
            Assert.IsTrue(result.All(app => app.IsActive));
        }
    }
}