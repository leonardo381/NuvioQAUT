using System.Threading.Tasks;
using Framework.Core;
using Microsoft.Playwright;

namespace Application.UI.Pages
{
    public class LoginPage : BasePage
    {
        private ILocator Email => Page.Locator("input[name='email']");
        private ILocator Password => Page.Locator("input[name='password']");
        private ILocator Submit => Page.Locator("button[type='submit']");

        public LoginPage(IPage page, ElementExecutor executor) : base(page, executor) { }

        public async Task GotoAsync()
        {
            await Page.GotoAsync("/_/");
        }

        public async Task LoginAsync(string email, string password)
        {
            await Exec.FillAsync(Email, email);
            await Exec.FillAsync(Password, password);
            await Exec.ClickAsync(Submit);
        }
    }
}