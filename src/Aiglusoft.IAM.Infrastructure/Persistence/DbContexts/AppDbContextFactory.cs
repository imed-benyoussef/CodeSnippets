using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Aiglusoft.IAM.Infrastructure.Persistence.DbContexts
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            // Locate the path to the solution directory
            var basePath = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;

            // Configure the configuration builder to read the appsettings.json file
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("Aiglusoft.IAM.Server/appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            var connectionString = configuration.GetConnectionString("Npgsql");

            optionsBuilder.UseNpgsql(connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}

