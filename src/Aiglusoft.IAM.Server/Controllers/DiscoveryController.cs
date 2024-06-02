using Aiglusoft.IAM.Application.Queries;
using Aiglusoft.IAM.Application.Queries.GetDiscoveryDocument;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Aiglusoft.IAM.Server.Controllers
{
    [Route(".well-known/openid-configuration")]
    public class DiscoveryController : ControllerBase
    {
        private readonly ISender _sender;

        public DiscoveryController(IMediator sender)
        {
            _sender = sender;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var discoveryDocument = await _sender.Send(new GetDiscoveryDocumentQuery());
            return Ok(discoveryDocument);
        }
    }

}