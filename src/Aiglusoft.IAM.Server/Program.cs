using Aiglusoft.IAM.Application;
using Aiglusoft.IAM.Application.Behaviors;
using Aiglusoft.IAM.Application.Commands;
using Aiglusoft.IAM.Domain;
using Aiglusoft.IAM.Domain.Repositories;
using Aiglusoft.IAM.Domain.Services;
using Aiglusoft.IAM.Infrastructure.Repositories;
using Aiglusoft.IAM.Infrastructure.Services;
using Aiglusoft.IAM.Server.Middleware;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;

namespace Aiglusoft.IAM.Server
{
    public partial class Program
    {
        public static void Main(string[] args)
        {
            // Configurer le logging avec Serilog
            ConfigureLogging();

            try
            {
                Log.Information("Starting web host");

                // Créer le builder de l'application
                var builder = WebApplication.CreateBuilder(args);

                // Configurer l'hôte pour utiliser Serilog
                ConfigureHost(builder);

                // Ajouter et configurer les services
                ConfigureServices(builder);

                // Construire l'application
                var app = builder.Build();

                // Configurer les middlewares de l'application
                ConfigureMiddleware(app);

                // Lancer l'application
                app.Run();
            }
            catch (Exception ex)
            {
                // Logger l'exception si l'hôte se termine de manière inattendue
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                // Fermer et vider le logger Serilog
                Log.CloseAndFlush();
            }
        }

        private static void ConfigureLogging()
        {
            // Configurer Serilog pour le logging
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(GetConfiguration())
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

        private static void ConfigureHost(WebApplicationBuilder builder)
        {
            // Configurer l'hôte pour utiliser Serilog comme fournisseur de logging
            builder.Host.UseSerilog();
        }

        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            var services = builder.Services;

            // Ajouter les services MVC et autres services nécessaires
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddValidatorsFromAssembly(typeof(Application.ApplicationLayer).Assembly);

            // Ajouter MediatR pour le traitement des commandes et des requêtes
            services.AddMediatR(typeof(ApplicationLayer).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            // Ajouter les dépendances pour les repositories et services
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAuthorizationCodeRepository, AuthorizationCodeRepository>();
            services.AddSingleton<ICertificateService, CertificateService>();
            services.AddSingleton<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IDiscoveryService, DiscoveryService>();
            services.AddScoped<IJwksService, JwksService>();
            services.AddSingleton<ExceptionMapping>();

            // Configurer l'authentification et la validation des tokens JWT
            ConfigureAuthentication(services, builder.Configuration);
        }

        private static void ConfigureAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var serviceProvider = services.BuildServiceProvider();
                var certificateService = serviceProvider.GetRequiredService<ICertificateService>();
                var rsa = certificateService.GetRsaPrivateKey();
                var key = new RsaSecurityKey(rsa)
                {
                    KeyId = certificateService.GetKeyId()
                };

                // Configurer les paramètres de validation des tokens JWT
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = key
                };
            });
        }

        private static void ConfigureMiddleware(WebApplication app)
        {
            // Configurer les fichiers par défaut et les fichiers statiques
            app.UseDefaultFiles();
            app.UseStaticFiles();


            // Configurer Swagger pour l'environnement de développement
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ErrorMiddleware>();
            app.UseMiddleware<OidcServerRequestMiddleware>();

            // Rediriger les requêtes HTTP vers HTTPS
            app.UseHttpsRedirection();

            // Configurer le routage
            app.UseRouting();

            // Ajouter l'authentification et l'autorisation
            app.UseAuthentication();
            app.UseAuthorization();

            // Mapper les contrôleurs
            app.MapControllers();

            // Mapper les requêtes de fallback vers index.html
            app.MapFallbackToFile("/index.html");
        }

        private static IConfiguration GetConfiguration()
        {
            // Récupérer l'environnement actuel
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

            // Construire la configuration en utilisant les fichiers appsettings.json et les variables d'environnement
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }
    }
}
