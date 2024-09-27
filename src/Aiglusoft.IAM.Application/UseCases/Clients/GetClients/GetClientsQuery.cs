using Aiglusoft.IAM.Application.DTOs;
using Aiglusoft.IAM.Application.Queries;
using Aiglusoft.IAM.Domain.Model.ClientAggregates;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiglusoft.IAM.Application.UseCases.Clients.GetClients
{
  public class GetClientsQuery : ODataQueryOptions<ClientDto>, IRequest<ODataResult<object>>
  {
  }
}
