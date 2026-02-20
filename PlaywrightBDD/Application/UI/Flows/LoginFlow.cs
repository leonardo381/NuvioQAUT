using System;
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

        private static (string User, string Password) ResolveAdminCredentials()
        {
            var user = Environment.GetEnvironmentVariable("ADMIN_USER");
            var pass = Environment.GetEnvironmentVariable("ADMIN_PASSWORD");

            if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(pass))
            {
                // Fallback to PocketBase demo credentials
                user = "test@example.com";
                pass = "123456";
            }

            return (user, pass);
        }

        public async Task AsAdminAsync()
        {
            var (user, pass) = ResolveAdminCredentials();

            await _login.GotoAsync();
            await _login.LoginAsync(user, pass);
        }
    }
}
