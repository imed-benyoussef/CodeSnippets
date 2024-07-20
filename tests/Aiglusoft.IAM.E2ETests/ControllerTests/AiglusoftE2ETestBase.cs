using Aiglusoft.IAM.Server;
using System.Net.Http;
using Xunit;

namespace Aiglusoft.IAM.E2ETests.ControllerTests
{
    public class AiglusoftE2ETestBase : IClassFixture<AiglusoftWebApplicationFactory>
    {
        protected readonly HttpClient Client;

        public AiglusoftE2ETestBase(AiglusoftWebApplicationFactory factory)
        {
            Client = factory.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("https://localhost:5001")
            });
        }
    }
}
