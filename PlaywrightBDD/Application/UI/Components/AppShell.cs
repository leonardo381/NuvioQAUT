using Microsoft.Playwright;

namespace Application.UI.Components
{
    /// <summary>
    /// Common app layout. Defines and wires the shared components.
    /// Keeps selector ownership in ONE place.
    /// </summary>
    public sealed class AppShell
    {
        public SidebarMenu Menu { get; }
        public Toolbar Toolbar { get; }
        public ToastsComponent Toasts { get; }

        public AppShell(IPage page)
        {
            // Roots (adjust these selectors to PocketBase UI as needed)
            var sidebarRoot = page.Locator("aside, nav, .sidebar").First;
            var toolbarRoot = page.Locator("header, .toolbar, [role='toolbar']").First;
            var toastRoot = page.Locator(".toast, .notification, [role='alert']").First;

            Menu = new SidebarMenu(page, sidebarRoot);
            Toolbar = new Toolbar(page, toolbarRoot);
            Toasts = new ToastsComponent(page, toastRoot);
        }
    }
}