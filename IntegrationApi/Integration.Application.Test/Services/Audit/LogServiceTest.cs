using AutoMapper;

using Integration.Application.Interfaces.Audit;
using Integration.Application.Services.Audit;
using Integration.Infrastructure.Interfaces.Audit;
using Integration.Shared.DTO.Audit;

using Moq;

namespace Integration.Application.Test.Services.Audit
{
    [TestFixture]
    public class LogServiceTest
    {
        private Mock<ILogRepository> _repositoryMock;
        private Mock<IMapper> _mapperMock;
        private ILogService _logService;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<ILogRepository>();
            _mapperMock = new Mock<IMapper>();
            _logService = new LogService(_repositoryMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task CreateAsync_ShouldReturnCreatedLogDTO()
        {
            // Arrange
            var logDTO = new LogDTO { LogId = Guid.NewGuid(), CodeApplication = "APP0000001", CodeUser = "USER0000001", UserIp = "127.0.0.1", Timestamp = DateTime.UtcNow, Level = "Info", Message = "Test log" };
            var log = new Integration.Core.Entities.Audit.Log { LogId = logDTO.LogId, CodeApplication = logDTO.CodeApplication, CodeUser = logDTO.CodeUser, UserIp = logDTO.UserIp, Timestamp = logDTO.Timestamp, Level = logDTO.Level, Message = logDTO.Message };

            _mapperMock.Setup(m => m.Map<Integration.Core.Entities.Audit.Log>(logDTO)).Returns(log);
            _repositoryMock.Setup(r => r.CreateAsync(log)).ReturnsAsync(log);
            _mapperMock.Setup(m => m.Map<LogDTO>(log)).Returns(logDTO);

            // Act
            var result = await _logService.CreateAsync(logDTO);

            // Assert
            Assert.AreEqual(logDTO, result);
        }

        [Test]
        public async Task SearchAsync_ShouldReturnLogDTOs_WhenLogsExist()
        {
            // Arrange
            var logs = new List<Integration.Core.Entities.Audit.Log>
            {
                new Integration.Core.Entities.Audit.Log { LogId = Guid.NewGuid(), CodeApplication = "APP0000001", CodeUser = "USER0000001", UserIp = "127.0.0.1", Timestamp = DateTime.UtcNow, Level = "Info", Message = "Test log" }
            };
            var logDTOs = new List<LogDTO>
            {
                new LogDTO { LogId = logs[0].LogId, CodeApplication = logs[0].CodeApplication, CodeUser = logs[0].CodeUser, UserIp = logs[0].UserIp, Timestamp = logs[0].Timestamp, Level = logs[0].Level, Message = logs[0].Message }
            };

            _repositoryMock.Setup(r => r.SearchAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime?>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(logs);
            _mapperMock.Setup(m => m.Map<IEnumerable<LogDTO>>(logs)).Returns(logDTOs);

            // Act
            var result = await _logService.SearchAsync("APP0000001", "USER0000001", DateTime.UtcNow, "Info", "Source", "Method");

            // Assert
            Assert.AreEqual(logDTOs, result);
        }

        [Test]
        public async Task SearchAsync_ShouldReturnEmpty_WhenNoLogsExist()
        {
            // Arrange
            _repositoryMock.Setup(r => r.SearchAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime?>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((IEnumerable<Integration.Core.Entities.Audit.Log>)null);

            // Act
            var result = await _logService.SearchAsync("APP0000001", "USER0000001", DateTime.UtcNow, "Info", "Source", "Method");

            // Assert
            Assert.IsEmpty(result);
        }
    }
}