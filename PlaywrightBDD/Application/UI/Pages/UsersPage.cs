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

        //optional shell injection
        public UsersPage(IPage page, ElementExecutor executor, AppShell? shell = null) : base(page, executor)
        {
            Shell = shell ?? new AppShell(page, executor);

            //selectors
            UsersGrid = new GridComponent(page.Locator(".collection-view"), executor);
            Modal = new ModalComponent(page.Locator(".modal"), executor);
        }

        public async Task OpenAsync()
        {
            await Shell.Menu.OpenUsersAsync();
            await UsersGrid.WaitForLoadedAsync();
        }

        public async Task CreateUserAsync(string email, string password)
        {
            await Shell.Toolbar.ClickCreateAsync();
            await Modal.FillFieldAsync("Email", email);
            await Modal.FillFieldAsync("Password", password);
            await Modal.ConfirmAsync();
            await UsersGrid.WaitForLoadedAsync();
        }
    }
}
