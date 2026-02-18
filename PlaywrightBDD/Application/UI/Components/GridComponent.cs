using Application.UI.Components.Base;
using Framework.Core;
using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Application.UI.Components
{
    public class GridComponent : UIComponent
    {
        private ILocator LoadingSpinner => Locator(".loading");

        public GridComponent(ILocator root, ElementExecutor executor) : base(root, executor) { }

        public async Task WaitForLoadedAsync()
        {
            // Replace ".loading" with your actual stable signal if possible
            await LoadingSpinner.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Detached
            });
        }

        public ILocator RowByText(string text)
            => Locator(".grid-row").Filter(new LocatorFilterOptions { HasTextString = text });
    }
}