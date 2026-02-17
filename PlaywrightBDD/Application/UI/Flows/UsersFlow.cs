using Application.UI.Pages;
using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Application.UI.Flows
{
    public class UsersFlow
    {
        private readonly IPage _page;

        public UsersFlow(IPage page)
        {
            _page = page;
        }

        public async Task OpenUsersAsync()
        {
            var users = new UsersPage(_page);
            await users.Menu.OpenUsersAsync();
        }

        public async Task CreateUserAsync(string email, string password)
        {
            var users = new UsersPage(_page);
            await users.Menu.OpenUsersAsync();
            await users.CreateUserAsync(email, password);
        }
    }
}