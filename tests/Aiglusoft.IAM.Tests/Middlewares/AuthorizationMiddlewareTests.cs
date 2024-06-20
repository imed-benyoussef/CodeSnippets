//using Aiglusoft.IAM.Application.UseCases.Authorization.GenerateAuthorizationCode;
//using Aiglusoft.IAM.Server.Middleware;
//using FluentAssertions;
//using MediatR;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.DependencyInjection;
//using Moq;
//using System.Net;

//namespace Aiglusoft.IAM.Tests.Middlewares
//{
//    public class AuthorizationMiddlewareTests
//    {
//        [Fact]
//        public async Task InvokeAsync_ShouldRedirectWithAuthorizationCode_WhenRequestIsValid()
//        {
//            // Arrange
//            var mockSender = new Mock<ISender>();
//            mockSender.Setup(sender => sender.Send(It.IsAny<GenerateAuthorizationCodeCommand>(), It.IsAny<CancellationToken>()))
//                      .ReturnsAsync("https://your-app.com/callback?code=authorization_code&state=random_state_string");

//            var context = new DefaultHttpContext();
//            context.Request.Path = "/connect/authorize";
//            context.Request.QueryString = new QueryString("?client_id=client_id&redirect_uri=https://your-app.com/callback&state=random_state_string&response_type=code&scope=openid&nonce=random_nonce&code_challenge=random_challenge&code_challenge_method=S256");
//            context.RequestServices = new ServiceCollection()
//                .AddSingleton(mockSender.Object)
//                .BuildServiceProvider();

//            var middleware = new OidcServerRequestMiddleware((innerHttpContext) => Task.CompletedTask);

//            // Act
//            await middleware.InvokeAsync(context);

//            // Assert
//            context.Response.StatusCode.Should().Be((int)HttpStatusCode.Redirect);
//            context.Response.Headers["Location"].ToString().Should().Be("https://your-app.com/callback?code=authorization_code&state=random_state_string");
//        }

//        //[Fact]
//        //public async Task InvokeAsync_ShouldRedirectWithError_WhenOAuthExceptionIsThrown()
//        //{
//        //    // Arrange
//        //    var mockSender = new Mock<ISender>();
//        //    mockSender.Setup(sender => sender.Send(It.IsAny<GenerateAuthorizationCodeCommand>(), It.IsAny<CancellationToken>()))
//        //              .ThrowsAsync(new OAuthException("invalid_request", "Invalid request parameters."));

//        //    var context = new DefaultHttpContext();
//        //    context.Request.Path = "/connect/authorize";
//        //    context.Request.QueryString = new QueryString("?client_id=client_id&redirect_uri=https://your-app.com/callback&state=random_state_string&response_type=code&scope=openid&nonce=random_nonce&code_challenge=random_challenge&code_challenge_method=S256");
//        //    context.RequestServices = new ServiceCollection()
//        //        .AddSingleton(mockSender.Object)
//        //        .BuildServiceProvider();

//        //    var middleware = new OidcServerRequestMiddleware((innerHttpContext) => Task.CompletedTask);

//        //    // Act
//        //    await middleware.InvokeAsync(context);

//        //    // Assert
//        //    context.Response.StatusCode.Should().Be((int)HttpStatusCode.Redirect);
//        //    var locationHeader = context.Response.Headers["Location"].ToString();
//        //    locationHeader.Should().Contain("https://your-app.com/callback");
//        //    locationHeader.Should().Contain("error=invalid_request");
//        //    locationHeader.Should().Contain("state=random_state_string");
//        //}

//        [Fact]
//        public async Task InvokeAsync_ShouldCallNextMiddleware_WhenPathIsNotAuthorize()
//        {
//            // Arrange
//            var nextCalled = false;
//            var mockSender = new Mock<ISender>();

//            var context = new DefaultHttpContext();
//            context.Request.Path = "/other/path";
//            context.RequestServices = new ServiceCollection()
//                .AddSingleton(mockSender.Object)
//                .BuildServiceProvider();

//            var middleware = new OidcServerRequestMiddleware((innerHttpContext) =>
//            {
//                nextCalled = true;
//                return Task.CompletedTask;
//            });

//            // Act
//            await middleware.InvokeAsync(context);

//            // Assert
//            nextCalled.Should().BeTrue();
//        }
//    }
//}
