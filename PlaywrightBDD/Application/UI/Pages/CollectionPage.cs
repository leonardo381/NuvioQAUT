using Application.UI.Components;
using Microsoft.Playwright;

namespace Application.UI.Pages
{
    public sealed class CollectionPage
    {
        public AppShell AppShell { get; }

        public SidebarMenu Menu => AppShell.Menu;
        public Toolbar Toolbar => AppShell.Toolbar;

        public GridComponent Grid { get; }
        public ModalComponent Modal { get; }

        public CollectionPage(
            IPage page,
            AppShell appShell)
        {
            AppShell = appShell ?? throw new ArgumentNullException(nameof(appShell));

            var gridRoot = page.Locator(".table-wrapper");
            var modalRoot = page.Locator(".overlay-panel.record-panel");

            Grid = new GridComponent(gridRoot);
            Modal = new ModalComponent(modalRoot);
        }
    }
}
