using Application.UI.Components;
using Microsoft.Playwright;

namespace Application.UI.Pages
{
    /// <summary>
    /// Generic PocketBase collection page.
    /// Pure mapping + wiring of component roots.
    /// </summary>
    public sealed class CollectionPage
    {
        public IPage Page { get; }

        // Shared layout
        public AppShell Shell { get; }

        // Page-specific components
        public GridComponent Grid { get; }
        public ModalComponent Modal { get; }

        public CollectionPage(IPage page)
        {
            Page = page;

            // App layout (sidebar/toolbar/toasts)
            Shell = new AppShell(page);

            // Page-specific roots (adjust selectors as needed for PocketBase)
            var gridRoot  = page.Locator(".table-wrapper, .pb-table, table").First;
            var modalRoot = page.Locator(".modal, [role='dialog']").First;

            Grid = new GridComponent(page, gridRoot);
            Modal = new ModalComponent(page, modalRoot);
        }
    }
}