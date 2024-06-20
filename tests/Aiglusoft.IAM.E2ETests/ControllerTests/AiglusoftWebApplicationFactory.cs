namespace Aiglusoft.IAM.E2ETests.ControllerTests
{
    using Aiglusoft.IAM.Infrastructure.Persistence.DbContexts;
    using Aiglusoft.IAM.Server;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Diagnostics;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using System.Data.Common;
    using System.Linq;

    public class AiglusoftWebApplicationFactory
     : WebApplicationFactory<Program> 
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Supprimer la configuration du DbContext existant
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                         typeof(DbContextOptions<AppDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Configurer le DbContext en mémoire
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDatabase");
                    options.ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning));


                });


                services.AddDbContext<AppDbContext>((container, options) =>
                {
                    options.UseInMemoryDatabase("db_test");
                });


                // Récupérer le service du contexte de base de données
                using (var scope = services.BuildServiceProvider().CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    dbContext.Database.EnsureCreated();
                }
            });

            builder.UseEnvironment("Test");
        }
    }
}
