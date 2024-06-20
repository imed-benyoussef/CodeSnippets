using Aiglusoft.IAM.Application.Queries.GetJwks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Aiglusoft.IAM.Server.Controllers
{
    [ApiController]
    public class JwksController : ControllerBase
    {
        private readonly ISender _sender;

        public JwksController(IMediator mediator)
        {
            _sender = mediator;
        }

        [HttpGet("~/.well-known/jwks.json")]
        public async Task<IActionResult> Get()
        {
            var jwks = await _sender.Send(new GetJwksQuery());
            return Ok(jwks);
        }
    }

}