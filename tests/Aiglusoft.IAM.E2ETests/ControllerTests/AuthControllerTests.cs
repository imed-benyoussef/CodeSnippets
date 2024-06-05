using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Aiglusoft.IAM.Server;
using System.Collections.Generic;
using Moq;
using Aiglusoft.IAM.Domain.Repositories;
using Aiglusoft.IAM.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using System;
using System.Web;
using AutoFixture.Xunit2;
using System.Net;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;
using Aiglusoft.IAM.Infrastructure.Repositories;
using AutoFixture;
using AutoFixture.Xunit2;
using System.Web;
using FluentAssertions;
using Aiglusoft.IAM.Application.Commands.GenerateAuthorizationCode;
using Aiglusoft.IAM.Application.Exceptions;
using MediatR;

namespace Aiglusoft.IAM.E2ETests.ControllerTests
{
    public class AuthControllerTests : BaseIntegrationTest
    {
        private readonly Mock<IAuthorizationCodeRepository> _authorizationCodeRepositoryMock;

        public AuthControllerTests(WebApplicationFactory<Program> factory)
            : base(factory)
        {
            _authorizationCodeRepositoryMock = new Mock<IAuthorizationCodeRepository>();
        }

        protected override void ConfigureTestServices(IServiceCollection services)
        {
            services.AddSingleton(_authorizationCodeRepositoryMock.Object);

        }

        [Theory, AutoData]
        public async Task AuthorizeOAuth2_ShouldRedirectWithAuthorizationCode(string client_id, string state, string scope)
        {
            // Arrange
            var redirect_uri = "http://localhost";
            var url = $"/connect/authorize?response_type=code&client_id={client_id}&redirect_uri={HttpUtility.UrlEncode(redirect_uri)}&scope={scope}&state={state}";

            var client = Factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Redirect);

            var redirectUri = response.Headers.Location;
            redirectUri.Should().NotBeNull();
            redirectUri.Query.Should().Contain("code=");
            redirectUri.Query.Should().Contain($"state={state}");
        }

        [Theory, AutoData]
        public async Task AuthorizeOidc_ShouldRedirectWithIdToken(string client_id, string state, string scope)
        {
            // Arrange
            var redirect_uri = "http://localhost:8888";
            var response_type = "id_token";
            var nonce = "random_nonce";
            var url = $"/connect/authorize?response_type={response_type}&client_id={client_id}&redirect_uri={HttpUtility.UrlEncode(redirect_uri)}&scope={scope}&state={state}&nonce={nonce}";

            var client = Factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Redirect);

            var redirectUri = response.Headers.Location;
            redirectUri.Should().NotBeNull();
            redirectUri.Query.Should().Contain("id_token=");
            redirectUri.Query.Should().Contain($"state={state}");
            redirectUri.Query.Should().Contain($"nonce={nonce}");
        }

        [Theory, AutoData]
        public async Task AuthorizeOAuth2_ShouldRedirectWithError_WhenOAuthExceptionIsThrown(string client_id, string state, string scope)
        {
            // Arrange
            var redirect_uri = "http://localhost:8888";
            var url = $"/connect/authorize?response_type=code&client_id={client_id}&redirect_uri={HttpUtility.UrlEncode(redirect_uri)}&scope={scope}&state={state}";

            var mockSender = new Mock<ISender>();
            mockSender.Setup(sender => sender.Send(It.IsAny<GenerateAuthorizationCodeCommand>(), It.IsAny<CancellationToken>()))
                      .ThrowsAsync(new OAuthException("invalid_request", "Invalid request parameters."));

            var client = Factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton(mockSender.Object);
                });
            }).CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Redirect);
            var redirectUri = response.Headers.Location;
            redirectUri.Should().NotBeNull();
            redirectUri.Query.Should().Contain("error=invalid_request");
            redirectUri.Query.Should().Contain($"state={state}");
        }

        [Fact]
        public async Task Token_ShouldReturnIdAndAccessToken()
        {
            // Arrange
            var authorizationCode = "test_code";
            var authorizationCodeEntity = new AuthorizationCode("test_client_id");
            _authorizationCodeRepositoryMock
                .Setup(repo => repo.GetAsync(authorizationCode))
                .ReturnsAsync(authorizationCodeEntity);

            var tokenRequest = new Dictionary<string, string>
            {
                { "code", authorizationCode },
                { "client_id", "test_client_id" },
                { "client_secret", "test_client_secret" },
                { "redirect_uri", "http://localhost/callback" },
                { "grant_type", "authorization_code" }
            };

            var content = new FormUrlEncodedContent(tokenRequest);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var client = Factory.CreateClient();
            // Act
            var response = await client.PostAsync("/connect/token", content);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseString);
            Assert.False(string.IsNullOrEmpty(tokenResponse.IdToken));
            Assert.False(string.IsNullOrEmpty(tokenResponse.AccessToken));
        }

        private class TokenResponse
        {
            public string IdToken { get; set; }
            public string AccessToken { get; set; }
            public string TokenType { get; set; }
            public int ExpiresIn { get; set; }
        }
    }


}
