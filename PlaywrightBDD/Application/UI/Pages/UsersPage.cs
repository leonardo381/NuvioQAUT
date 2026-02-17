using Framework.Core;
using Microsoft.Playwright;
using System.Threading.Tasks;
using Application.UI.Components;

namespace Application.UI.Pages
{
    public class UsersPage : BasePage
    {
        public SidebarMenu Menu { get; }
        public Toolbar Toolbar { get; }
        public GridComponent Grid { get; }

        // These selectors may differ depending on PocketBase version/theme.
        private ILocator GridRoot => Page.Locator("table, .data-table, .list-table").First;

        private ILocator CreateButton => Page.Locator("button:has-text('New'), button:has-text('Create'), a:has-text('New')");
        private ILocator EmailInput => Page.Locator("input[name='email'], input[type='email']");
        private ILocator PasswordInput => Page.Locator("input[name='password']").First;
        private ILocator PasswordConfirmInput => Page.Locator("input[name='passwordConfirm'], input[name='password_confirmation']").First;
        private ILocator SaveButton => Page.Locator("button:has-text('Save'), button[type='submit']:has-text('Save')");

        public UsersPage(IPage page) : base(page)
        {
            Menu = new SidebarMenu(page);
            Toolbar = new Toolbar(page);
            Grid = new GridComponent(page, GridRoot);
        }

        public async Task OpenAsync()
        {
            // PocketBase admin “collections” routing varies; you can open Users via menu instead.
            await Page.GotoAsync("/_/");
            // Prefer: await Menu.OpenUsersAsync(); (implemented in SidebarMenu)
        }

        public async Task ClickCreateAsync()
        {
            await Executor.ClickAsync(CreateButton);
        }

        public async Task CreateUserAsync(string email, string password)
        {
            await ClickCreateAsync();

            await Executor.FillAsync(EmailInput, email);
            await Executor.FillAsync(PasswordInput, password);

            // If confirm field exists, fill it
            if (await PasswordConfirmInput.CountAsync() > 0)
                await Executor.FillAsync(PasswordConfirmInput, password);

            await Executor.ClickAsync(SaveButton);
        }
    }
}