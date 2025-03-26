using Integration.Api.Middlewares;
using Integration.Application.Exceptions;
using Integration.Shared.Response;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using Moq;

using Newtonsoft.Json;

using System.Net;

namespace Integration.Api.Test.Middlewares
{
    [TestFixture]
    public class ExceptionHandlingMiddlewareTest
    {
        private Mock<RequestDelegate> _nextMock;
        private Mock<ILogger<ExceptionHandlingMiddleware>> _loggerMock;
        private ExceptionHandlingMiddleware _middleware;

        [SetUp]
        public void SetUp()
        {
            _nextMock = new Mock<RequestDelegate>();
            _loggerMock = new Mock<ILogger<ExceptionHandlingMiddleware>>();
            _middleware = new ExceptionHandlingMiddleware(_nextMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task Invoke_ShouldReturnNotFound_WhenNotFoundExceptionIsThrown()
        {
            // Arrange
            _nextMock.Setup(next => next(It.IsAny<HttpContext>())).Throws(new NotFoundException("Recurso no encontrado."));
            var context = new DefaultHttpContext();
            var responseStream = new MemoryStream();
            context.Response.Body = responseStream;

            // Act
            await _middleware.Invoke(context);

            // Assert
            Assert.AreEqual((int)HttpStatusCode.NotFound, context.Response.StatusCode);
            responseStream.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(responseStream).ReadToEndAsync();
            var response = JsonConvert.DeserializeObject<ResponseApi<string>>(responseBody);
            Assert.AreEqual("Recurso no encontrado.", response.Errors.ToList()[0]);
        }

        [Test]
        public async Task Invoke_ShouldReturnBadRequest_WhenValidationExceptionIsThrown()
        {
            // Arrange
            _nextMock.Setup(next => next(It.IsAny<HttpContext>())).Throws(new ValidationException("Solicitud inválida."));
            var context = new DefaultHttpContext();
            var responseStream = new MemoryStream();
            context.Response.Body = responseStream;

            // Act
            await _middleware.Invoke(context);

            // Assert
            Assert.AreEqual((int)HttpStatusCode.BadRequest, context.Response.StatusCode);
            responseStream.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(responseStream).ReadToEndAsync();
            var response = JsonConvert.DeserializeObject<ResponseApi<string>>(responseBody);
            Assert.AreEqual("Solicitud inválida.", response.Errors.ToList()[0]);
        }

        [Test]
        public async Task Invoke_ShouldReturnUnauthorized_WhenUnauthorizedExceptionIsThrown()
        {
            // Arrange
            _nextMock.Setup(next => next(It.IsAny<HttpContext>())).Throws(new UnauthorizedException("No autorizado."));
            var context = new DefaultHttpContext();
            var responseStream = new MemoryStream();
            context.Response.Body = responseStream;

            // Act
            await _middleware.Invoke(context);

            // Assert
            Assert.AreEqual((int)HttpStatusCode.Unauthorized, context.Response.StatusCode);
            responseStream.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(responseStream).ReadToEndAsync();
            var response = JsonConvert.DeserializeObject<ResponseApi<string>>(responseBody);
            Assert.AreEqual("No autorizado.", response.Errors.ToList()[0]);
        }

        [Test]
        public async Task Invoke_ShouldReturnConflict_WhenBusinessExceptionIsThrown()
        {
            // Arrange
            _nextMock.Setup(next => next(It.IsAny<HttpContext>())).Throws(new BusinessException("Error de negocio."));
            var context = new DefaultHttpContext();
            var responseStream = new MemoryStream();
            context.Response.Body = responseStream;

            // Act
            await _middleware.Invoke(context);

            // Assert
            Assert.AreEqual((int)HttpStatusCode.Conflict, context.Response.StatusCode);
            responseStream.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(responseStream).ReadToEndAsync();
            var response = JsonConvert.DeserializeObject<ResponseApi<string>>(responseBody);
            Assert.AreEqual("Error de negocio.", response.Errors.ToList()[0]);
        }

        [Test]
        public async Task Invoke_ShouldReturnBadGateway_WhenHttpRequestExceptionIsThrown()
        {
            // Arrange
            _nextMock.Setup(next => next(It.IsAny<HttpContext>())).Throws(new HttpRequestException("Error de red."));
            var context = new DefaultHttpContext();
            var responseStream = new MemoryStream();
            context.Response.Body = responseStream;

            // Act
            await _middleware.Invoke(context);

            // Assert
            Assert.AreEqual((int)HttpStatusCode.BadGateway, context.Response.StatusCode);
            responseStream.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(responseStream).ReadToEndAsync();
            var response = JsonConvert.DeserializeObject<ResponseApi<string>>(responseBody);
            Assert.AreEqual("Error de red.", response.Errors.ToList()[0]);
        }

        [Test]
        public async Task Invoke_ShouldReturnInternalServerError_WhenUnexpectedExceptionIsThrown()
        {
            // Arrange
            _nextMock.Setup(next => next(It.IsAny<HttpContext>())).Throws(new Exception("Error inesperado."));
            var context = new DefaultHttpContext();
            var responseStream = new MemoryStream();
            context.Response.Body = responseStream;

            // Act
            await _middleware.Invoke(context);

            // Assert
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, context.Response.StatusCode);
            responseStream.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(responseStream).ReadToEndAsync();
            var response = JsonConvert.DeserializeObject<ResponseApi<string>>(responseBody);
            Assert.AreEqual("Ha ocurrido un error inesperado.", response.Errors.ToList()[0]);
        }
    }
}