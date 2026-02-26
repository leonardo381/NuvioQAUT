using Application.UI.Components;
using Framework.Core;
using Microsoft.Playwright;

namespace Application.UI.Pages
{
    public sealed class CollectionPage
    {
        public IPage Page { get; }

        public AppShell AppShell { get; }

        // Optional convenience forwarding
        public SidebarMenu Menu => AppShell.Menu;
        public Toolbar Toolbar => AppShell.Toolbar;

        public GridComponent Grid { get; }
        public ModalComponent Modal { get; }

        public CollectionPage(IPage page, ElementExecutor executor)
        {
            Page = page;
            AppShell = new AppShell(page, executor);       
             var gridRoot = page.Locator(".table-wrapper");
            var modalRoot = page.Locator(".overlay-panel.record-panel");

            Grid  = new GridComponent(gridRoot, executor);
            Modal = new ModalComponent(modalRoot, executor);
        }
    }
}