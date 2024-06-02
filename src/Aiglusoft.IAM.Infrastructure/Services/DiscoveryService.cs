using Aiglusoft.IAM.Domain.Services;
using Aiglusoft.IAM.Domain.ValueObjects;
using Microsoft.Extensions.Configuration;

namespace Aiglusoft.IAM.Infrastructure.Services
{
    public class DiscoveryService : IDiscoveryService
    {
        private readonly IConfiguration _configuration;

        public DiscoveryService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public OidcDiscoveryDocument GetDiscoveryDocument()
        {
            var issuer = _configuration["Oidc:Issuer"];
            return new OidcDiscoveryDocument
            {
                Issuer = issuer,
                AuthorizationEndpoint = $"{issuer}/connect/authorize",
                TokenEndpoint = $"{issuer}/connect/token",
                UserinfoEndpoint = $"{issuer}/connect/userinfo",
                JwksUri = $"{issuer}/.well-known/jwks.json",
                ResponseTypesSupported = new[] { "code", "token", "id_token", "code token", "code id_token", "id_token token", "code id_token token" },
                SubjectTypesSupported = new[] { "public" },
                IdTokenSigningAlgValuesSupported = new[] { "RS256" },
                ScopesSupported = new[] { "openid", "profile", "email" },
                TokenEndpointAuthMethodsSupported = new[] { "client_secret_basic", "client_secret_post" },
                ClaimsSupported = new[] { "sub", "name", "preferred_username", "email", "picture" }
            };
        }
    }
}
