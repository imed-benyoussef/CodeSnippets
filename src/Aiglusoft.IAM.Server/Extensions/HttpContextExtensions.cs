using Aiglusoft.IAM.Application.Exceptions;
using Aiglusoft.IAM.Server.Models;
using Azure.Core;
using MediatR;

namespace Aiglusoft.IAM.Server.Extensions
{
    public static class HttpContextExtensions
    {
        public static OidcServerRequest GetOidcServerRequest(this HttpContext context)
        {
            var request = new OidcServerRequest();
            var properties = typeof(OidcServerRequest).GetProperties();

            foreach (var property in properties)
            {
                // Set other properties from Query and Form collections
                string value = context.Request.GetRequestValue(property.Name);
                property.SetValue(request, value);
            }

            return request;
        }

        
        public static bool IsAuthorizationCodeRequest(this OidcServerRequest request)
        {
            // Check required parameters for Authorization Code request
            if (string.IsNullOrEmpty(request.ResponseType) || request.ResponseType != "code")
            {
                return false;
            }

            if (string.IsNullOrEmpty(request.ClientId))
            {
                return false;
            }

            if (string.IsNullOrEmpty(request.RedirectUri))
            {
                return false;
            }

            if (string.IsNullOrEmpty(request.Scope))
            {
                return false;
            }

            if (string.IsNullOrEmpty(request.State))
            {
                return false;
            }

            return true;
        }

        public static void ThrowIfNotValidAuthorizationCodeRequest(this OidcServerRequest request)
        {
            if (string.IsNullOrEmpty(request.ResponseType) || request.ResponseType != "code")
            {
                throw new OAuthException("invalid_request", "The response_type must be 'code'.");
            }

            if (string.IsNullOrEmpty(request.ClientId))
            {
                throw new OAuthException("invalid_request", "The client_id is missing.");
            }

            if (string.IsNullOrEmpty(request.RedirectUri))
            {
                throw new OAuthException("invalid_request", "The redirect_uri is missing.");
            }

            if (string.IsNullOrEmpty(request.Scope))
            {
                throw new OAuthException("invalid_request", "The scope is missing.");
            }

            if (string.IsNullOrEmpty(request.State))
            {
                throw new OAuthException("invalid_request", "The state is missing.");
            }
        }


        private static string GetRequestValue(this HttpRequest request, string key)
        {
            // Normalize the key to match property names
            var propertyName = MapPropertyName(key);

            // Check query parameters first
            if (request.Query.TryGetValue(propertyName, out var queryValue))
            {
                return queryValue.FirstOrDefault() ?? string.Empty;
            }

            // Check form parameters if the request has form content type
            if (request.HasFormContentType && request.Form.TryGetValue(propertyName, out var formValue))
            {
                return formValue.FirstOrDefault() ?? string.Empty;
            }

            return string.Empty;
        }

        static string MapPropertyName(string key) => key switch
        {
            nameof(OidcServerRequest.Code) => "code",
            nameof(OidcServerRequest.ClientId) => "client_id",
            nameof(OidcServerRequest.ClientSecret) => "client_secret",
            nameof(OidcServerRequest.RedirectUri) => "redirect_uri",
            nameof(OidcServerRequest.GrantType) => "grant_type",
            nameof(OidcServerRequest.ResponseType) => "response_type",
            nameof(OidcServerRequest.CodeChallenge) => "code_challenge",
            nameof(OidcServerRequest.CodeChallengeMethod) => "code_challenge_method",
            nameof(OidcServerRequest.State) => "state",
            nameof(OidcServerRequest.Nonce) => "nonce",
            nameof(OidcServerRequest.Scope) => "scope",
            _ => throw new ArgumentException($"Unknown property: {key}")
        };

    }
}
