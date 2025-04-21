using Integration.Infrastructure.Interfaces.Security;
using Integration.Shared.DTO.Security;

using Moq;

namespace Integration.Infrastructure.Test.Repositories.Security
{
    [TestFixture]
    public class UserPermissionRepositoryTest
    {
        private Mock<IUserPermissionRepository> _mockRepository;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<IUserPermissionRepository>();
        }

        [Test]
        public async Task GetAllPermissionsByUserCodeAsync_ShouldReturnPermissions_WhenUserHasPermissions()
        {
            // Arrange
            var userCode = "USR0000001";
            var applicationId = 1;

            var dto = new UserPermissionDTO
            {
                CodeUser = userCode,
                UserName = "epulido",
                Roles = new List<RoleDto>
                {
                    new RoleDto
                    {
                        Code = "ROL0000001",
                        Name = "Admin",
                        Modules = new List<ModuleDto>
                        {
                            new ModuleDto
                            {
                                Code = "MOD0000001",
                                Name = "Gestión",
                                Menus = new List<MenuDto>
                                {
                                    new MenuDto
                                    {
                                        Code = "MNU0000001",
                                        Name = "Menú 1",
                                        Permissions = new List<PermissionDto>
                                        {
                                            new PermissionDto
                                            {
                                                Code = "PER0000001",
                                                Name = "Ver"
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            _mockRepository.Setup(r => r.GetAllPermissionsByUserCodeAsync(userCode, applicationId))
                           .ReturnsAsync(dto);

            // Act
            var result = await _mockRepository.Object.GetAllPermissionsByUserCodeAsync(userCode, applicationId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(userCode, result.CodeUser);
            Assert.AreEqual("epulido", result.UserName);
            Assert.IsNotEmpty(result.Roles);
            Assert.AreEqual("ROL0000001", result.Roles.First().Code);
            Assert.AreEqual("MOD0000001", result.Roles.First().Modules.First().Code);
            Assert.AreEqual("MNU0000001", result.Roles.First().Modules.First().Menus.First().Code);
            Assert.AreEqual("PER0000001", result.Roles.First().Modules.First().Menus.First().Permissions.First().Code);
        }
    }
}