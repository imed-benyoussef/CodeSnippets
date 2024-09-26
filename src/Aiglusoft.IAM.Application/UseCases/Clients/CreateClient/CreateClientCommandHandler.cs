namespace Aiglusoft.IAM.Application.UseCases.Clients.CreateClient
{
  using Aiglusoft.IAM.Domain.Model.ClientAggregates;
  using Aiglusoft.IAM.Domain.Repositories;
  using MediatR;
  using System.Security.Cryptography;

  public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, string>
  {
    private readonly IClientRepository _clientRepository;

    public CreateClientCommandHandler(IClientRepository clientRepository)
    {
      _clientRepository = clientRepository;
    }

    public async Task<string> Handle(CreateClientCommand request, CancellationToken cancellationToken)
    {
      // Générer automatiquement le ClientSecret
      string generatedClientSecret = GenerateClientSecret();

      // Créer une nouvelle instance de Client avec le ClientSecret généré
      var client = new Client(request.ClientId, generatedClientSecret, request.ClientName);

      // Ajouter les RedirectUris
      foreach (var uri in request.RedirectUris)
      {
        client.AddRedirectUri(uri);
      }

      // Ajouter les Scopes
      foreach (var scope in request.Scopes)
      {
        client.AddScope(scope);
      }

      // Ajouter les GrantTypes
      foreach (var grantType in request.GrantTypes)
      {
        client.AddGrantType(grantType);
      }

      // Sauvegarder le client dans le référentiel (repository)
      await _clientRepository.AddAsync(client);

      // Retourner l'ID du client nouvellement créé
      return generatedClientSecret; // Vous pouvez retourner le vrai ID du client après l'ajout dans le référentiel
    }

    private string GenerateClientSecret(int length = 32)
    {
      using (var rng = new RNGCryptoServiceProvider())
      {
        var byteArray = new byte[length];
        rng.GetBytes(byteArray);
        return Convert.ToBase64String(byteArray);
      }
    }
  }

}
