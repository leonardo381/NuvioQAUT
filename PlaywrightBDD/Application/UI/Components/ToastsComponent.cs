using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Application.UI.Components
{
    public class ToastsComponent
    {
        private readonly IPage _page;

        public ToastsComponent(IPage page)
        {
            _page = page;
        }

        private ILocator ToastRoot =>
            _page.Locator(".toast, .notification, [role='alert']").First;

        public async Task<bool> IsVisibleAsync()
        {
            return await ToastRoot.IsVisibleAsync();
        }

        public async Task<string> GetTextAsync()
        {
            return (await ToastRoot.InnerTextAsync()).Trim();
        }
    }
}