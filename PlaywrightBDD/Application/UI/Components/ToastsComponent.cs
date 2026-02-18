using Application.UI.Components.Base;
using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Application.UI.Components
{
    public sealed class ToastsComponent : UIComponent
    {
        public ToastsComponent(IPage page, ILocator root) : base(page, root) { }

        public async Task<string> GetTextAsync()
            => (await Root.InnerTextAsync()).Trim();
    }
}