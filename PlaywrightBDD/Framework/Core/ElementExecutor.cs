using Microsoft.Playwright;
using System;
using System.Threading.Tasks;

namespace Framework.Core
{
    public sealed class ElementExecutor
    {
        private readonly Waiter _wait;
        private readonly RetryHandler _retry;

        public ElementExecutor(Waiter waiter, RetryHandler retry)
        {
            _wait = waiter ?? throw new ArgumentNullException(nameof(waiter));
            _retry = retry ?? throw new ArgumentNullException(nameof(retry));
        }

        // Convenience ctor
        public ElementExecutor() : this(new Waiter(), new RetryHandler()) { }

        public async Task ClickAsync(ILocator locator, int timeoutMs = 5000)
        {
            await _wait.VisibleAsync(locator, timeoutMs);
            await _retry.ExecuteAsync("Click", () => locator.ClickAsync());
        }

        public async Task FillAsync(ILocator locator, string value, int timeoutMs = 5000)
        {
            await _wait.VisibleAsync(locator, timeoutMs);
            await _retry.ExecuteAsync("Fill", () => locator.FillAsync(value));
        }

        public async Task PressAsync(ILocator locator, string key, int timeoutMs = 5000)
        {
            await _wait.VisibleAsync(locator, timeoutMs);
            await _retry.ExecuteAsync("Press", () => locator.PressAsync(key));
        }
    }
}