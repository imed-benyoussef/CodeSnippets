namespace Aiglusoft.IAM.Application.UseCases.Clients.GetClient
{
  using Aiglusoft.IAM.Application.DTOs;
  using Aiglusoft.IAM.Application.Exceptions;
  using Aiglusoft.IAM.Domain.Repositories;
  using AutoMapper;
  using MediatR;
  using Microsoft.Extensions.Localization;

  public class GetClientQueryHandler : IRequestHandler<GetClientQuery, ClientDto>
  {
    private readonly IClientRepository _clientRepository;
    private readonly IStringLocalizer<ErrorMessages> _localizerErrorMessages;
    private readonly IMapper _mapper;

    public GetClientQueryHandler(IClientRepository clientRepository, IStringLocalizer<ErrorMessages> localizerErrorMessages, IMapper mapper)
    {
      _clientRepository = clientRepository;
      _localizerErrorMessages = localizerErrorMessages;
      _mapper = mapper;
    }

    public async Task<ClientDto> Handle(GetClientQuery request, CancellationToken cancellationToken)
    {
      var client = await _clientRepository.GetByIdAsync(request.ClientId);
      if (client == null)
      {
        throw new ClientNotFoundException(_localizerErrorMessages, "ClientNotFound", request.ClientId);
      }

      var result = _mapper.Map<ClientDto>(client);

      return result;
    }
  }
}
