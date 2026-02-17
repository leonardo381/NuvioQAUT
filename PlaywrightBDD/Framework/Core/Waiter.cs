using Microsoft.Playwright;
using System;
using System.Threading.Tasks;

namespace Framework.Core
{
    public class Waiter
    {
        public async Task VisibleAsync(ILocator locator, int timeoutMs = 5000)
        {
            await locator.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = timeoutMs
            });
        }

        public async Task HiddenAsync(ILocator locator, int timeoutMs = 5000)
        {
            await locator.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Hidden,
                Timeout = timeoutMs
            });
        }

        public async Task UntilAsync(Func<Task<bool>> condition, int timeoutMs = 5000, int pollMs = 200)
        {
            var start = DateTime.UtcNow;

            while ((DateTime.UtcNow - start).TotalMilliseconds < timeoutMs)
            {
                if (await condition())
                    return;

                await Task.Delay(pollMs);
            }

            throw new TimeoutException("Wait condition was not met within timeout.");
        }

        public async Task NetworkIdleAsync(IPage page, int timeoutMs = 10000)
        {
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle, new PageWaitForLoadStateOptions
            {
                Timeout = timeoutMs
            });
        }
    }
}