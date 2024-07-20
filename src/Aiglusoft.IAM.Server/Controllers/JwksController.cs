using Aiglusoft.IAM.Application.Queries.GetJwks;
using Aiglusoft.IAM.Infrastructure.Services;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace Aiglusoft.IAM.Server.Controllers
{
    [ApiVersionNeutral]
    [ApiController]
    public class JwksController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly RSAKeyService _rsaKeyService;
        public JwksController(IMediator mediator, RSAKeyService rsaKeyService)
        {
            _sender = mediator;
            _rsaKeyService = rsaKeyService;
        }

        [HttpGet("~/.well-known/jwks.json")]
        public async Task<IActionResult> Get()
        {
            using (var rsa = RSA.Create(2048))
            {
                var parameters = rsa.ExportParameters(false); // Exporter uniquement la clé publique

                var key = new JWKKey
                {
                    Kty = "RSA",
                    Kid = Guid.NewGuid().ToString(),
                    N = Convert.ToBase64String(parameters.Modulus),
                    E = Convert.ToBase64String(parameters.Exponent),
                    Alg = "RS256",
                    Use = "enc"
                };

                var jwks = new JWKSResponse
                {
                    Keys = new List<JWKKey> { key }
                };

                return Ok(jwks);
            }
        }

    }

    public class JWKKey
    {
        public string Kty { get; set; }
        public string Kid { get; set; }
        public string N { get; set; }
        public string E { get; set; }
        public string Alg { get; set; }
        public string Use { get; set; }
    }

    public class JWKSResponse
    {
        public List<JWKKey> Keys { get; set; }
    }
}