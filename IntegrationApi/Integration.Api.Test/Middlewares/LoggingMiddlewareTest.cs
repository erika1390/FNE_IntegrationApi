using Integration.Api.Middlewares;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using Moq;

using Newtonsoft.Json;

using NUnit.Framework;

using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Api.Test.Middlewares
{
    [TestFixture]
    public class LoggingMiddlewareTest
    {
        private Mock<RequestDelegate> _nextMock;
        private Mock<ILogger<LoggingMiddleware>> _loggerMock;
        private LoggingMiddleware _middleware;

        [SetUp]
        public void SetUp()
        {
            _nextMock = new Mock<RequestDelegate>();
            _loggerMock = new Mock<ILogger<LoggingMiddleware>>();
            _middleware = new LoggingMiddleware(_nextMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task Invoke_ShouldLogRequestAndResponse()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var requestBody = "Request body content";
            var responseBody = "Response body content";

            context.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(requestBody));
            var responseStream = new MemoryStream();
            context.Response.Body = responseStream;

            _nextMock.Setup(next => next(It.IsAny<HttpContext>())).Returns(async (HttpContext ctx) =>
            {
                await ctx.Response.WriteAsync(responseBody);
            });

            // Act
            await _middleware.Invoke(context);

            // Assert
            context.Request.Body.Seek(0, SeekOrigin.Begin);
            responseStream.Seek(0, SeekOrigin.Begin);

            var loggedRequestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
            var loggedResponseBody = await new StreamReader(responseStream).ReadToEndAsync();

            _loggerMock.Verify(
                logger => logger.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(requestBody) && v.ToString().Contains(responseBody)),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Test]
        public async Task Invoke_ShouldLogError_WhenExceptionIsThrown()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var requestBody = "Request body content";

            context.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(requestBody));
            context.Response.Body = new MemoryStream();

            _nextMock.Setup(next => next(It.IsAny<HttpContext>())).Throws(new System.Exception("Test exception"));

            // Act
            try
            {
                await _middleware.Invoke(context);
            }
            catch
            {
                // Ignored
            }

            // Assert
            _loggerMock.Verify(
                logger => logger.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error al procesar la solicitud")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}