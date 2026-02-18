using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Framework.Core
{
    public class ElementExecutor
    {
        private readonly Waiter _wait;
        private readonly RetryHandler _retry;

        public ElementExecutor(IPage page)
        {
            _wait = new Waiter();
            _retry = new RetryHandler();
        }

        public async Task ClickAsync(ILocator locator, int timeoutMs = 5000)
        {
            await _wait.VisibleAsync(locator, timeoutMs);

            await _retry.ExecuteAsync(async () =>
            {
                await locator.ClickAsync();
            });
        }

        public async Task FillAsync(ILocator locator, string value, int timeoutMs = 5000)
        {
            await _wait.VisibleAsync(locator, timeoutMs);

            await _retry.ExecuteAsync("Click", async () =>
            {
                await locator.ClickAsync();
            });

        }

        public async Task PressAsync(ILocator locator, string key, int timeoutMs = 5000)
        {
            await _wait.VisibleAsync(locator, timeoutMs);

            await _retry.ExecuteAsync(async () =>
            {
                await locator.PressAsync(key);
            });
        }
    }
}