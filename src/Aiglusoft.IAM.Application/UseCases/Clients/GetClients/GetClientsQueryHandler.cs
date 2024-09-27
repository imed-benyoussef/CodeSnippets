using Aiglusoft.IAM.Application.DTOs;
using Aiglusoft.IAM.Application.Queries;
using Aiglusoft.IAM.Application.Services;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

namespace Aiglusoft.IAM.Application.UseCases.Clients.GetClients
{
  public class GetClientsQueryHandler : IRequestHandler<GetClientsQuery, ODataResult<object>>
  {
    private readonly IQueryContext context;
    private readonly IMapper mapper;
    private readonly IODataQueryService<ClientDto> _oDataQueryService;

    public GetClientsQueryHandler(IQueryContext context, IMapper mapper, IODataQueryService<ClientDto> oDataQueryService)
    {
      this.context = context;
      this.mapper = mapper;
      _oDataQueryService = oDataQueryService;
    }

    public async Task<ODataResult<object>> Handle(GetClientsQuery queryOptions, CancellationToken cancellationToken)
    {
      var query = context.Clients.ProjectTo<ClientDto>(mapper.ConfigurationProvider);

      var result = await _oDataQueryService.ExecuteQueryAsync(
        query: query,
        queryOptions: queryOptions,
        cancellationToken: cancellationToken);

      return result;
    }
  }
}
