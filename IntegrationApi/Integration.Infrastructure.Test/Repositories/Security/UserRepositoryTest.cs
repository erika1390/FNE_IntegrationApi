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
        public async Task GetByIdAsync_ShouldReturnUser()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                Code = "USR0000001",
                FirstName = "Erika",
                LastName = "Pulido Moreno",
                DateOfBirth = DateTime.Now.AddYears(-34),                
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CreatedBy = "System",
                UpdatedBy = "System",
                IsActive = true,
                UserName = "epulido",
                NormalizedUserName = "EPULIDO",
                Email = "epulido@minsalud.gov.co",
                NormalizedEmail = "EPULIDO@MINSALUD.GOV.CO",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAEAACcQAAAAEJ9zQ6",
                SecurityStamp = "QJZQ4Q",
                ConcurrencyStamp = "d1b1b2b3-4b5b-6b7b-8b9b-0b1b2b3b4b5",
                PhoneNumber = "3001234567",
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                LockoutEnd = null,
                LockoutEnabled = true,
                AccessFailedCount = 0
            };
            _mock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(user);
            // Act
            var result = await _mock.Object.GetByIdAsync(1);
            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.Id);
        }
        [Test]
        public async Task GetAllAsync_WithPredicate_ShouldReturnFilteredUsers()
        {
            // Arrange
            var Users = new List<User>
            {
                new User { 
                    Id = 1,
                    Code = "USR0000001",
                    FirstName = "Erika",
                    LastName = "Pulido Moreno",
                    DateOfBirth = DateTime.Now.AddYears(-34),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    IsActive = true,
                    UserName = "epulido",
                    NormalizedUserName = "EPULIDO",
                    Email = "epulido@minsalud.gov.co",
                    NormalizedEmail = "EPULIDO@MINSALUD.GOV.CO",
                    EmailConfirmed = true,
                    PasswordHash = "AQAAAAEAACcQAAAAEJ9zQ6",
                    SecurityStamp = "QJZQ4Q",
                    ConcurrencyStamp = "d1b1b2b3-4b5b-6b7b-8b9b-0b1b2b3b4b5",
                    PhoneNumber = "3001234567",
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0
                },
                new User {
                    Id = 2,
                    Code = "USR0000002",
                    FirstName = "Julian",
                    LastName = "Cuervo Bustamante",
                    DateOfBirth = DateTime.Now.AddYears(-45),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    IsActive = true,
                    UserName = "jcuervo",
                    NormalizedUserName = "JCUERVO",
                    Email = "jcuervo@minsalud.gov.co",
                    NormalizedEmail = "JCUERVO@MINSALUD.GOV.CO",
                    EmailConfirmed = true,
                    PasswordHash = "AQBBBBEAACcQAAAAEJ9zQ6",
                    SecurityStamp = "TJZM4H",
                    ConcurrencyStamp = "d1b1b2b3-4b5b-6b7b-8b9b-0b1b2b3b4b5",
                    PhoneNumber = "3001234567",
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0
                }
            };
            Expression<Func<User, bool>> predicate = app => app.IsActive;
            _mock.Setup(repo => repo.GetAllAsync(predicate))
                .ReturnsAsync(Users.Where(predicate.Compile()).ToList());
            // Act
            var result = await _mock.Object.GetAllAsync(predicate);
            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.First().IsActive);
        }
        [Test]
        public async Task GetAllAsync_WithPredicates_ShouldReturnFilteredUsers()
        {
            // Arrange
            var Users = new List<User>
            {
                 new User {
                    Id = 1,
                    Code = "USR0000001",
                    FirstName = "Erika",
                    LastName = "Pulido Moreno",
                    DateOfBirth = DateTime.Now.AddYears(-34),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    IsActive = true,
                    UserName = "epulido",
                    NormalizedUserName = "EPULIDO",
                    Email = "epulido@minsalud.gov.co",
                    NormalizedEmail = "EPULIDO@MINSALUD.GOV.CO",
                    EmailConfirmed = true,
                    PasswordHash = "AQAAAAEAACcQAAAAEJ9zQ6",
                    SecurityStamp = "QJZQ4Q",
                    ConcurrencyStamp = "d1b1b2b3-4b5b-6b7b-8b9b-0b1b2b3b4b5",
                    PhoneNumber = "3001234567",
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0
                },
                new User {
                    Id = 2,
                    Code = "USR0000002",
                    FirstName = "Julian",
                    LastName = "Cuervo Bustamante",
                    DateOfBirth = DateTime.Now.AddYears(-45),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    IsActive = true,
                    UserName = "jcuervo",
                    NormalizedUserName = "JCUERVO",
                    Email = "jcuervo@minsalud.gov.co",
                    NormalizedEmail = "JCUERVO@MINSALUD.GOV.CO",
                    EmailConfirmed = true,
                    PasswordHash = "AQBBBBEAACcQAAAAEJ9zQ6",
                    SecurityStamp = "TJZM4H",
                    ConcurrencyStamp = "d1b1b2b3-4b5b-6b7b-8b9b-0b1b2b3b4b5",
                    PhoneNumber = "3001234567",
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0
                }
            };
            var predicates = new List<Expression<Func<User, bool>>>
            {
                app => app.IsActive,
                app => app.Code.Contains("USR")
            };
            _mock.Setup(repo => repo.GetAllAsync(predicates))
                .ReturnsAsync(Users.Where(app => predicates.All(p => p.Compile()(app))).ToList());
            // Act
            var result = await _mock.Object.GetAllAsync(predicates);
            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.First().IsActive);
        }
        [Test]
        public async Task CreateAsync_ShouldReturnCreatedUser()
        {
            // Arrange
            var User = new User {
                Id = 1,
                Code = "USR0000001",
                FirstName = "Erika",
                LastName = "Pulido Moreno",
                DateOfBirth = DateTime.Now.AddYears(-34),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CreatedBy = "System",
                UpdatedBy = "System",
                IsActive = true,
                UserName = "epulido",
                NormalizedUserName = "EPULIDO",
                Email = "epulido@minsalud.gov.co",
                NormalizedEmail = "EPULIDO@MINSALUD.GOV.CO",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAEAACcQAAAAEJ9zQ6",
                SecurityStamp = "QJZQ4Q",
                ConcurrencyStamp = "d1b1b2b3-4b5b-6b7b-8b9b-0b1b2b3b4b5",
                PhoneNumber = "3001234567",
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                LockoutEnd = null,
                LockoutEnabled = true,
                AccessFailedCount = 0
            };
            _mock.Setup(repo => repo.CreateAsync(User))
                .ReturnsAsync(User);
            // Act
            var result = await _mock.Object.CreateAsync(User);
            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.Id);
        }
        [Test]
        public async Task UpdateAsync_ShouldReturnUpdatedUser()
        {
            // Arrange
            var User = new User {
                Id = 1,
                Code = "USR0000001",
                FirstName = "Erika",
                LastName = "Pulido Moreno",
                DateOfBirth = DateTime.Now.AddYears(-34),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CreatedBy = "System",
                UpdatedBy = "System",
                IsActive = true,
                UserName = "epulido",
                NormalizedUserName = "EPULIDO",
                Email = "epulido@minsalud.gov.co",
                NormalizedEmail = "EPULIDO@MINSALUD.GOV.CO",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAEAACcQAAAAEJ9zQ6",
                SecurityStamp = "QJZQ4Q",
                ConcurrencyStamp = "d1b1b2b3-4b5b-6b7b-8b9b-0b1b2b3b4b5",
                PhoneNumber = "3001234567",
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                LockoutEnd = null,
                LockoutEnabled = true,
                AccessFailedCount = 0
            };
            _mock.Setup(repo => repo.UpdateAsync(User)).ReturnsAsync(User);
            // Act
            var result = await _mock.Object.UpdateAsync(User);
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
        public async Task GetAllActiveAsync_ShouldReturnActiveUsers()
        {
            // Arrange
            var activeUsers = new List<User>
            {
                new User {
                    Id = 1,
                    Code = "USR0000001",
                    FirstName = "Erika",
                    LastName = "Pulido Moreno",
                    DateOfBirth = DateTime.Now.AddYears(-34),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    IsActive = true,
                    UserName = "epulido",
                    NormalizedUserName = "EPULIDO",
                    Email = "epulido@minsalud.gov.co",
                    NormalizedEmail = "EPULIDO@MINSALUD.GOV.CO",
                    EmailConfirmed = true,
                    PasswordHash = "AQAAAAEAACcQAAAAEJ9zQ6",
                    SecurityStamp = "QJZQ4Q",
                    ConcurrencyStamp = "d1b1b2b3-4b5b-6b7b-8b9b-0b1b2b3b4b5",
                    PhoneNumber = "3001234567",
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0
                },
                new User {
                    Id = 2,
                    Code = "USR0000002",
                    FirstName = "Julian",
                    LastName = "Cuervo Bustamante",
                    DateOfBirth = DateTime.Now.AddYears(-45),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    IsActive = true,
                    UserName = "jcuervo",
                    NormalizedUserName = "JCUERVO",
                    Email = "jcuervo@minsalud.gov.co",
                    NormalizedEmail = "JCUERVO@MINSALUD.GOV.CO",
                    EmailConfirmed = true,
                    PasswordHash = "AQBBBBEAACcQAAAAEJ9zQ6",
                    SecurityStamp = "TJZM4H",
                    ConcurrencyStamp = "d1b1b2b3-4b5b-6b7b-8b9b-0b1b2b3b4b5",
                    PhoneNumber = "3001234567",
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0
                }
            };
            _mock.Setup(repo => repo.GetAllActiveAsync()).ReturnsAsync(activeUsers);
            // Act
            var result = await _mock.Object.GetAllActiveAsync();
            // Assert
            Assert.NotNull(result);
            Assert.IsTrue(result.All(app => app.IsActive));
        }
    }
}