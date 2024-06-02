﻿using System.Net.Http;
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

        [Fact]
        public async Task Authorize_ShouldRedirectWithAuthorizationCode()
        {
            // Arrange
            var client_id = "test_client_id";
            var redirect_uri = "http://localhost";
            var state = "xyz";
            var url = $"/connect/authorize?response_type=code&client_id={client_id}&redirect_uri={System.Web.HttpUtility.UrlEncode(redirect_uri)}&state={state}";
            var client = Factory.CreateClient();
            // Act
            var response = await client.GetAsync(url);

            // Assert
            //response.EnsureSuccessStatusCode();
            var redirectUri = response.Headers.Location;
            Assert.Contains("code=", redirectUri.Query);
            Assert.Contains($"state={state}", redirectUri.Query);
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
