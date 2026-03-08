using Application.UI.Components;
using Framework.Core;
using Framework.Engine;
using Microsoft.Playwright;

namespace Application.UI.Pages
{
    public sealed class CollectionPage : BasePage
    {
        public AppShell AppShell { get; }

        public SidebarMenu Menu => AppShell.Menu;
        public Toolbar Toolbar => AppShell.Toolbar;

        public GridComponent Grid { get; }
        public ModalComponent Modal { get; }

        public CollectionPage(
            IPage page,
            ElementExecutor executor,
            ExecutionSettings settings,
            AppShell appShell)
            : base(page, executor, settings)
        {
            AppShell = appShell ?? throw new ArgumentNullException(nameof(appShell));

            var gridRoot = page.Locator(".table-wrapper");
            var modalRoot = page.Locator(".overlay-panel.record-panel");

            Grid = new GridComponent(gridRoot, executor);
            Modal = new ModalComponent(modalRoot, executor);
        }
    }
}