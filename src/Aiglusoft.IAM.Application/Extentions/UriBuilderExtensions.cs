

namespace Aiglusoft.IAM.Application.Extentions
{
    using System;
    using System.Web;

    public static class UriBuilderExtensions
    {
        public static void RemoveDefaultPort(this UriBuilder uriBuilder)
        {
            if ((uriBuilder.Scheme == Uri.UriSchemeHttp && uriBuilder.Port == 80) ||
                (uriBuilder.Scheme == Uri.UriSchemeHttps && uriBuilder.Port == 443))
            {
                uriBuilder.Port = -1;  // Avoid including the default port
            }
        }

        public static void AddOrUpdateQuery(this UriBuilder uriBuilder, string name, string value)
        {
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query[name] = value;
            uriBuilder.Query = query.ToString();
        }
    }

}
