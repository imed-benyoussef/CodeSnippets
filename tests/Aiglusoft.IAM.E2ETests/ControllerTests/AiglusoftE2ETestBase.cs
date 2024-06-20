using Aiglusoft.IAM.Server;
using System.Net.Http;
using Xunit;

namespace Aiglusoft.IAM.E2ETests
{
    public class AiglusoftE2ETestBase : IClassFixture<ControllerTests.AiglusoftWebApplicationFactory>
    {
        protected readonly HttpClient Client;

        public AiglusoftE2ETestBase(ControllerTests.AiglusoftWebApplicationFactory factory)
        {
            Client = factory.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
            {
                BaseAddress = new System.Uri("https://localhost:5001")
            });
        }
    }
}
