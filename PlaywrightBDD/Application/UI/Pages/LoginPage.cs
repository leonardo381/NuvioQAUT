using Framework.Core;
using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Application.UI.Pages
{
    public class LoginPage : BasePage
    {
        // PocketBase admin UI typically uses these inputs (may vary by version/theme)
        private ILocator IdentityInput => Page.Locator("input[name='identity'], input[type='email']");
        private ILocator PasswordInput => Page.Locator("input[name='password'], input[type='password']");
        private ILocator SubmitButton => Page.Locator("button[type='submit']");

        public LoginPage(IPage page) : base(page) { }

        public async Task OpenAsync()
        {
            // BaseURL is set in Engine ContextManager. This becomes: {BaseURL}/_/
            await Page.GotoAsync("/_/");
        }

        public async Task LoginAsync(string identityOrEmail, string password)
        {
            await Executor.FillAsync(IdentityInput, identityOrEmail);
            await Executor.FillAsync(PasswordInput, password);
            await Executor.ClickAsync(SubmitButton);
        }
    }
}