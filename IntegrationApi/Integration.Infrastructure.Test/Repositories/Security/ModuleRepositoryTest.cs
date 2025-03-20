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
        public async Task GetByCodeAsync_ShouldReturnCorrectModule()
        {
            // Arrange
            var module = new Module { Id = 1, Code = "MOD0000001", Name = "Aplicaciones", IsActive = true };
            _mock.Setup(repo => repo.GetByCodeAsync("MOD0000001"))
                .ReturnsAsync(module);

            // Act
            var result = await _mock.Object.GetByCodeAsync("MOD0000001");

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("MOD0000001", result.Code);
            Assert.AreEqual("Aplicaciones", result.Name);
        }

        [Test]
        public async Task GetByCodeAsync_ShouldReturnNullForNonExistentCode()
        {
            // Arrange
            _mock.Setup(repo => repo.GetByCodeAsync("INVALID_CODE"))
                .ReturnsAsync((Module)null);

            // Act
            var result = await _mock.Object.GetByCodeAsync("INVALID_CODE");

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetAllAsync_WithPredicate_ShouldReturnFilteredModules()
        {
            // Arrange
            var modules = new List<Module>
            {
                new Module { Id = 1, Code = "MOD0000001", Name = "Aplicaciones", IsActive = true },
                new Module { Id = 2, Code = "MOD0000002", Name = "Roles", IsActive = false },
                new Module { Id = 3, Code = "MOD0000003", Name = "Modulos", IsActive = false }
            };

            Expression<Func<Module, bool>> predicate = mod => mod.IsActive;
            var expectedResults = modules.Where(predicate.Compile()).ToList();

            _mock.Setup(repo => repo.GetByFilterAsync(predicate))
                .ReturnsAsync(expectedResults);

            // Act
            var result = await _mock.Object.GetByFilterAsync(predicate);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(expectedResults.Count, result.Count);
            Assert.IsTrue(result.All(mod => mod.IsActive));
        }

        [Test]
        public async Task GetAllAsync_WithPredicates_ShouldReturnFilteredModules()
        {
            // Arrange
            var modules = new List<Module>
            {
                new Module { Id = 1, Code = "MOD0000001", Name = "Aplicaciones", IsActive = true },
                new Module { Id = 2, Code = "MOD0000002", Name = "Roles", IsActive = false },
                new Module { Id = 3, Code = "MOD0000003", Name = "Modulos", IsActive = false }
            };

            var predicates = new List<Expression<Func<Module, bool>>>
            {
                mod => mod.IsActive,
                mod => mod.Code.Contains("MOD")
            };

            var expectedResults = modules.Where(mod => predicates.All(p => p.Compile()(mod))).ToList();

            _mock.Setup(repo => repo.GetByMultipleFiltersAsync(predicates))
                .ReturnsAsync(expectedResults);

            // Act
            var result = await _mock.Object.GetByMultipleFiltersAsync(predicates);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(expectedResults.Count, result.Count);
            Assert.IsTrue(result.All(mod => mod.IsActive));
        }

        [Test]
        public async Task UpdateAsync_ShouldReturnUpdatedModule()
        {
            // Arrange
            var module = new Module { Id = 1, Code = "MOD0000001", Name = "Aplicaciones", IsActive = true };

            _mock.Setup(repo => repo.UpdateAsync(It.IsAny<Module>()))
                .ReturnsAsync((Module mod) => mod);

            // Act
            var result = await _mock.Object.UpdateAsync(module);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("Aplicaciones", result.Name);
        }

        [Test]
        public async Task DeactivateAsync_ShouldReturnTrue()
        {
            // Arrange
            _mock.Setup(repo => repo.DeactivateAsync("MOD0000001", "epulido"))
                .ReturnsAsync(true);

            // Act
            var result = await _mock.Object.DeactivateAsync("MOD0000001", "epulido");

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
                new Module { Id = 2, Code = "MOD0000002", Name = "Roles", IsActive = true }
            };

            _mock.Setup(repo => repo.GetAllActiveAsync())
                .ReturnsAsync(activeModules);

            // Act
            var result = await _mock.Object.GetAllActiveAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsTrue(result.All(mod => mod.IsActive));
        }

        [Test]
        public async Task CreateAsync_ShouldReturnCreatedModule()
        {
            // Arrange
            var module = new Module { Id = 1, Code = "MOD0000001", Name = "Aplicaciones", IsActive = true };

            _mock.Setup(repo => repo.CreateAsync(It.IsAny<Module>()))
                .ReturnsAsync((Module mod) => mod);

            // Act
            var result = await _mock.Object.CreateAsync(module);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("MOD0000001", result.Code);
        }
    }
}