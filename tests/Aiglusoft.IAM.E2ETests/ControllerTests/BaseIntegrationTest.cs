using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Text;

namespace Aiglusoft.IAM.E2ETests.ControllerTests
{
    public abstract class BaseIntegrationTest : IClassFixture<WebApplicationFactory<Aiglusoft.IAM.Server.Program>>
    {
        protected readonly HttpClient Client;

        public BaseIntegrationTest(WebApplicationFactory<Aiglusoft.IAM.Server.Program> factory)
        {
            Client = factory.CreateClient();
        }

        protected StringContent CreateJsonContent(object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }

}