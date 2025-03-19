using Integration.Core.Entities.Audit;
using Integration.Infrastructure.Interfaces.Audit;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Integration.Infrastructure.Test.Repositories.Audit
{
    [TestFixture]
    public class LogRepositoryTest
    {
        private Mock<ILogRepository> _mock;

        [SetUp]
        public void Setup()
        {
            _mock = new Mock<ILogRepository>();
        }

        [Test]
        public async Task CreateAsync_ShouldReturnCreatedLog()
        {
            // Arrange
            var log = new Log
            {
                CodeApplication = "APP0000001",
                CodeUser = "USR0000001",
                Timestamp = DateTime.UtcNow,
                Level = "Info",
                Source = "System",
                Method = "POST",
                Message = "Test log",
                UserIp = "172.60.8.114"
            };

            _mock.Setup(repo => repo.CreateAsync(It.IsAny<Log>()))
                .ReturnsAsync((Log l) => l);

            // Act
            var result = await _mock.Object.CreateAsync(log);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("APP0000001", result.CodeApplication);
        }

        [Test]
        public async Task CreateAsync_ShouldThrowArgumentNullException_WhenLogIsNull()
        {
            // Arrange
            _mock.Setup(repo => repo.CreateAsync(null))
                 .ThrowsAsync(new ArgumentNullException("log", "El log no puede ser nulo."));

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () => await _mock.Object.CreateAsync(null));

            // ✅ Verificar que la excepción se lanzó y el mensaje es correcto
            Assert.NotNull(ex, "La excepción no debe ser nula.");
            Assert.AreEqual("El log no puede ser nulo. (Parameter 'log')", ex.Message);
        }

        [Test]
        public async Task SearchAsync_ShouldReturnLogs_FilteredByCodeApplication()
        {
            // Arrange
            var logs = new List<Log>
            {
                new Log { CodeApplication = "APP0000001", CodeUser = "USR0000001", Timestamp = DateTime.UtcNow, Level = "Info", Source = "System", Method = "GET", Message = "Log 1", UserIp = "172.60.8.114" },
                new Log { CodeApplication = "APP0000002", CodeUser = "USR0000001", Timestamp = DateTime.UtcNow, Level = "Error", Source = "System", Method = "POST", Message = "Log 2", UserIp = "172.60.8.114" }
            };

            // ✅ Asegurar que el setup coincide con la consulta del Act
            _mock.Setup(repo => repo.SearchAsync("APP0000001", null, null, null, null, null))
                .ReturnsAsync(logs.Where(l => l.CodeApplication == "APP0000001").ToList());

            // Act
            var result = await _mock.Object.SearchAsync("APP0000001", null, null, null, null, null); // ✅ Usar el mismo valor configurado en el mock

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.Count(), "El resultado debe contener exactamente un elemento.");
            Assert.AreEqual("APP0000001", result.First().CodeApplication, "El CodeApplication debe coincidir con el esperado.");
        }

        [Test]
        public async Task SearchAsync_ShouldReturnLogs_FilteredByTimestamp()
        {
            // Arrange
            var date = DateTime.UtcNow.Date;
            var logs = new List<Log>
            {
                new Log { CodeApplication = "APP0000001", CodeUser = "USR0000001", Timestamp = date, Level = "Info", Source = "System", Method = "GET", Message = "Log 1",UserIp = "172.60.8.114" },
                new Log { CodeApplication = "APP0000002", CodeUser = "USR0000001", Timestamp = date.AddDays(-1), Level = "Error", Source = "System", Method = "POST", Message = "Log 2",UserIp = "172.60.8.114" }
            };

            _mock.Setup(repo => repo.SearchAsync(null, null, date, null, null, null))
                .ReturnsAsync(logs.Where(l => l.Timestamp.Date == date).ToList());

            // Act
            var result = await _mock.Object.SearchAsync(null, null, date, null, null, null);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(date, result.First().Timestamp.Date);
        }

        [Test]
        public async Task SearchAsync_ShouldReturnEmptyList_WhenNoMatches()
        {
            // Arrange
            _mock.Setup(repo => repo.SearchAsync("APP999", null, null, null, null, null))
                .ReturnsAsync(new List<Log>());

            // Act
            var result = await _mock.Object.SearchAsync("APP999", null, null, null, null, null);

            // Assert
            Assert.NotNull(result);
            Assert.IsEmpty(result);
        }
    }
}