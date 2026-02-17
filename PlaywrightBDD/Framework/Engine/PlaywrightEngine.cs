using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Framework.Engine
{
    public class PlaywrightEngine
    {
        public IPlaywright Playwright { get; private set; } = default!;

        public async Task InitializeAsync()
        {
            Playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        }

        public async Task DisposeAsync()
        {
            Playwright?.Dispose();
        }
    }
}