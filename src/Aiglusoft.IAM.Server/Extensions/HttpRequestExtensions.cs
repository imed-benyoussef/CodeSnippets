using Aiglusoft.IAM.Server.Models;
using Microsoft.Extensions.Primitives;

namespace Aiglusoft.IAM.Server.Extensions
{
    public static class HttpRequestExtensions
    {
        public static AuthorizeRequest ToAuthorizeRequest(this IQueryCollection query)
        {
            return new AuthorizeRequest
            {
                ResponseType = query.TryGetValue("response_type", out StringValues responseType) ? responseType.FirstOrDefault() : string.Empty,
                ClientId = query.TryGetValue("client_id", out StringValues clientId) ? clientId.FirstOrDefault() : string.Empty,
                RedirectUri = query.TryGetValue("redirect_uri", out StringValues redirectUri) ? redirectUri.FirstOrDefault() : string.Empty,
                CodeChallenge = query.TryGetValue("code_challenge", out StringValues codeChallenge) ? codeChallenge.FirstOrDefault() : string.Empty,
                CodeChallengeMethod = query.TryGetValue("code_challenge_method", out StringValues codeChallengeMethod) ? codeChallengeMethod.FirstOrDefault() : string.Empty,
                State = query.TryGetValue("state", out StringValues state) ? state.FirstOrDefault() : string.Empty,
                Nonce = query.TryGetValue("nonce", out StringValues nonce) ? nonce.FirstOrDefault() : string.Empty,
                Scope = query.TryGetValue("scope", out StringValues scope) ? scope.FirstOrDefault() : string.Empty
            };
        }


        public static TokenRequest ToTokenRequest(this IFormCollection form)
        {
            return new TokenRequest
            {
                Code = form.TryGetValue("code", out StringValues code) ? code.FirstOrDefault() : string.Empty,
                ClientId = form.TryGetValue("client_id", out StringValues clientId) ? clientId.FirstOrDefault() : string.Empty,
                ClientSecret = form.TryGetValue("client_secret", out StringValues clientSecret) ? clientSecret.FirstOrDefault() : string.Empty,
                RedirectUri = form.TryGetValue("redirect_uri", out StringValues redirectUri) ? redirectUri.FirstOrDefault() : string.Empty,
                GrantType = form.TryGetValue("grant_type", out StringValues grantType) ? grantType.FirstOrDefault() : string.Empty
            };
        }
    }
}
