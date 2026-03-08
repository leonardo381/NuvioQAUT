using System.Threading.Tasks;
using Application.UI.Components;
using Application.UI.Pages;
using Framework.Core;
using Framework.Engine;
using Microsoft.Playwright;

namespace Application.UI.Flows
{
    public class UsersFlow
    {
        private readonly UsersPage _users;

        public UsersFlow(
            IPage page,
            ElementExecutor executor,
            ExecutionSettings settings,
            AppShell shell)
        {
            _users = new UsersPage(page, executor, settings, shell);
        }

        public async Task CreateUserAsync(string email, string password)
        {
            await _users.OpenAsync();
            await _users.CreateUserAsync(email, password);
        }
    }
}