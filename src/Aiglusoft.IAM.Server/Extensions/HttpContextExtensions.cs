using Aiglusoft.IAM.Server.Models;

namespace Aiglusoft.IAM.Server.Extensions
{
    public static class HttpContextExtensions
    {
        public static OAuthServerRequest GetOAuthServerRequest(this HttpContext context)
        {
            var request = new OAuthServerRequest();
            var properties = typeof(OAuthServerRequest).GetProperties();

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
           "Code"              =>  "code"                   ,
           "ClientId"           => "client_id"               ,
           "ClientSecret"       => "client_secret"           ,
           "RedirectUri"        => "redirect_uri"            ,
           "GrantType"          => "grant_type"              ,

           "ResponseType"       => "response_type"           ,
           "CodeChallenge"      => "code_challenge"          ,
           "CodeChallengeMethod"=> "code_challenge_method"   ,
           "State"              => "state"                   ,
           "Nonce"              => "nonce"                   ,
           "Scope"              => "scope"                   ,
        };
    }
}
