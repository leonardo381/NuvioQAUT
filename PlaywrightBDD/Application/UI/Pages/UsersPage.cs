using System;
using System.Threading.Tasks;
using Application.UI.Components;
using Framework.Core;
using Framework.Engine;
using Microsoft.Playwright;

namespace Application.UI.Pages
{
    public sealed class UsersPage : BasePage
    {
        public AppShell Shell { get; }
        public GridComponent UsersGrid { get; }
        public ModalComponent Modal { get; }

        public UsersPage(
            IPage page,
            ElementExecutor executor,
            ExecutionSettings settings,
            AppShell? shell = null)
            : base(page, executor, settings)
        {
            Shell = shell ?? new AppShell(page, executor);

            var gridRoot = page.Locator(".table-wrapper, .pb-table, table").First;
            var modalRoot = page.Locator(".overlay-panel.record-panel").First;

            UsersGrid = new GridComponent(gridRoot, executor);
            Modal = new ModalComponent(modalRoot, executor);
        }

        public async Task OpenAsync()
        {
            await Shell.Menu.OpenUsersAsync();
            await UsersGrid.WaitForLoadedAsync();
        }

        public async Task CreateUserAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty.", nameof(email));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty.", nameof(password));

            await Shell.Toolbar.ClickCreateAsync();

            await Modal.WaitForOpenAsync(10000);

            await Modal.FillFieldAsync("email", email);
            await Modal.FillFieldAsync("Password", password);
            await Modal.FillFieldAsync("Password confirm", password);

            await Modal.ConfirmAsync();
            await UsersGrid.WaitForLoadedAsync();
        }
    }
}