using Integration.Api.Middlewares;
using Integration.Application.Interfaces.Security;

using Microsoft.AspNetCore.Http;

using Moq;

namespace Integration.Api.Test.Middlewares
{
    [TestFixture]
    public class HeaderMiddlewareTest
    {
        private Mock<ICurrentUserService> _currentUserServiceMock;
        private Mock<IUserService> _userServiceMock;
        private RequestDelegate _next;
        private HeaderMiddleware _middleware;

        [SetUp]
        public void SetUp()
        {
            _currentUserServiceMock = new Mock<ICurrentUserService>();
            _userServiceMock = new Mock<IUserService>();
            _next = new Mock<RequestDelegate>().Object;
            _middleware = new HeaderMiddleware(_next);
        }

        [Test]
        public async Task Invoke_ShouldSetUserCodeAndUserName_WhenUserCodeHeaderIsPresent()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Request.Headers["UserCode"] = "USR0000001";

            _userServiceMock
                .Setup(x => x.GetUserNameByCodeAsync("USR0000001"))
                .ReturnsAsync("TestUser");

            // Act
            await _middleware.Invoke(context, _currentUserServiceMock.Object, _userServiceMock.Object);

            // Assert
            _currentUserServiceMock.VerifySet(x => x.UserCode = "USR0000001", Times.Once);
            _currentUserServiceMock.VerifySet(x => x.UserName = "TestUser", Times.Once);
            _userServiceMock.Verify(x => x.GetUserNameByCodeAsync("USR0000001"), Times.Once);
        }

        [Test]
        public async Task Invoke_ShouldNotSetUserCodeAndUserName_WhenUserCodeHeaderIsAbsent()
        {
            // Arrange
            var context = new DefaultHttpContext();

            // Act
            await _middleware.Invoke(context, _currentUserServiceMock.Object, _userServiceMock.Object);

            // Assert
            _currentUserServiceMock.VerifySet(x => x.UserCode = It.IsAny<string>(), Times.Never);
            _currentUserServiceMock.VerifySet(x => x.UserName = It.IsAny<string>(), Times.Never);
            _userServiceMock.Verify(x => x.GetUserNameByCodeAsync(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task Invoke_ShouldCallNextMiddleware()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var nextMock = new Mock<RequestDelegate>();
            var middleware = new HeaderMiddleware(nextMock.Object);

            // Act
            await middleware.Invoke(context, _currentUserServiceMock.Object, _userServiceMock.Object);

            // Assert
            nextMock.Verify(x => x(context), Times.Once);
        }
    }
}
