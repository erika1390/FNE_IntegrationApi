using Integration.Core.Entities.Base;
using Integration.Infrastructure.Data.Contexts;
using Integration.Infrastructure.Repositories.Base;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using MockQueryable.Moq;

using Moq;

using System.Linq.Expressions;

namespace Integration.Infrastructure.Test.Repositories.Base
{
    public class FakeEntity : BaseEntity
    {
        public string Name { get; set; }
    }

    [TestFixture]
    public class RepositoryBaseTest
    {
        private Mock<ApplicationDbContext> _dbContextMock;
        private Mock<DbSet<FakeEntity>> _dbSetMock;
        private RepositoryBase<FakeEntity> _repository;

        [SetUp]
        public void SetUp()
        {
            _dbContextMock = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            _dbSetMock = new Mock<DbSet<FakeEntity>>();
            _dbContextMock.Setup(x => x.Set<FakeEntity>()).Returns(_dbSetMock.Object);
            _repository = new RepositoryBase<FakeEntity>(_dbContextMock.Object);
        }

        [Test]
        public async Task CreateAsync_ShouldAddEntity_WhenEntityIsValid()
        {
            var entity = new FakeEntity { Id = 1, Name = "Test" };
            _dbSetMock.Setup(x => x.AddAsync(entity, default)).ReturnsAsync((EntityEntry<FakeEntity>)null);

            var result = await _repository.CreateAsync(entity);

            Assert.IsNotNull(result);
            Assert.AreEqual(entity.Id, result.Id);
            _dbSetMock.Verify(x => x.AddAsync(entity, default), Times.Once);
            _dbContextMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }

        [Test]
        public async Task DeactivateAsync_ShouldReturnTrue_WhenEntityExists()
        {
            var entity = new FakeEntity { Id = 1 };
            _dbSetMock.Setup(x => x.FindAsync("CODE001")).ReturnsAsync(entity);

            var result = await _repository.DeactivateAsync("CODE001", "TestUser");

            Assert.IsTrue(result);
            _dbSetMock.Verify(x => x.Remove(entity), Times.Once);
            _dbContextMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }

        [Test]
        public async Task DeactivateAsync_ShouldReturnFalse_WhenEntityDoesNotExist()
        {
            _dbSetMock.Setup(x => x.FindAsync("CODE001")).ReturnsAsync((FakeEntity)null);

            var result = await _repository.DeactivateAsync("CODE001", "TestUser");

            Assert.IsFalse(result);
            _dbSetMock.Verify(x => x.Remove(It.IsAny<FakeEntity>()), Times.Never);
            _dbContextMock.Verify(x => x.SaveChangesAsync(default), Times.Never);
        }

        [Test]
        public async Task GetByFilterAsync_ShouldReturnEntities_WhenFilterIsApplied()
        {
            // Arrange
            var data = new List<FakeEntity>
            {
                new FakeEntity { Id = 1 },
                new FakeEntity { Id = 2 }
            };

            var mockDbSet = data.AsQueryable().BuildMockDbSet(); // ✅ construye un DbSet con soporte async y linq

            _dbContextMock.Setup(x => x.Set<FakeEntity>()).Returns(mockDbSet.Object);
            _repository = new RepositoryBase<FakeEntity>(_dbContextMock.Object);

            Expression<Func<FakeEntity, bool>> predicate = e => e.Id > 0;

            // Act
            var result = await _repository.GetByFilterAsync(predicate);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public async Task GetByMultipleFiltersAsync_ShouldReturnEntities_WhenFiltersAreApplied()
        {
            // Arrange
            var data = new List<FakeEntity>
            {
                new FakeEntity { Id = 1 },
                new FakeEntity { Id = 2 }
            };

            var queryableData = data.AsQueryable().BuildMockDbSet();

            _dbContextMock.Setup(db => db.Set<FakeEntity>()).Returns(queryableData.Object);
            _repository = new RepositoryBase<FakeEntity>(_dbContextMock.Object);

            var filters = new List<Expression<Func<FakeEntity, bool>>>
            {
                e => e.Id > 0,
                e => e.Id < 3
            };

            // Act
            var result = await _repository.GetByMultipleFiltersAsync(filters);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnEntity_WhenEntityExists()
        {
            // Arrange
            var fakeData = new List<FakeEntity>
            {
                new FakeEntity { Id = 1 },
                new FakeEntity { Id = 2 }
            }.AsQueryable();

            var mockDbSet = fakeData.BuildMockDbSet();

            _dbContextMock.Setup(x => x.Set<FakeEntity>()).Returns(mockDbSet.Object);
            _repository = new RepositoryBase<FakeEntity>(_dbContextMock.Object);

            // Act
            var result = await _repository.GetByIdAsync(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateEntity_WhenEntityIsValid()
        {
            var entity = new FakeEntity { Id = 1 };
            _dbSetMock.Setup(x => x.Update(entity));

            var result = await _repository.UpdateAsync(entity);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            _dbSetMock.Verify(x => x.Update(entity), Times.Once);
            _dbContextMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }
    }
}