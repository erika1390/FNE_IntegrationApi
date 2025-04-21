using Integration.Api.Controllers.Audit;
using Integration.Application.Interfaces.Audit;
using Integration.Shared.DTO.Audit;
using Integration.Shared.DTO.Header;
using Integration.Shared.Response;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Moq;

using System.Reflection.PortableExecutable;

namespace Integration.Api.Test.Controllers.Audit
{
    [TestFixture]
    public class LogControllerTest
    {
        private Mock<ILogService> _serviceMock;
        private Mock<ILogger<LogController>> _loggerMock;
        private LogController _controller;

        [SetUp]
        public void SetUp()
        {
            _serviceMock = new Mock<ILogService>();
            _loggerMock = new Mock<ILogger<LogController>>();
            _controller = new LogController(_serviceMock.Object, _loggerMock.Object);
        }
        [Test]
        public async Task Search_ShouldReturnLogs_WhenLogsExist()
        {
            var header = new HeaderDTO { ApplicationCode = "APP0000001", UserCode = "USR0000001" };
            var logs = new List<LogDTO> { new LogDTO { Level = "Info", UserIp = "186.30.6.144" } };
            _serviceMock.Setup(s => s.SearchAsync(header, null, null, null, null))
                        .ReturnsAsync(logs);

            var result = await _controller.Search(header, null, null, null, null);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var response = okResult.Value as ResponseApi<IEnumerable<LogDTO>>;
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Data.Count());
        }

        [Test]
        public async Task Search_ShouldReturnOkWithEmptyList_WhenNoLogsFound()
        {
            var header = new HeaderDTO { ApplicationCode = "APP0000001", UserCode = "USR0000001" };
            _serviceMock.Setup(s => s.SearchAsync(header, null, null, null, null))
            .ReturnsAsync(new List<LogDTO>());

            var result = await _controller.Search(header, null, null, null, null);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var response = okResult.Value as ResponseApi<IEnumerable<LogDTO>>;
            Assert.IsNotNull(response);
            Assert.IsEmpty(response.Data);
        }

        [Test]
        public async Task Search_ShouldReturn500_WhenExceptionThrown()
        {
            var header = new HeaderDTO { ApplicationCode = "APP0000001", UserCode = "USR0000001" };
            _serviceMock.Setup(s => s.SearchAsync(header, null, null, null, null))
                        .ThrowsAsync(new Exception("Test Exception"));

            var result = await _controller.Search(header, null, null, null, null);

            var objectResult = result as ObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(500, objectResult.StatusCode);
        }

        [Test]
        public async Task Create_ShouldReturnOk_WhenLogCreated()
        {
            var header = new HeaderDTO { ApplicationCode = "APP0000001", UserCode = "USR0000001" };
            var dto = new LogDTO { Level = "Error", UserIp = "186.30.6.144" };
            _serviceMock.Setup(s => s.CreateAsync(header, dto)).ReturnsAsync(dto);

            var result = await _controller.Create(header, dto);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var response = okResult.Value as ResponseApi<LogDTO>;
            Assert.IsNotNull(response);
        }

        [Test]
        public async Task Create_ShouldReturnBadRequest_WhenLogNotCreated()
        {
            var header = new HeaderDTO { ApplicationCode = "APP0000001", UserCode = "USR0000001" };
            var dto = new LogDTO { Level = "Error", UserIp = "186.30.6.144" };
            _serviceMock.Setup(s => s.CreateAsync(header, dto)).ReturnsAsync((LogDTO)null);

            var result = await _controller.Create(header, dto);

            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            var response = badRequest.Value as ResponseApi<LogDTO>;
            Assert.IsNotNull(response);
        }

        [Test]
        public async Task Create_ShouldReturn500_WhenExceptionThrown()
        {
            var header = new HeaderDTO { ApplicationCode = "APP0000001", UserCode = "USR0000001" };
            var dto = new LogDTO { Level = "Error", UserIp = "186.30.6.144" };
            _serviceMock.Setup(s => s.CreateAsync(header, dto)).ThrowsAsync(new Exception("Database error"));

            var result = await _controller.Create(header, dto);

            var status = result as ObjectResult;
            Assert.IsNotNull(status);
            Assert.AreEqual(500, status.StatusCode);
        }
    }
}
