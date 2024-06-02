using Aiglusoft.IAM.Application;
using Aiglusoft.IAM.Application.Behaviors;
using Aiglusoft.IAM.Application.Commands;
using Aiglusoft.IAM.Domain;
using Aiglusoft.IAM.Domain.Repositories;
using Aiglusoft.IAM.Domain.Services;
using Aiglusoft.IAM.Infrastructure.Repositories;
using Aiglusoft.IAM.Infrastructure.Services;
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
using System.Reflection;
using System.Text;


namespace Aiglusoft.IAM.Server
{
    public partial class Program
    {
        public static void Main(string[] args)
        {
            // Créer une configuration Serilog
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(GetConfiguration())
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            try
            {
                Log.Information("Starting web host");

                var builder = WebApplication.CreateBuilder(args);

                // Utiliser Serilog comme le fournisseur de logging
                builder.Host.UseSerilog();

                // Ajouter des services au conteneur.
                builder.Services.AddControllers();
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                builder.Services.AddValidatorsFromAssembly(typeof(Application.ApplicationLayer).Assembly);

                // Register MediatR
                builder.Services.AddMediatR(typeof(ApplicationLayer).Assembly);
                builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

                builder.Services.AddScoped<IUserRepository, UserRepository>();
                builder.Services.AddScoped<IAuthorizationCodeRepository, AuthorizationCodeRepository>();


                builder.Services.AddSingleton<ICertificateService, CertificateService>();
                builder.Services.AddSingleton<IJwtTokenService, JwtTokenService>();       
                builder.Services.AddScoped<IDiscoveryService, DiscoveryService>();
                builder.Services.AddScoped<IJwksService, JwksService>();         

                  builder.Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    var serviceProvider = builder.Services.BuildServiceProvider();
                    var certificateService = serviceProvider.GetRequiredService<ICertificateService>();
                    var rsa = certificateService.GetRsaPrivateKey();
                    var key = new RsaSecurityKey(rsa)
                    {
                        KeyId = certificateService.GetKeyId()
                    };

                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = key
                    };
                });


                var app = builder.Build();

                app.UseDefaultFiles();
                app.UseStaticFiles();

                // Configurer le pipeline de requêtes HTTP.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();

                app.UseRouting();

                app.UseAuthentication();
                app.UseAuthorization();

                app.MapControllers();

                app.MapFallbackToFile("/index.html");

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IConfiguration GetConfiguration()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }
    }
}
