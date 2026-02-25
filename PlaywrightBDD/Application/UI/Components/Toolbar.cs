using System.Threading.Tasks;
using Application.UI.Components.Base;
using Framework.Core;
using Microsoft.Playwright;

namespace Application.UI.Components
{
    public class Toolbar : UIComponent
    {
        public Toolbar(ILocator root, ElementExecutor executor) : base(root, executor) { }

        private ILocator CreateButton => Root.GetByRole(AriaRole.Button, new() { Name = "New record" });

        public async Task ClickCreateAsync()
        {
            await Exec.ClickAsync(CreateButton);
        }
        
    }
}