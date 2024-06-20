using Aiglusoft.IAM.Application;
using Aiglusoft.IAM.Application.Behaviors;
using Aiglusoft.IAM.Application.Commands;
using Aiglusoft.IAM.Domain;
using Aiglusoft.IAM.Domain.Factories;
using Aiglusoft.IAM.Domain.Repositories;
using Aiglusoft.IAM.Domain.Services;
using Aiglusoft.IAM.Infrastructure.Factories;
using Aiglusoft.IAM.Infrastructure.Middlewares;
using Aiglusoft.IAM.Infrastructure.Persistence.DbContexts;
using Aiglusoft.IAM.Infrastructure.Persistence.DbContexts.SeedData;
using Aiglusoft.IAM.Infrastructure.Repositories;
using Aiglusoft.IAM.Infrastructure.Services;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;

var builder = WebApplication.CreateBuilder(args);

// Configure logging with Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

var services = builder.Services;

// Add services to the container
services.AddHttpContextAccessor();
services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddValidatorsFromAssembly(typeof(ApplicationLayer).Assembly);

services.AddMediatR(typeof(ApplicationLayer).Assembly);
services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

var serviceProvider = new ServiceCollection()
       .AddEntityFrameworkNpgsql()
       .BuildServiceProvider();

services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Npgsql");
    options.UseNpgsql(connectionString);
    options.UseInternalServiceProvider(serviceProvider);
});

services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<IClientRepository, ClientRepository>();
services.AddScoped<IAuthorizationCodeRepository, AuthorizationCodeRepository>();
services.AddScoped<ITokenRepository, TokenRepository>();

services.AddSingleton<IHashPasswordService, HashPasswordService>();
services.AddScoped<IUserFactory, UserFactory>();
services.AddScoped<ITokenFactory, TokenFactory>();
services.AddScoped<IRootContext, RootContext>();
services.AddSingleton<IEmailService, SmtpEmailService>();

services.AddSingleton<ICertificateService, CertificateService>();
services.AddSingleton<IJwtTokenService, JwtTokenService>();
services.AddScoped<IDiscoveryService, DiscoveryService>();
services.AddScoped<IJwksService, JwksService>();
services.AddSingleton<ErrorMappingService>();

ConfigureAuthentication(services, builder.Configuration);

// Register DatabaseSeeder as a hosted service in development environment
if (builder.Environment.IsDevelopment())
{
    services.AddHostedService<DatabaseSeeder>();
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapFallbackToFile("/index.html");

app.Run();


static void ConfigureAuthentication(IServiceCollection services, IConfiguration configuration)
{
    services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(options =>
    {
        options.LoginPath = "/signin";
        options.AccessDeniedPath = "/accessdenied";
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

public partial class Program { }