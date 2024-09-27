using Aiglusoft.IAM.Infrastructure.Persistence.DbContexts;
using Aiglusoft.IAM.Domain.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Aiglusoft.IAM.Domain;
using Aiglusoft.IAM.Domain.Factories;
using Aiglusoft.IAM.Domain.Model.ClientAggregates;
using Aiglusoft.IAM.Infrastructure.Factories;
using Aiglusoft.IAM.Domain.Model.UserAggregates;

namespace Aiglusoft.IAM.Infrastructure.Persistence.DbContexts.SeedData
{
  public class DatabaseSeederForTests : IHostedService
  {
    private readonly IServiceProvider _serviceProvider;

    public DatabaseSeederForTests(IServiceProvider serviceProvider)
    {
      _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
      using var scope = _serviceProvider.CreateScope();
      var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
      var userFactory = scope.ServiceProvider.GetRequiredService<IUserFactory>();


      try
      {
        context.Database.EnsureCreated();
        if (context.Database.GetPendingMigrations().Any())
        {
          context.Database.Migrate();
        }

        await Seed(context, userFactory);

      }
      catch (Exception ex)
      {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        await Seed(context, userFactory);
      }
    }

    async Task Seed(AppDbContext context, IUserFactory userFactory)
    {
      await SeedUsers(context, userFactory);

      await SeedClients(context);

      var codeValidators = context.CodeValidators.Take(1);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private async Task SeedClients(AppDbContext context)
    {
      if (!await context.Clients.AnyAsync())
      {
        var clientId = "postman";
        var clientSecret = "postman-client-secret";
        var clientName = "postman Client";
        var client = new Client(clientId, clientSecret, clientName);
        client.AddRedirectUri("https://oauth.pstmn.io/v1/callback");
        client.AddScope("openid");
        client.AddGrantType("authorization_code");

        context.Set<Client>().Add(client);
        await context.SaveChangesAsync();

        foreach (var redirectUri in client.RedirectUris.ToArray())
        {
          context.Set<ClientRedirectUri>().Add(new ClientRedirectUri(client, redirectUri.RedirectUri));
        }

        foreach (var scope in client.Scopes.ToArray())
        {
          context.Set<ClientScope>().Add(new ClientScope(client, scope.Scope));
        }

        foreach (var grantType in client.GrantTypes.ToArray())
        {
          context.Set<ClientGrantType>().Add(new ClientGrantType(client, grantType.GrantType));
        }

        await context.SaveChangesAsync();
      }
    }

    public static async Task SeedUsers(AppDbContext context, IUserFactory userFactory)
    {
      if (!context.Users.Any())
      {
        var user = userFactory.CreateUser("testuser", "testuser@example.com", "password", "", "", DateOnly.MinValue, null);
        //user.VerifyEmail();
        context.Set<User>().Add(user);
        await context.SaveChangesAsync();
      }
    }
  }
}
