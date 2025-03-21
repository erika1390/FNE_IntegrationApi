using Integration.Core.Entities.Security;
using Integration.Infrastructure.Interfaces.Security;
using Moq;
using System.Linq.Expressions;
namespace Integration.Infrastructure.Test.Repositories.Security
{
    [TestFixture]
    public class UserRepositoryTest
    {
        private Mock<IUserRepository> _mock;

        [SetUp]
        public void Setup()
        {
            _mock = new Mock<IUserRepository>();
        }

        [Test]
        public async Task GetByCodeAsync_ShouldReturnCorrectUser()
        {
            // Arrange
            var user = new User { Id = 1, Code = "USR0000001", UserName = "epulido", IsActive = true, CreatedBy ="epulido", FirstName="Erika", LastName="Pulido"};
            _mock.Setup(repo => repo.GetByCodeAsync("USR0000001"))
                .ReturnsAsync(user);

            // Act
            var result = await _mock.Object.GetByCodeAsync("USR0000001");

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("USR0000001", result.Code);
            Assert.AreEqual("epulido", result.UserName);
        }

        [Test]
        public async Task GetByCodeAsync_ShouldReturnNullForNonExistentCode()
        {
            // Arrange
            _mock.Setup(repo => repo.GetByCodeAsync("INVALID_CODE"))
                .ReturnsAsync((User)null);

            // Act
            var result = await _mock.Object.GetByCodeAsync("INVALID_CODE");

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetAllAsync_WithPredicate_ShouldReturnFilteredUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User {Id = 1, Code = "USR0000001", UserName = "epulido", IsActive = true, CreatedBy ="epulido", FirstName="Erika", LastName="Pulido" },
                new User { Id = 2, Code = "USR0000002", UserName = "test", IsActive = true, CreatedBy ="epulido", FirstName="system1", LastName="system2" }
            };

            Expression<Func<User, bool>> predicate = user => user.IsActive;
            var expectedResults = users.Where(predicate.Compile()).ToList();

            _mock.Setup(repo => repo.GetByFilterAsync(predicate))
                .ReturnsAsync(expectedResults);

            // Act
            var result = await _mock.Object.GetByFilterAsync(predicate);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(expectedResults.Count, result.Count);
            Assert.IsTrue(result.All(user => user.IsActive));
        }

        [Test]
        public async Task GetAllAsync_WithPredicates_ShouldReturnFilteredUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User {Id = 1, Code = "USR0000001", UserName = "epulido", IsActive = true, CreatedBy ="epulido", FirstName="Erika", LastName="Pulido" },
                new User { Id = 2, Code = "USR0000002", UserName = "test", IsActive = true, CreatedBy ="epulido", FirstName="system1", LastName="system2" }
            };

            var predicates = new List<Expression<Func<User, bool>>>
            {
                user => user.IsActive,
                user => user.Code.Contains("USR")
            };

            var expectedResults = users.Where(user => predicates.All(p => p.Compile()(user))).ToList();

            _mock.Setup(repo => repo.GetByMultipleFiltersAsync(predicates))
                .ReturnsAsync(expectedResults);

            // Act
            var result = await _mock.Object.GetByMultipleFiltersAsync(predicates);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(expectedResults.Count, result.Count);
            Assert.IsTrue(result.All(user => user.IsActive));
        }

        [Test]
        public async Task UpdateAsync_ShouldReturnUpdatedUser()
        {
            // Arrange
            var user = new User { Id = 1, Code = "USR0000001", UserName = "epulido", IsActive = true, CreatedBy = "epulido", FirstName = "Erika", LastName = "Pulido" };

            _mock.Setup(repo => repo.UpdateAsync(It.IsAny<User>()))
                .ReturnsAsync((User u) => u);

            // Act
            var result = await _mock.Object.UpdateAsync(user);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("epulido", result.UserName);
        }

        [Test]
        public async Task DeactivateAsync_ShouldReturnTrue()
        {
            // Arrange
            _mock.Setup(repo => repo.DeactivateAsync("USR0000001", "epulido"))
                .ReturnsAsync(true);

            // Act
            var result = await _mock.Object.DeactivateAsync("USR0000001", "epulido");

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task GetAllActiveAsync_ShouldReturnActiveUsers()
        {
            // Arrange
            var activeUsers = new List<User>
            {
                new User {Id = 1, Code = "USR0000001", UserName = "epulido", IsActive = true, CreatedBy ="epulido", FirstName="Erika", LastName="Pulido" },
                new User { Id = 2, Code = "USR0000002", UserName = "test", IsActive = true, CreatedBy ="epulido", FirstName="system1", LastName="system2" }
            };

            _mock.Setup(repo => repo.GetAllActiveAsync())
                .ReturnsAsync(activeUsers);

            // Act
            var result = await _mock.Object.GetAllActiveAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsTrue(result.All(user => user.IsActive));
        }

        [Test]
        public async Task CreateAsync_ShouldReturnCreatedUser()
        {
            // Arrange
            var user = new User { Id = 1, Code = "USR0000001", UserName = "epulido", IsActive = true, CreatedBy = "epulido", FirstName = "Erika", LastName = "Pulido" };

            _mock.Setup(repo => repo.CreateAsync(It.IsAny<User>()))
                .ReturnsAsync((User u) => u);

            // Act
            var result = await _mock.Object.CreateAsync(user);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("USR0000001", result.Code);
        }
    }
}