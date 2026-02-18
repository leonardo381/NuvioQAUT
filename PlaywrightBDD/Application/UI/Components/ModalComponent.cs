using System.Threading.Tasks;
using Application.UI.Components.Base;
using Framework.Core;
using Microsoft.Playwright;

namespace Application.UI.Components
{
    public class ModalComponent : UIComponent
    {
        public ModalComponent(ILocator root, ElementExecutor executor) : base(root, executor) { }

        public async Task FillFieldAsync(string label, string value)
        {
            var input = Root.GetByLabel(label);
            await Exec.FillAsync(input, value);
        }

        public async Task ConfirmAsync()
        {
            var btn = Root.GetByRole(AriaRole.Button, new() { Name = "Save" });
            await Exec.ClickAsync(btn);
        }
    }
}