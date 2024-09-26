namespace Aiglusoft.IAM.Application.UseCases.Clients.CreateClient
{
  using MediatR;

  public class CreateClientCommand : IRequest<string>
  {
    public string ClientId { get; }
    public string ClientName { get; }
    public List<string> RedirectUris { get; }
    public List<string> Scopes { get; }
    public List<string> GrantTypes { get; }

    public CreateClientCommand(string clientId, string clientName, List<string> redirectUris, List<string> scopes, List<string> grantTypes)
    {
      if (string.IsNullOrWhiteSpace(clientId)) throw new ArgumentException("Client ID cannot be null or empty", nameof(clientId));
      if (string.IsNullOrWhiteSpace(clientName)) throw new ArgumentException("Client Name cannot be null or empty", nameof(clientName));

      ClientId = clientId;
      ClientName = clientName;
      RedirectUris = redirectUris ?? new List<string>();
      Scopes = scopes ?? new List<string>();
      GrantTypes = grantTypes ?? new List<string>();
    }
  }

}
