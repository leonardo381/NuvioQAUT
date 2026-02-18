using Application.UI.Components.Base;
using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Application.UI.Components
{
    public sealed class ModalComponent : UIComponent
    {
        public ModalComponent(IPage page, ILocator root) : base(page, root) { }

        private ILocator SaveButton =>
            Root.Locator("button:has-text('Save'), button[type='submit']:has-text('Save')").First;

        public Task FillFieldAsync(string name, string value)
            => Exec.FillAsync(Root.Locator($"[name='{name}']").First, value);

        public Task SaveAsync()
            => Exec.ClickAsync(SaveButton);
    }
}