using Aiglusoft.IAM.Application;
using Aiglusoft.IAM.Application.Behaviors;
using Aiglusoft.IAM.Application.Mappings;
using Aiglusoft.IAM.Application.Queries;
using Aiglusoft.IAM.Application.Services;
using Aiglusoft.IAM.Domain;
using Aiglusoft.IAM.Domain.Factories;
using Aiglusoft.IAM.Domain.Model;
using Aiglusoft.IAM.Domain.Model.CodeValidators;
using Aiglusoft.IAM.Domain.Repositories;
using Aiglusoft.IAM.Domain.Services;
using Aiglusoft.IAM.Infrastructure.Behaviors;
using Aiglusoft.IAM.Infrastructure.Factories;
using Aiglusoft.IAM.Infrastructure.Middlewares;
using Aiglusoft.IAM.Infrastructure.Persistence.DbContexts;
using Aiglusoft.IAM.Infrastructure.Persistence.DbContexts.SeedData;
using Aiglusoft.IAM.Infrastructure.Repositories;
using Aiglusoft.IAM.Infrastructure.Services;
using Asp.Versioning;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Globalization;

var builder = WebApplication.CreateBuilder();

// Configure logging with Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

var services = builder.Services;
var configuration = builder.Configuration;

services.AddCors(options =>
{
  options.AddPolicy("AllowAll",
      builder =>
      {
        builder
                  .AllowAnyOrigin() // Autorise toutes les origines
                  .AllowAnyMethod() // Autorise toutes les méthodes HTTP (GET, POST, PUT, etc.)
                  .AllowAnyHeader(); // Autorise tous les en-têtes
      });
});

// Configuration des services de localisation
services.AddLocalization(options => options.ResourcesPath = "Resources/Localization");

services.Configure<RequestLocalizationOptions>(options =>
{
  var supportedCultures = new[] { "fr" };
  options.DefaultRequestCulture = new RequestCulture("fr");
  options.SupportedCultures = supportedCultures.Select(c => new CultureInfo(c)).ToList();
  options.SupportedUICultures = supportedCultures.Select(c => new CultureInfo(c)).ToList();
});

// Add services to the container
services.AddControllers();



builder.Services.AddProblemDetails();
builder.Services.AddApiVersioning(
                    options =>
                    {
                      // reporting api versions will return the headers
                      // "api-supported-versions" and "api-deprecated-versions"
                      options.ReportApiVersions = true;

                      options.Policies.Sunset(0.9)
                                      .Effective(DateTimeOffset.Now.AddDays(60))
                                      .Link("policy.html")
                                          .Title("Versioning Policy")
                                          .Type("text/html");
                    })
                .AddMvc()
                .AddApiExplorer(
                    options =>
                    {
                      // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                      // note: the specified format code will format the version as "'v'major[.minor][-status]"
                      options.GroupNameFormat = "'v'VVV";

                      // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                      // can also be used to control the format of the API version in route templates
                      options.SubstituteApiVersionInUrl = true;
                    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen(
    options =>
    {
      // add a custom operation filter which sets default values
      options.OperationFilter<SwaggerDefaultValues>();

      var fileName = typeof(Program).Assembly.GetName().Name + ".xml";
      var filePath = Path.Combine(AppContext.BaseDirectory, fileName);

      // integrate xml comments
      options.IncludeXmlComments(filePath);
    });

services.AddHttpContextAccessor();
services.AddEndpointsApiExplorer();
services.AddValidatorsFromAssembly(typeof(ApplicationModule).Assembly);


services.AddAutoMapper(opt =>
{
  opt.AddProfile<MappingProfile>();
});

services.AddMediatR(cfg =>
{
  cfg.RegisterServicesFromAssemblyContaining(typeof(ApplicationModule));

  cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
  cfg.AddOpenBehavior(typeof(ValidatorBehavior<,>));
  cfg.AddOpenBehavior(typeof(TransactionBehavior<,>));
});





var serviceProvider = new ServiceCollection()
       .AddEntityFrameworkNpgsql()
       .BuildServiceProvider();
var connectionString = builder.Configuration.GetConnectionString("Npgsql");

services.AddDbContext<AppDbContext>(options =>
{
  
  options.UseNpgsql(connectionString);
  options.UseInternalServiceProvider(serviceProvider);
});


services.AddScoped<IQueryContext, AppDbContext>();
services.AddScoped<IUnitOfWork, AppDbContext>();
services.AddScoped(typeof(IODataQueryService<>), typeof(ODataQueryService<>));

builder.Services.AddDataProtection()
    .PersistKeysToDbContext<AppDbContext>();

services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<IClientRepository, ClientRepository>();
services.AddScoped<IAuthorizationCodeRepository, AuthorizationCodeRepository>();
services.AddScoped<ITokenRepository, TokenRepository>();
services.AddScoped<ICodeValidatorRepository, CodeValidatorRepository>();


services.AddSingleton<IHashPasswordService, HashPasswordService>();
services.AddScoped<IUserFactory, UserFactory>();
services.AddScoped<ITokenFactory, TokenFactory>();
services.AddScoped<IRootContext, RootContext>();
services.AddTransient<IEmailSender, SenderService>();
services.AddTransient<ISmsSender, SenderService>();

services.AddScoped<IVerificationCodeService, VerificationCodeService>();

services.AddSingleton<ICertificateService, CertificateService>();
services.AddSingleton<IJwtTokenService, JwtTokenService>();
services.AddSingleton<IEncryptionService, EncryptionService>();

services.AddSingleton<KeyManagementService>();
services.AddSingleton<RSAKeyService>();


services.AddScoped<IDiscoveryService, DiscoveryService>();
services.AddScoped<IJwksService, JwksService>();
services.AddSingleton<ErrorMappingService>();

// prevent from mapping "sub" claim to nameidentifier.
JsonWebTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

    services.AddAuthentication(options =>
    {
      options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
      options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
      options.RequireHttpsMetadata = false;
      options.SaveToken = true;
      var serviceProvider = services.BuildServiceProvider();
      var configuration = serviceProvider.GetRequiredService<IConfiguration>();
      var certificateService = serviceProvider.GetRequiredService<ICertificateService>();

      var rsa = certificateService.GetRsaPrivateKey();
      var key = new RsaSecurityKey(rsa)
      {
        KeyId = certificateService.GetKeyId()
      };

      options.TokenValidationParameters = new TokenValidationParameters
      {
        ValidateIssuer = true,
        ValidIssuer = configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = configuration["Jwt:Audience"],
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = key
      };
    })
   .AddCookie(options =>
   {
     options.LoginPath = "/login";
     options.AccessDeniedPath = "/accessdenied";
   });


// Définir des politiques d'autorisation basées sur les schémas d'authentification
services.AddAuthorization(options =>
{
  // Politique basée sur le schéma d'authentification par cookie
  options.AddPolicy("CookiePolicy", policy =>
  {
    policy.AuthenticationSchemes.Add("Cookies");
    policy.RequireAuthenticatedUser();
  });

  // Politique basée sur le schéma d'authentification par JWT (Bearer)
  options.AddPolicy("JwtPolicy", policy =>
  {
    policy.AuthenticationSchemes.Add("Bearer");
    policy.RequireAuthenticatedUser();
  });
});

// Register DatabaseSeeder as a hosted service in development environment
if (builder.Environment.IsDevelopment())
{
  services.AddHostedService<DatabaseSeederForTests>();
}

var app = builder.Build();

// Utiliser les arguments pour contrôler les migrations
using (var scope = app.Services.CreateScope())
{
  var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

  // Si l'argument "db-migration" est fourni, appliquer toutes les migrations
  if (args.Contains("db-migration"))
  {
    Console.WriteLine("Appliquer toutes les migrations...");
    try
    {
      dbContext.Database.Migrate();
      Console.WriteLine("Migrations appliquées avec succès.");
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Erreur lors de l'application des migrations : {ex.Message}");
    }

    // Si vous voulez arrêter l'application après les migrations, ajoutez cette ligne
    return;
  }
}

app.UseDefaultFiles();
app.UseStaticFiles();


app.UseSwagger();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage();

}
app.UseSwaggerUI(
    options =>
    {
      var descriptions = app.DescribeApiVersions();

      // build a swagger endpoint for each discovered API version
      foreach (var description in descriptions)
      {
        var url = $"/swagger/{description.GroupName}/swagger.json";
        var name = description.GroupName.ToUpperInvariant();
        options.SwaggerEndpoint(url, name);
      }
    });


app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseStaticFiles();

app.UseRouting();

//app.UseCors("AllowSpecificOrigin");

app.UseAuthentication();
app.UseAuthorization();

// Configurer la localisation des requêtes
app.UseRequestLocalization(options =>
{
  var questStringCultureProvider = options.RequestCultureProviders[0];
  options.RequestCultureProviders.RemoveAt(0);
  options.RequestCultureProviders.Insert(1, questStringCultureProvider);
});

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();

public partial class Program { }
