using System;
using System.Threading.Tasks;
using Application.UI.Pages;
using Framework.Core;
using Framework.Engine;
using Microsoft.Playwright;

namespace Application.UI.Flows
{
    public sealed class LoginFlow
    {
        private readonly LoginPage _loginPage;
        private readonly ExecutionSettings _settings;

        public LoginFlow(IPage page, ElementExecutor executor, ExecutionSettings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _loginPage = new LoginPage(page, executor, settings);
        }

        public async Task AsAdminAsync()
        {
            if (string.IsNullOrWhiteSpace(_settings.AdminUser) ||
                string.IsNullOrWhiteSpace(_settings.AdminPassword))
            {
                throw new InvalidOperationException(
                    "Admin credentials are missing. Check ADMIN_USER and ADMIN_PASSWORD.");
            }

            await _loginPage.LoginAsync(_settings.AdminUser, _settings.AdminPassword);
        }
    }
}