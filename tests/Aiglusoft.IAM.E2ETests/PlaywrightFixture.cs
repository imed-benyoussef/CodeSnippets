using Microsoft.AspNetCore.Hosting;
using Microsoft.Playwright;

namespace Aiglusoft.IAM.E2ETests
{
    public class PlaywrightFixture : IAsyncLifetime
    {
        public IPlaywright Playwright { get; private set; }
        public IBrowser Browser { get; private set; }

        public async Task InitializeAsync()
        {
            Playwright = await Microsoft.Playwright.Playwright.CreateAsync();
            Browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false // Set to true if you don't need a UI
            });
        }

        public async Task DisposeAsync()
        {
            await Browser.DisposeAsync();
            Playwright.Dispose();
        }
    }

}