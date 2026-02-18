using Application.UI.Components;
using Framework.Core;
using Microsoft.Playwright;
using System;

namespace Application.UI.Pages
{
    /// <summary>
    /// Generic PocketBase collection page.
    /// Pure mapping + wiring of component roots.
    /// </summary>
    public sealed class CollectionPage : BasePage
    {
        // Shared layout
        public AppShell Shell { get; }

        // Page-specific components
        public GridComponent Grid { get; }
        public ModalComponent Modal { get; }

        public CollectionPage(IPage page, ElementExecutor executor) : base(page, executor)
        {
            // App layout (sidebar/toolbar/toasts)
            Shell = new AppShell(page, executor);

            // Page-specific locators
            var gridRoot = page.Locator(".table-wrapper, .pb-table, table").First;
            var modalRoot = page.Locator(".modal, [role='dialog']").First;

            if (gridRoot is null) throw new InvalidOperationException("Grid root locator resolved to null.");
            if (modalRoot is null) throw new InvalidOperationException("Modal root locator resolved to null.");

            Grid = new GridComponent(gridRoot, executor);
            Modal = new ModalComponent(modalRoot, executor);
        }
    }
}