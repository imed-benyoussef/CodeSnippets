namespace Aiglusoft.IAM.Application.UseCases.Clients.GetClient
{
  using Aiglusoft.IAM.Application.DTOs;
  using MediatR;

  public class GetClientQuery : IRequest<ClientDto>
  {
    public string ClientId { get; }

    public GetClientQuery(string clientId)
    {
      ClientId = clientId ?? throw new ArgumentNullException(nameof(clientId));
    }
  }
}
