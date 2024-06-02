using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Text;

namespace Aiglusoft.IAM.E2ETests.ControllerTests
{
    using Aiglusoft.IAM.Server;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using System.Net.Http;
    using System.Text;

    public abstract class BaseIntegrationTest : IClassFixture<WebApplicationFactory<Aiglusoft.IAM.Server.Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        protected BaseIntegrationTest(WebApplicationFactory<Aiglusoft.IAM.Server.Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(ConfigureTestServices);
            });

        }

        public WebApplicationFactory<Program> Factory => _factory;

        protected abstract void ConfigureTestServices(IServiceCollection services);

        protected StringContent CreateJsonContent(object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }


}