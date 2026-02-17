using Framework.Core;
using Microsoft.Playwright;
using System.Threading.Tasks;
using Application.UI.Components;

namespace Application.UI.Pages
{
    public class DashboardPage : BasePage
    {
        public SidebarMenu Menu { get; }
        public Toolbar Toolbar { get; }

        // “Loaded” indicator – adapt selector to your PB UI (header/logo/etc.)
        private ILocator ShellHeader => Page.Locator("header, .app-header, .navbar");

        public DashboardPage(IPage page) : base(page)
        {
            Menu = new SidebarMenu(page);
            Toolbar = new Toolbar(page);
        }

        public async Task OpenAsync()
        {
            await Page.GotoAsync("/_/");
        }

        public async Task<bool> IsLoadedAsync()
        {
            return await ShellHeader.IsVisibleAsync();
        }
    }
}