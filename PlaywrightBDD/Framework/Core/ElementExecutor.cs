using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Framework.Core
{
    public sealed class ElementExecutor
    {
        public async Task ClickAsync(ILocator locator, int timeoutMs = 5000)
        {
            await locator.ClickAsync(new LocatorClickOptions
            {
                Timeout = timeoutMs
            });
        }

        public async Task FillAsync(ILocator locator, string value, int timeoutMs = 5000)
        {
            await locator.FillAsync(value, new LocatorFillOptions
            {
                Timeout = timeoutMs
            });
        }
    }
}
