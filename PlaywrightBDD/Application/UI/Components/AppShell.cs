using Framework.Core;
using Microsoft.Playwright;

namespace Application.UI.Components
{
    public class AppShell
    {
        public SidebarMenu Menu { get; }
        public Toolbar Toolbar { get; }
        public ToastsComponent Toasts { get; }

        public AppShell(IPage page)
        {
            var sidebarRoot = page.Locator(".collection-sidebar");
            //var toolbarRoot = page.Locator(".page-header");
            var toolbarRoot = page.Locator(".page-content");
            var toastRoot = page.Locator(".toasts-wrapper");
            Menu = new SidebarMenu(sidebarRoot);
            Toolbar = new Toolbar(toolbarRoot);
            Toasts = new ToastsComponent(toastRoot);
        }

        public AppShell(IPage page, ElementExecutor executor)
            : this(page)
        {
        }
    }
}
