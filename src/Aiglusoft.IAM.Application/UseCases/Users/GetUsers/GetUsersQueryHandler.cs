using Aiglusoft.IAM.Application.DTOs;
using Aiglusoft.IAM.Application.Queries;
using Aiglusoft.IAM.Application.Services;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using MediatR;

namespace Aiglusoft.IAM.Application.UseCases.Users.GetUsers
{
  public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, ODataResult<object>>
  {
    private readonly IQueryContext context;
    private readonly IMapper mapper;
    private readonly IODataQueryService<UserDto> _oDataQueryService;

    public GetUsersQueryHandler(IQueryContext context, IMapper mapper, IODataQueryService<UserDto> oDataQueryService)
    {
      this.context = context;
      this.mapper = mapper;
      _oDataQueryService = oDataQueryService;
    }

    public async Task<ODataResult<object>> Handle(GetUsersQuery queryOptions, CancellationToken cancellationToken)
    {
      var query = context.Users.ProjectTo<UserDto>(mapper.ConfigurationProvider);

      var result = await _oDataQueryService.ExecuteQueryAsync(
        query: query,
        queryOptions: queryOptions,
        cancellationToken: cancellationToken);

      return result;
    }
  }
}
