using Integration.Core.Entities.Security;
using Integration.Infrastructure.Interfaces.Security;
using Moq;
using System.Linq.Expressions;
namespace Integration.Infrastructure.Test.Repositories.Security
{
    [TestFixture]
    public class ApplicationRepositoryTest
    {
        private Mock<IApplicationRepository> _mock;

        [SetUp]
        public void Setup()
        {
            _mock = new Mock<IApplicationRepository>();
        }

        [Test]
        public async Task GetByCodeAsync_ShouldReturnCorrectApplication()
        {
            // Arrange
            var application = new Integration.Core.Entities.Security.Application { Id = 1, Code = "APP0000001", Name = "Saga 2.0", IsActive = true };
            _mock.Setup(repo => repo.GetByCodeAsync("APP0000001"))
                .ReturnsAsync(application);

            // Act
            var result = await _mock.Object.GetByCodeAsync("APP0000001");

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("APP0000001", result.Code);
            Assert.AreEqual("Saga 2.0", result.Name);
        }

        [Test]
        public async Task GetByCodeAsync_ShouldReturnNullForNonExistentCode()
        {
            // Arrange
            _mock.Setup(repo => repo.GetByCodeAsync("INVALID_CODE"))
                .ReturnsAsync((Integration.Core.Entities.Security.Application)null);

            // Act
            var result = await _mock.Object.GetByCodeAsync("INVALID_CODE");

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetAllAsync_WithPredicate_ShouldReturnFilteredApplications()
        {
            // Arrange
            var applications = new List<Integration.Core.Entities.Security.Application>
            {
                new Integration.Core.Entities.Security.Application { Id = 1, Code = "APP0000001", Name = "Integration", IsActive = true },
                new Integration.Core.Entities.Security.Application { Id = 2, Code = "APP0000002", Name = "Saga 2.0", IsActive = false },
                new Integration.Core.Entities.Security.Application { Id = 3, Code = "APP0000003", Name = "Sicof Lite", IsActive = false }
            };

            Expression<Func<Integration.Core.Entities.Security.Application, bool>> predicate = app => app.IsActive;
            var expectedResults = applications.Where(predicate.Compile()).ToList();

            _mock.Setup(repo => repo.GetByFilterAsync(predicate))
                .ReturnsAsync(expectedResults);

            // Act
            var result = await _mock.Object.GetByFilterAsync(predicate);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(expectedResults.Count, result.Count);
            Assert.IsTrue(result.All(app => app.IsActive));
        }

        [Test]
        public async Task GetAllAsync_WithPredicates_ShouldReturnFilteredApplications()
        {
            // Arrange
            var applications = new List<Integration.Core.Entities.Security.Application>
            {
                new Integration.Core.Entities.Security.Application { Id = 1, Code = "APP0000001", Name = "Integration", IsActive = true },
                new Integration.Core.Entities.Security.Application { Id = 2, Code = "APP0000002", Name = "Saga 2.0", IsActive = false },
                new Integration.Core.Entities.Security.Application { Id = 3, Code = "APP0000003", Name = "Sicof Lite", IsActive = true }
            };

            var predicates = new List<Expression<Func<Integration.Core.Entities.Security.Application, bool>>>
            {
                app => app.IsActive,
                app => app.Code.Contains("APP")
            };

            var expectedResults = applications.Where(app => predicates.All(p => p.Compile()(app))).ToList();

            _mock.Setup(repo => repo.GetByMultipleFiltersAsync(predicates))
                .ReturnsAsync(expectedResults);

            // Act
            var result = await _mock.Object.GetByMultipleFiltersAsync(predicates);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(expectedResults.Count, result.Count);
            Assert.IsTrue(result.All(app => app.IsActive));
        }

        [Test]
        public async Task UpdateAsync_ShouldReturnUpdatedApplication()
        {
            // Arrange
            var application = new Integration.Core.Entities.Security.Application { Id = 1, Code = "APP0000001", Name = "Updated Name", IsActive = true };

            _mock.Setup(repo => repo.UpdateAsync(It.IsAny<Integration.Core.Entities.Security.Application>()))
                .ReturnsAsync((Integration.Core.Entities.Security.Application app) => app);

            // Act
            var result = await _mock.Object.UpdateAsync(application);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("Updated Name", result.Name);
        }

        [Test]
        public async Task DeactivateAsync_ShouldReturnTrue()
        {
            // Arrange
            _mock.Setup(repo => repo.DeactivateAsync("APP0000001","epulido"))
                .ReturnsAsync(true);

            // Act
            var result = await _mock.Object.DeactivateAsync("APP0000001", "epulido");

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task GetAllActiveAsync_ShouldReturnActiveApplications()
        {
            // Arrange
            var activeApplications = new List<Integration.Core.Entities.Security.Application>
            {
                new Integration.Core.Entities.Security.Application { Id = 1, Code = "APP0000001", Name = "Saga 2.0", IsActive = true },
                new Integration.Core.Entities.Security.Application { Id = 2, Code = "APP0000002", Name = "Sicof Lite", IsActive = true }
            };

            _mock.Setup(repo => repo.GetAllActiveAsync())
                .ReturnsAsync(activeApplications);

            // Act
            var result = await _mock.Object.GetAllActiveAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsTrue(result.All(app => app.IsActive));
        }

        [Test]
        public async Task CreateAsync_ShouldReturnCreatedApplication()
        {
            // Arrange
            var application = new Integration.Core.Entities.Security.Application { Id = 1, Code = "APP0000001", Name = "Integration", IsActive = true };

            _mock.Setup(repo => repo.CreateAsync(It.IsAny<Integration.Core.Entities.Security.Application>()))
                .ReturnsAsync((Integration.Core.Entities.Security.Application app) => app);

            // Act
            var result = await _mock.Object.CreateAsync(application);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("APP0000001", result.Code);
        }
    }
}