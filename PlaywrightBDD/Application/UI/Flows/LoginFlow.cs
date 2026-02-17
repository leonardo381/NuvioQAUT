using Microsoft.Playwright;
using System.Threading.Tasks;
using Application.UI.Pages;

namespace Application.UI.Flows
{
    public class LoginFlow
    {
        private readonly IPage _page;

        public LoginFlow(IPage page)
        {
            _page = page;
        }

        public async Task LoginToAdminAsync(string identityOrEmail, string password)
        {
            var login = new LoginPage(_page);
            await login.OpenAsync();
            await login.LoginAsync(identityOrEmail, password);

            // Optionally wait for dashboard to appear (keep asserts out of Application)
            var dashboard = new DashboardPage(_page);
            await dashboard.IsLoadedAsync();
        }
    }
}