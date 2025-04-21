using Integration.Core.Entities.Security;
using Integration.Infrastructure.Data.Contexts;
using Integration.Infrastructure.Repositories.Security;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Moq;

namespace Integration.Infrastructure.Test.Repositories.Security
{
    [TestFixture]
    public class UserPermissionRepositoryTest
    {
        private ApplicationDbContext _context;
        private UserPermissionRepository _repository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("MyTestDb") // <- Esta línea ahora sí será reconocida
            .Options;

            _context = new ApplicationDbContext(options);

            var loggerMock = new Mock<ILogger<UserPermissionRepository>>();
            _repository = new UserPermissionRepository(_context, loggerMock.Object);
        }

        [Test]
        public async Task GetAllPermissionsByUserCodeAsync_ShouldReturnPermissions_WhenUserHasPermissions()
        {
            // Arrange
            var user = new User { Id = 1, Code = "USR0000001", UserName = "epulido", IsActive = true, CreatedBy="epulido", FirstName="Erika", LastName="Pulido" };
            var role = new Role { Id = 1, Code = "ROL0000001", Name = "Administrador", ApplicationId = 1, IsActive = true, CreatedBy = "epulido" };
            var module = new Module { Id = 1, Code = "MOD0000001", Name = "Configuración", ApplicationId = 1, IsActive = true };
            var menu = new Menu { Id = 1, Code = "MNU0000001", Name = "Configuración", ModuleId = 1, IsActive = true };
            var permission = new Permission { Id = 1, Code = "PER0000001", Name = "Consultar", IsActive = true };

            _context.Users.Add(user);
            _context.Roles.Add(role);
            _context.Modules.Add(module);
            _context.Menus.Add(menu);
            _context.Permissions.Add(permission);
            _context.UserRoles.Add(new UserRole { UserId = 1, RoleId = 1, IsActive = true, CreatedBy = "epulido" });
            _context.RoleMenuPermissions.Add(new RoleMenuPermission { RoleId = 1, MenuId = 1, PermissionId = 1, IsActive = true });

            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllPermissionsByUserCodeAsync("USR0000001", 1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("USR0000001", result.CodeUser);
            Assert.AreEqual("epulido", result.UserName);
            Assert.AreEqual(1, result.Roles.Count);
            Assert.AreEqual("ROL0000001", result.Roles[0].Code);
            Assert.AreEqual("Administrador", result.Roles[0].Name);
            Assert.AreEqual(1, result.Roles[0].Modules.Count);
            Assert.AreEqual("MOD0000001", result.Roles[0].Modules[0].Code);
            Assert.AreEqual(1, result.Roles[0].Modules[0].Menus.Count);
            Assert.AreEqual("MNU0000001", result.Roles[0].Modules[0].Menus[0].Code);
            Assert.AreEqual(1, result.Roles[0].Modules[0].Menus[0].Permissions.Count);
            Assert.AreEqual("PER0000001", result.Roles[0].Modules[0].Menus[0].Permissions[0].Code);
        }

        [Test]
        public async Task GetAllPermissionsByUserCodeAsync_ShouldReturnNull_WhenUserHasNoPermissions()
        {
            // Arrange
            // (sin datos)

            // Act
            var result = await _repository.GetAllPermissionsByUserCodeAsync("USR0000001", 1);

            // Assert
            Assert.IsNull(result);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }
    }
}