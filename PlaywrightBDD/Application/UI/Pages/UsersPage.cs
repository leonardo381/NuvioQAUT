using System.Threading.Tasks;
using Application.UI.Components;
using Framework.Core;
using Microsoft.Playwright;

namespace Application.UI.Pages
{
    public class UsersPage : BasePage
    {
        public AppShell Shell { get; }
        public GridComponent UsersGrid { get; }
        public ModalComponent Modal { get; }

        // optional shell injection
        public UsersPage(IPage page, ElementExecutor executor, AppShell? shell = null)
            : base(page, executor)
        {
            Shell = shell ?? new AppShell(page, executor);

            // Grid root: tolerant selectors for PocketBase list view
            var gridRoot = page.Locator(".table-wrapper, .pb-table, table").First;

            // ✅ Modal root: your overlay record panel
            var modalRoot = page.Locator(".overlay-panel.record-panel").First;

            UsersGrid = new GridComponent(gridRoot, executor);
            Modal     = new ModalComponent(modalRoot, executor);
        }

        public async Task OpenAsync()
        {
            await Shell.Menu.OpenUsersAsync();
            await UsersGrid.WaitForLoadedAsync();
        }

        public async Task CreateUserAsync(string email, string password)
        {
            await Shell.Toolbar.ClickCreateAsync();

            await Modal.WaitForOpenAsync(10000);

            // label text is literally "email"
            await Modal.FillFieldAsync("email", email);

            // fill both password fields
            await Modal.FillFieldAsync("Password", password);
            await Modal.FillFieldAsync("Password confirm", password);

            await Modal.ConfirmAsync();
            await UsersGrid.WaitForLoadedAsync();
        }
    }
}