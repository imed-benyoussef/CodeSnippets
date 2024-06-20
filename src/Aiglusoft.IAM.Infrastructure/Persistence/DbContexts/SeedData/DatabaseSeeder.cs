using Aiglusoft.IAM.Infrastructure.Persistence.DbContexts;
using Aiglusoft.IAM.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Aiglusoft.IAM.Domain;
using Aiglusoft.IAM.Domain.Factories;

namespace Aiglusoft.IAM.Infrastructure.Persistence.DbContexts.SeedData
{
    public class DatabaseSeeder : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public DatabaseSeeder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var userFactory = scope.ServiceProvider.GetRequiredService<IUserFactory>();

            await SeedUsers(context, userFactory);

            await SeedClients(context);
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

                context.Clients.Add(client);
                await context.SaveChangesAsync();

                foreach (var redirectUri in client.RedirectUris)
                {
                    context.ClientRedirectUris.Add(new ClientRedirectUri(client, redirectUri.RedirectUri));
                }

                foreach (var scope in client.Scopes)
                {
                    context.ClientScopes.Add(new ClientScope(client, scope.Scope));
                }

                foreach (var grantType in client.GrantTypes)
                {
                    context.ClientGrantTypes.Add(new ClientGrantType(client, grantType.GrantType));
                }

                await context.SaveChangesAsync();
            }
        }

        public static async Task SeedUsers(AppDbContext context, IUserFactory userFactory)
        {
            if (!context.Users.Any())
            {
                var user = userFactory.CreateUser("testuser", "testuser@example.com", "password");
                user.VerifyEmail();
                context.Users.Add(user);
                await context.SaveChangesAsync();
            }
        }
    }
}
