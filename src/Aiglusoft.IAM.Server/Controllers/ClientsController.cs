using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Asp.Versioning;
using Aiglusoft.IAM.Application.UseCases.Clients.CreateClient;
using Aiglusoft.IAM.Application.UseCases.Clients.GetClients;
using static Microsoft.AspNetCore.Http.StatusCodes;
using Aiglusoft.IAM.Application.UseCases.Clients.GetClient;

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

    [HttpGet]
    public async Task<IActionResult> Get(
           [FromQuery(Name = "$filter")] string filter = null,      // Filtrage (ex: "title eq 'Act 1'")
           [FromQuery(Name = "$orderby")] string orderBy = null,    // Tri (ex: "title asc")
           [FromQuery(Name = "$top")] int top = 10,                 // Pagination (nombre d'éléments par page)
           [FromQuery(Name = "$skip")] int skip = 0,                // Pagination (éléments à ignorer)
           [FromQuery(Name = "$select")] string select = null       // Sélection de champs (ex: "title,description")
       )
    {
      // Validate the input values for pagination
      if (top <= 0 || skip < 0)
      {
        return BadRequest("Invalid pagination parameters.");
      }

      try
      {
        // Créer une requête GetAllActsQuery avec les paramètres OData-like
        var query = new GetClientsQuery { Filter = filter, OrderBy = orderBy, Top = top, Skip = skip, Select = select };

        // Envoyer la requête via MediatR
        var result = await _mediator.Send(query);

        // Retourner les résultats
        return Ok(result);
      }
      catch (Exception ex)
      {
        // Log the exception if needed
        return StatusCode(Status500InternalServerError, "An error occurred while processing your request.");
      }
    }

    [HttpPost]
    public async Task<IActionResult> CreateClient([FromBody] CreateClientCommand command)
    {
      var result = await _mediator.Send(command);
      return Ok(result);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetClient(string id)
    {
   
        var query = new GetClientQuery(id);
        var result = await _mediator.Send(query);
        return Ok(result);

    }
  }

}