using Integration.Core.Entities.Security;
using Integration.Infrastructure.Interfaces.Security;

using Moq;

using System.Linq.Expressions;

namespace Integration.Infrastructure.Test.Repositories.Security
{
    [TestFixture]
    public class MenuRepositoryTest
    {
        private Mock<IMenuRepository> _mock;

        [SetUp]
        public void Setup()
        {
            _mock = new Mock<IMenuRepository>();
        }

        [Test]
        public async Task GetByCodeAsync_ShouldReturnCorrectMenu()
        {
            var menu = new Menu { Id = 1, Code = "MNU0000001", Name = "Administrador", IsActive = true };
            _mock.Setup(repo => repo.GetByCodeAsync("MNU0000001"))
                 .ReturnsAsync(menu);

            var result = await _mock.Object.GetByCodeAsync("MNU0000001");

            Assert.NotNull(result);
            Assert.AreEqual("MNU0000001", result.Code);
            Assert.AreEqual("Administrador", result.Name);
        }

        [Test]
        public async Task GetByCodeAsync_ShouldReturnNullForNonExistentCode()
        {
            _mock.Setup(repo => repo.GetByCodeAsync("INVALID_CODE"))
                 .ReturnsAsync((Menu)null);

            var result = await _mock.Object.GetByCodeAsync("INVALID_CODE");

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetAllActiveAsync_ShouldReturnActiveMenus()
        {
            var menus = new List<Menu>
            {
                new Menu { Id = 1, Code = "MNU0000001", Name = "Administrador", IsActive = true },
                new Menu { Id = 2, Code = "MNU0000002", Name = "Gestionar Aplicación", IsActive = true }
            };

            _mock.Setup(repo => repo.GetAllActiveAsync())
                 .ReturnsAsync(menus);

            var result = await _mock.Object.GetAllActiveAsync();

            Assert.NotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.All(m => m.IsActive));
        }

        [Test]
        public async Task CreateAsync_ShouldReturnCreatedMenu()
        {
            var menu = new Menu { Id = 1, Code = "MNU0000001", Name = "Administrador" };

            _mock.Setup(repo => repo.CreateAsync(It.IsAny<Menu>()))
                 .ReturnsAsync((Menu m) => m);

            var result = await _mock.Object.CreateAsync(menu);

            Assert.NotNull(result);
            Assert.AreEqual("MNU0000001", result.Code);
        }

        [Test]
        public async Task DeactivateAsync_ShouldReturnTrue()
        {
            _mock.Setup(repo => repo.DeactivateAsync("MNU0000001", "Administrador"))
                 .ReturnsAsync(true);

            var result = await _mock.Object.DeactivateAsync("MNU0000001", "Administrador");

            Assert.IsTrue(result);
        }

        [Test]
        public async Task UpdateAsync_ShouldReturnUpdatedMenu()
        {
            var updatedMenu = new Menu { Id = 1, Code = "MNU0000001", Name = "Administrador" };

            _mock.Setup(repo => repo.UpdateAsync(It.IsAny<Menu>()))
                 .ReturnsAsync((Menu m) => m);

            var result = await _mock.Object.UpdateAsync(updatedMenu);

            Assert.NotNull(result);
            Assert.AreEqual("Administrador", result.Name);
        }

        [Test]
        public async Task GetByFilterAsync_ShouldReturnFilteredMenus()
        {
            var menus = new List<Menu>
            {
                new Menu { Id = 1, Code = "MNU0000001", Name = "Administrador", IsActive = true },
                new Menu { Id = 2, Code = "MNU0000002", Name = "Gestionar Aplicación", IsActive = false }
            };

            Expression<Func<Menu, bool>> predicate = m => m.IsActive;
            var expected = menus.Where(predicate.Compile()).ToList();

            _mock.Setup(repo => repo.GetByFilterAsync(predicate))
                 .ReturnsAsync(expected);

            var result = await _mock.Object.GetByFilterAsync(predicate);

            Assert.NotNull(result);
            Assert.IsTrue(result.All(m => m.IsActive));
        }

        [Test]
        public async Task GetByMultipleFiltersAsync_ShouldReturnFilteredMenus()
        {
            var menus = new List<Menu>
            {
                new Menu { Id = 1, Code = "MNU0000001", Name = "Administrador", IsActive = true },
                new Menu { Id = 2, Code = "MNU0000002", Name = "Gestionar Aplicación", IsActive = true }
            };

            var predicates = new List<Expression<Func<Menu, bool>>>
            {
                m => m.IsActive,
                m => m.Code.StartsWith("MNU")
            };

            var expected = menus.Where(m => predicates.All(p => p.Compile()(m))).ToList();

            _mock.Setup(repo => repo.GetByMultipleFiltersAsync(predicates))
                 .ReturnsAsync(expected);

            var result = await _mock.Object.GetByMultipleFiltersAsync(predicates);

            Assert.NotNull(result);
            Assert.AreEqual(expected.Count, result.Count);
        }
    }
}