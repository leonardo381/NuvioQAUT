using Application.UI.Components.Base;
using Framework.Core;
using Microsoft.Playwright;

namespace Application.UI.Components
{
    public class AppShell
    {
        public SidebarMenu Menu { get; }
        public Toolbar Toolbar { get; }
        public ToastsComponent Toasts { get; }

        public AppShell(IPage page, ElementExecutor executor)
        {
            var sidebarRoot = page.Locator(".collection-sidebar");
            //var toolbarRoot = page.Locator(".page-header");
            var toolbarRoot = page.Locator(".page-content");
            var toastRoot = page.Locator(".toasts-wrapper");
            Menu = new SidebarMenu(sidebarRoot, executor);
            Toolbar = new Toolbar(toolbarRoot, executor);
            Toasts = new ToastsComponent(toastRoot, executor);
        }
    }
}