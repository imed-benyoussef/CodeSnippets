using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Asp.Versioning;
using Aiglusoft.IAM.Application.UseCases.Clients.CreateClient;

namespace Aiglusoft.IAM.Server.Controllers
{
  [ApiVersion(1.0)]
  [ApiController]
  [Route("api/v{version:apiVersion}/clients")]
  public class ClientsController : ControllerBase
  {
    private readonly IMediator _mediator;

    public ClientsController(IMediator mediator)
    {
      _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateClient([FromBody] CreateClientCommand command)
    {
      var result = await _mediator.Send(command);
      return Ok(result);
    }
  }
}