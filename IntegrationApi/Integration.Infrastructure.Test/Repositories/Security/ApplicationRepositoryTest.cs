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
        public async Task GetAllAsync_WithPredicate_ShouldReturnFilteredApplications()
        {
            // Arrange
            var applications = new List<Application>
            {
                new Application { Id = 1, Code = "APP0000001", Name = "Saga 2.0", IsActive = true },
                new Application { Id = 2, Code = "APP0000002", Name = "Sicof Lite", IsActive = false }
            };
            Expression<Func<Application, bool>> predicate = app => app.IsActive;
            _mock.Setup(repo => repo.GetAllAsync(predicate))
                .ReturnsAsync(applications.Where(predicate.Compile()).ToList());
            // Act
            var result = await _mock.Object.GetAllAsync(predicate);
            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result.First().IsActive);
        }
        [Test]
        public async Task GetAllAsync_WithPredicates_ShouldReturnFilteredApplications()
        {
            // Arrange
            var applications = new List<Application>
            {
                new Application { Id = 1, Code = "APP0000001", Name = "Saga 2.0", IsActive = true },
                new Application { Id = 2, Code = "APP0000002", Name = "Sicof Lite", IsActive = false }
            };
            var predicates = new List<Expression<Func<Application, bool>>>
            {
                app => app.IsActive,
                app => app.Name.Contains("Saga")
            };
            _mock.Setup(repo => repo.GetAllAsync(predicates))
                .ReturnsAsync(applications.Where(app => predicates.All(p => p.Compile()(app))).ToList());
            // Act
            var result = await _mock.Object.GetAllAsync(predicates);
            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result.First().IsActive);
        }
        [Test]
        public async Task CreateAsync_ShouldReturnCreatedApplication()
        {
            // Arrange
            var application = new Application { Id = 1, Code = "APP0000001", Name = "Saga 2.0", IsActive = true };
            _mock.Setup(repo => repo.CreateAsync(application))
                .ReturnsAsync(application);
            // Act
            var result = await _mock.Object.CreateAsync(application);
            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.Id);
        }
        [Test]
        public async Task UpdateAsync_ShouldReturnUpdatedApplication()
        {
            // Arrange
            var application = new Application { Id = 1, Code = "APP0000001", Name = "Saga 2.0", IsActive = true };
            _mock.Setup(repo => repo.UpdateAsync(application))
                .ReturnsAsync(application);
            // Act
            var result = await _mock.Object.UpdateAsync(application);
            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.Id);
        }
        [Test]
        public async Task DeleteAsync_ShouldReturnTrue()
        {
            // Arrange
            _mock.Setup(repo => repo.DeleteAsync("APP0000001"))
                .ReturnsAsync(true);
            // Act
            var result = await _mock.Object.DeleteAsync("APP0000001");
            // Assert
            Assert.IsTrue(result);
        }
        [Test]
        public async Task GetAllActiveAsync_ShouldReturnActiveApplications()
        {
            // Arrange
            var activeApplications = new List<Application>
            {
                new Application { Id = 1, Code = "APP0000001", Name = "Saga 2.0", IsActive = true },
                new Application { Id = 2, Code = "APP0000002", Name = "Sicof Lite", IsActive = true }
            };
            _mock.Setup(repo => repo.GetAllActiveAsync())
                .ReturnsAsync(activeApplications);
            // Act
            var result = await _mock.Object.GetAllActiveAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsTrue(result.All(app => app.IsActive));
        }
    }
}