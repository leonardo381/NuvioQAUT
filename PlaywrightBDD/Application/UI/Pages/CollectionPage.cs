using Application.UI.Components;
using Framework.Core;
using Microsoft.Playwright;

namespace Application.UI.Pages
{
    /// <summary>
    /// Generic PocketBase collection page.
    /// Pure mapping + wiring of shared app shell, grid and modal.
    /// </summary>
    public sealed class CollectionPage : BasePage
    {
        // Shared layout (sidebar, toolbar, toasts)
        public AppShell Shell { get; }

        // Generic collection grid
        public GridComponent Grid { get; }

        // Generic record modal (create/update)
        public ModalComponent Modal { get; }

        public CollectionPage(IPage page, ElementExecutor executor)
            : base(page, executor)
        {
            Shell = new AppShell(page, executor);

            // Grid root: tolerant selectors for PocketBase list/grid
            var gridRoot = page.Locator(".table-wrapper, .pb-table, table").First;

            // Modal root: your record panel overlay
            var modalRoot = page.Locator(".overlay-panel.record-panel").First;

            Grid  = new GridComponent(gridRoot, executor);
            Modal = new ModalComponent(modalRoot, executor);
        }
    }
}