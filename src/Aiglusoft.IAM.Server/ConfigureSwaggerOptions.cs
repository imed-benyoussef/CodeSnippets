using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;
using System;
using System.Reflection;

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
  private readonly IApiVersionDescriptionProvider _provider;

  public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => _provider = provider;

  public void Configure(SwaggerGenOptions options)
  {
    // Get assembly metadata
    var assembly = Assembly.GetExecutingAssembly();
    var title = assembly.GetCustomAttribute<AssemblyProductAttribute>()?.Product ;
    var description = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description ;
    var version = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;  // GitVersion will populate this

    foreach (var apiVersionDescription in _provider.ApiVersionDescriptions)
    {
      var apiInfo = new OpenApiInfo
      {
        Title = $"{title} v{apiVersionDescription.ApiVersion}",
        Version = apiVersionDescription.ApiVersion.ToString(),
        Description = $"{description} <br> GitVersion: {version} <br> API Version: {apiVersionDescription.ApiVersion}" +
                        (apiVersionDescription.IsDeprecated ? " (Deprecated)" : string.Empty),
        Contact = new OpenApiContact
        {
          Name = "API Support",
          Email = "support@aiglusoft.com",
          Url = new Uri("https://www.aiglusoft.com/support"),
        },
        License = new OpenApiLicense
        {
          Name = "MIT License",
          Url = new Uri("https://opensource.org/licenses/MIT"),
        }
      };

      options.SwaggerDoc(apiVersionDescription.GroupName, apiInfo);
    }
  }
}
