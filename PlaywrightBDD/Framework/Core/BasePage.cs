using Microsoft.Playwright;

namespace Framework.Core
{
    public abstract class BasePage
    {
        protected readonly IPage Page;
        protected readonly ElementExecutor Executor;

        protected BasePage(IPage page)
        {
            Page = page;
            Executor = new ElementExecutor(page);
        }
    }
}