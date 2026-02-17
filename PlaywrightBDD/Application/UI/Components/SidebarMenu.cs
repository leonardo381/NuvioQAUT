using Microsoft.Playwright;
using Framework.Core;
using System.Threading.Tasks;

namespace Application.UI.Components
{
    public class SidebarMenu
    {
        private readonly IPage _page;
        private readonly ElementExecutor _executor;

        public SidebarMenu(IPage page)
        {
            _page = page;
            _executor = new ElementExecutor(page);
        }

        private ILocator SidebarRoot => _page.Locator("aside, nav, .sidebar").First;

        // PocketBase often shows collections in sidebar. Update these selectors to your UI.
        private ILocator CollectionsLink => SidebarRoot.Locator("a:has-text('Collections'), a:has-text('Collection')").First;
        private ILocator UsersLink => SidebarRoot.Locator("a:has-text('Users')").First;

        public async Task OpenCollectionsAsync()
        {
            if (await CollectionsLink.CountAsync() > 0)
                await _executor.ClickAsync(CollectionsLink);
        }

        public async Task OpenUsersAsync()
        {
            // In some PB UIs users are directly in sidebar, in others under Collections.
            if (await UsersLink.CountAsync() > 0)
            {
                await _executor.ClickAsync(UsersLink);
                return;
            }

            await OpenCollectionsAsync();

            if (await UsersLink.CountAsync() > 0)
                await _executor.ClickAsync(UsersLink);
        }
    }
}