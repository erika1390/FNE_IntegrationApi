using Integration.Core.Entities.Security;
using Integration.Infrastructure.Interfaces.Security;

using Moq;

using System.Linq.Expressions;

namespace Integration.Infrastructure.Test.Repositories.Security
{
    [TestFixture]
    public class UserRoleRepositoryTest
    {
        private Mock<IUserRoleRepository> _mock;

        [SetUp]
        public void Setup()
        {
            _mock = new Mock<IUserRoleRepository>();
        }

        [Test]
        public async Task CreateAsync_ShouldReturnCreatedUserRole()
        {
            // Arrange
            var userRole = new UserRole { 
                Id = 1, 
                UserId = 1, 
                RoleId = 1, 
                IsActive = true ,
                CreatedBy = "epulido"
            };

            _mock.Setup(repo => repo.CreateAsync(It.IsAny<UserRole>()))
                .ReturnsAsync((UserRole ur) => ur);

            // Act
            var result = await _mock.Object.CreateAsync(userRole);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.UserId);
            Assert.AreEqual(1, result.RoleId);
        }

        [Test]
        public void CreateAsync_ShouldThrowArgumentNullException_WhenUserRoleIsNull()
        {
            // Arrange
            _mock.Setup(repo => repo.CreateAsync(null))
                 .ThrowsAsync(new ArgumentNullException("userRole", "El UserRole no puede ser nulo."));

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () => await _mock.Object.CreateAsync(null));

            // ✅ Verificar que la excepción se lanzó y el mensaje es correcto
            Assert.NotNull(ex, "La excepción no debe ser nula.");
            Assert.AreEqual("El UserRole no puede ser nulo. (Parameter 'userRole')", ex.Message);
        }


        [Test]
        public async Task DeactivateAsync_ShouldReturnTrue()
        {
            // Arrange
            _mock.Setup(repo => repo.DeactivateAsync(1, 1, "epulido"))
                .ReturnsAsync(true);

            // Act
            var result = await _mock.Object.DeactivateAsync(1, 1, "epulido");

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task GetAllActiveAsync_ShouldReturnActiveUserRoles()
        {
            // Arrange
            var activeUserRoles = new List<UserRole>
            {
                new UserRole {
                    Id = 1,
                    UserId = 1,
                    RoleId = 1,
                    IsActive = true ,
                    CreatedBy = "epulido"
                },
                new UserRole {
                    Id = 1,
                    UserId = 1,
                    RoleId = 2,
                    IsActive = true ,
                    CreatedBy = "epulido"
                }
            };

            _mock.Setup(repo => repo.GetAllActiveAsync())
                .ReturnsAsync(activeUserRoles);

            // Act
            var result = await _mock.Object.GetAllActiveAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsTrue(result.All(ur => ur.IsActive));
        }

        [Test]
        public async Task GetByUserIdRoleIdAsync_ShouldReturnCorrectUserRole()
        {
            // Arrange
            var userRole = new UserRole {
                Id = 1,
                UserId = 1,
                RoleId = 1,
                IsActive = true,
                CreatedBy = "epulido"
            };

            _mock.Setup(repo => repo.GetByUserIdRoleIdAsync(1, 1))
                .ReturnsAsync(userRole);

            // Act
            var result = await _mock.Object.GetByUserIdRoleIdAsync(1, 1);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.UserId);
            Assert.AreEqual(1, result.RoleId);
        }

        [Test]
        public async Task GetByUserIdRoleIdAsync_ShouldReturnNull_WhenNotFound()
        {
            // Arrange
            _mock.Setup(repo => repo.GetByUserIdRoleIdAsync(999, 999))
                .ReturnsAsync((UserRole)null);

            // Act
            var result = await _mock.Object.GetByUserIdRoleIdAsync(999, 999);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task UpdateAsync_ShouldReturnUpdatedUserRole()
        {
            // Arrange
            var userRole = new UserRole {
                Id = 1,
                UserId = 1,
                RoleId = 1,
                IsActive = false,
                CreatedBy = "epulido"
            };

            _mock.Setup(repo => repo.UpdateAsync(It.IsAny<UserRole>()))
                .ReturnsAsync((UserRole ur) => ur);

            // Act
            var result = await _mock.Object.UpdateAsync(userRole);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(false, result.IsActive);
        }

        [Test]
        public async Task GetAllAsync_WithPredicate_ShouldReturnFilteredUserRoles()
        {
            // Arrange
            var userRoles = new List<UserRole>
            {
                new UserRole { Id = 1, UserId = 1, RoleId = 1, IsActive = true, CreatedBy = "epulido"},
                new UserRole {Id = 1, UserId = 1, RoleId = 2, IsActive = false, CreatedBy = "epulido"},
                new UserRole {Id = 1, UserId = 1, RoleId = 3, IsActive = false, CreatedBy = "epulido"}
            };

            Expression<Func<UserRole, bool>> predicate = ur => ur.IsActive;
            var expectedResults = userRoles.Where(predicate.Compile()).ToList();

            _mock.Setup(repo => repo.GetByFilterAsync(predicate))
                .ReturnsAsync(expectedResults);

            // Act
            var result = await _mock.Object.GetByFilterAsync(predicate);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(expectedResults.Count, result.Count);
            Assert.IsTrue(result.All(ur => ur.IsActive));
        }

        [Test]
        public async Task GetAllAsync_WithPredicates_ShouldReturnFilteredUserRoles()
        {
            // Arrange
            var userRoles = new List<UserRole>
            {
                new UserRole { Id = 1, UserId = 1, RoleId = 1, IsActive = true , CreatedBy = "epulido"},
                new UserRole {Id = 1, UserId = 1, RoleId = 2, IsActive = false, CreatedBy = "epulido"},
                new UserRole {Id = 1, UserId = 1, RoleId = 3, IsActive = true, CreatedBy = "epulido"}
            };

            var predicates = new List<Expression<Func<UserRole, bool>>>
            {
                ur => ur.IsActive,
                ur => ur.UserId > 1
            };

            var expectedResults = userRoles.Where(ur => predicates.All(p => p.Compile()(ur))).ToList();

            _mock.Setup(repo => repo.GetByMultipleFiltersAsync(predicates))
                .ReturnsAsync(expectedResults);

            // Act
            var result = await _mock.Object.GetByMultipleFiltersAsync(predicates);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(expectedResults.Count, result.Count);
            Assert.IsTrue(result.All(ur => ur.IsActive));
        }
    }
}