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
            var application = new Application { Id = 1, Code = "APP0000001", Name = "Saga 2.0", IsActive = true };
            _mock.Setup(repo => repo.GetByCodeAsync("APP0000001"))
                .ReturnsAsync(application);

            // Act
            var result = await _mock.Object.GetByCodeAsync("APP0000001");

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("APP0000001", result.Code);
        }

        [Test]
        public async Task GetByCodeAsync_ShouldReturnNullForNonExistentCode()
        {
            // Arrange
            _mock.Setup(repo => repo.GetByCodeAsync("INVALID_CODE"))
                .ReturnsAsync((Application)null);

            // Act
            var result = await _mock.Object.GetByCodeAsync("INVALID_CODE");

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllApplications()
        {
            // Arrange
            var applications = new List<Application>
            {
                new Application { Id = 1, Code = "APP0000001", Name = "Saga 2.0", IsActive = true },
                new Application { Id = 2, Code = "APP0000002", Name = "Sicof Lite", IsActive = false }
            };
            _mock.Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Application, bool>>>()))
                .ReturnsAsync(applications);

            // Act
            var result = await _mock.Object.GetAllAsync(app => true);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public async Task UpdateAsync_ShouldReturnUpdatedApplication()
        {
            // Arrange
            var application = new Application { Id = 1, Code = "APP0000001", Name = "Updated Name", IsActive = true };
            _mock.Setup(repo => repo.UpdateAsync(application))
                .ReturnsAsync(application);

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
            _mock.Setup(repo => repo.DeactivateAsync("APP0000001"))
                .ReturnsAsync(true);

            // Act
            var result = await _mock.Object.DeactivateAsync("APP0000001");

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