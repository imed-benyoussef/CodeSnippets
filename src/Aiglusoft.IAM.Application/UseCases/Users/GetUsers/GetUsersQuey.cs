using Aiglusoft.IAM.Application.DTOs;
using Aiglusoft.IAM.Application.Queries;
using Aiglusoft.IAM.Application.UseCases.Clients.GetClients;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiglusoft.IAM.Application.UseCases.Users.GetUsers
{
  public class GetUsersQuery : ODataQueryOptions<UserDto>, IRequest<ODataResult<object>>
  {
  }
}
