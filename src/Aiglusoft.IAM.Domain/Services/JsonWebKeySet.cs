using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiglusoft.IAM.Domain
{
    public class JsonWebKeySet
    {
        public IEnumerable<JsonWebKey> Keys { get; set; }
    }

    public class JsonWebKey
    {
        public string Kty { get; set; }
        public string Use { get; set; }
        public string Kid { get; set; }
        public string E { get; set; }
        public string N { get; set; }
    }

}
