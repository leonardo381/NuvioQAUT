using Framework.Core;
using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Application.UI.Components
{
    public class ModalComponent
    {
        private readonly IPage _page;
        private readonly ElementExecutor _executor;

        public ModalComponent(IPage page)
        {
            _page = page;
            _executor = new ElementExecutor(page);
        }

        private ILocator ModalRoot => _page.Locator(".modal, [role='dialog']").First;

        private ILocator SaveButton =>
            ModalRoot.Locator("button:has-text('Save'), button[type='submit']:has-text('Save')").First;

        private ILocator CancelButton =>
            ModalRoot.Locator("button:has-text('Cancel')").First;

        private ILocator ConfirmButton =>
            ModalRoot.Locator("button:has-text('Confirm'), button:has-text('Yes')").First;

        public async Task SaveAsync()
        {
            await _executor.ClickAsync(SaveButton);
        }

        public async Task CancelAsync()
        {
            if (await CancelButton.CountAsync() > 0)
                await _executor.ClickAsync(CancelButton);
        }

        public async Task ConfirmAsync()
        {
            if (await ConfirmButton.CountAsync() > 0)
                await _executor.ClickAsync(ConfirmButton);
        }

        public async Task<bool> IsOpenAsync()
        {
            return await ModalRoot.IsVisibleAsync();
        }
    }
}