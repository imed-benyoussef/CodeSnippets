using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiglusoft.IAM.Domain.ValueObjects
{
    public class OidcDiscoveryDocument
    {
        public string Issuer { get; set; }
        public string AuthorizationEndpoint { get; set; }
        public string TokenEndpoint { get; set; }
        public string UserinfoEndpoint { get; set; }
        public string JwksUri { get; set; }
        public IEnumerable<string> ResponseTypesSupported { get; set; }
        public IEnumerable<string> SubjectTypesSupported { get; set; }
        public IEnumerable<string> IdTokenSigningAlgValuesSupported { get; set; }
        public IEnumerable<string> ScopesSupported { get; set; }
        public IEnumerable<string> TokenEndpointAuthMethodsSupported { get; set; }
        public IEnumerable<string> ClaimsSupported { get; set; }
    }
}
