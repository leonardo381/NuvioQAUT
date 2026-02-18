using Framework.Core;
using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Application.UI.Components.Base
{
    public abstract class UIComponent
    {
        protected ILocator Root { get; }
        protected ElementExecutor Exec { get; }

        protected UIComponent(IPage page, ILocator root)
        {
            Root = root;
            Exec = new ElementExecutor(page);
        }

        // Small helpers (optional but useful)
        public Task WaitForVisibleAsync()
            => Root.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });

        public Task WaitForHiddenAsync()
            => Root.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Hidden });

        public async Task<bool> ExistsAsync()
            => await Root.CountAsync() > 0;
    }
}