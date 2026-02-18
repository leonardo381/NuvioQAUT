using Framework.Core;
using Microsoft.Playwright;
using System;
using System.Threading.Tasks;

namespace Application.UI.Components.Base
{
    public abstract class UIComponent
    {
        protected ILocator Root { get; }
        protected ElementExecutor Exec { get; }

        protected UIComponent(ILocator root, ElementExecutor executor)
        {
            Root = root ?? throw new ArgumentNullException(nameof(root));
            Exec = executor ?? throw new ArgumentNullException(nameof(executor));
        }

        protected ILocator Locator(string selector) => Root.Locator(selector);

        // ✅ Correct option types for locator-rooted queries
        protected ILocator GetByRole(AriaRole role, LocatorGetByRoleOptions? options = null)
            => Root.GetByRole(role, options);

        protected ILocator GetByText(string text, LocatorGetByTextOptions? options = null)
            => Root.GetByText(text, options);

        public Task WaitForVisibleAsync()
            => Root.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });

        public Task WaitForHiddenAsync()
            => Root.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Hidden });

        public async Task<bool> ExistsAsync()
            => await Root.CountAsync() > 0;
    }
}