using System.Threading.Tasks;
using Application.UI.Components.Base;
using Framework.Core;
using Microsoft.Playwright;

namespace Application.UI.Components
{
    public class SidebarMenu : UIComponent
    {
        public SidebarMenu(ILocator root, ElementExecutor executor) : base(root, executor) { }

        private ILocator UsersLink => Root.GetByRole(AriaRole.Link, new() { Name = "Users" });

        public async Task OpenUsersAsync()
        {
            await Exec.ClickAsync(UsersLink);
        }
    }
}