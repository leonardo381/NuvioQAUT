using System.Threading.Tasks;
using Application.UI.Pages;
using Framework.Core;
using Microsoft.Playwright;

namespace Application.UI.Flows
{
    public class LoginFlow
    {
        private readonly LoginPage _login;

        public LoginFlow(IPage page, ElementExecutor exec)
        {
            _login = new LoginPage(page, exec);
        }

        public async Task AsAdminAsync()
        {
            await _login.GotoAsync();
            await _login.LoginAsync("admin@admin.com", "admin123");
        }
    }
}