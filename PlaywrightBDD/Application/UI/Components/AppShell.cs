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
            Menu = new SidebarMenu(page);
            Toolbar = new Toolbar(page);
            Toasts = new ToastsComponent(page);
        }
    }
}