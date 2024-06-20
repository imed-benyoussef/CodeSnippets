using Aiglusoft.IAM.Application.Exceptions;
using Aiglusoft.IAM.Application.Extentions;
using Aiglusoft.IAM.Server.Models;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;

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
            nameof(OidcServerRequest.Prompt) => "prompt",
            nameof(OidcServerRequest.MaxAge) => "max_age",
            nameof(OidcServerRequest.Display) => "display",
            nameof(OidcServerRequest.AcrValues) => "acr_values",
            nameof(OidcServerRequest.IdTokenHint) => "id_token_hint",
            nameof(OidcServerRequest.LoginHint) => "login_hint",
            _ => throw new ArgumentException($"Unknown property: {key}")


        };

        public static string GetBaseUrl(this HttpContext context)
        {
            var request = context.Request;
            var uri = new Uri(request.GetEncodedUrl());
            var baseUri = $"{uri.Scheme}://{uri.Host}";

            if (!(uri.IsDefaultPort || (uri.Scheme == Uri.UriSchemeHttp && uri.Port == 80) || (uri.Scheme == Uri.UriSchemeHttps && uri.Port == 443)))
            {
                baseUri += $":{uri.Port}";
            }

            return baseUri;
        }

        public static string GetAbsoluteUri(this HttpContext context)
        {
            var request = context.Request;
            var uriBuilder = new UriBuilder
            {
                Scheme = request.Scheme,
                Host = request.Host.Host,
                Path = request.Path.ToString(),
                Query = request.QueryString.ToString()
            };

            uriBuilder.RemoveDefaultPort();

            if (request.Host.Port.HasValue)
            {
                uriBuilder.Port = request.Host.Port.Value;
            }

            return uriBuilder.Uri.ToString();
        }
    }
}
