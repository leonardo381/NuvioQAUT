using Application.UI.Components.Base;
using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Application.UI.Components
{
    /// <summary>
    /// Top toolbar actions (search, new/save buttons, etc.)
    /// Root-injected to avoid collisions if there are multiple toolbars.
    /// </summary>
    public sealed class Toolbar : UIComponent
    {
        public Toolbar(IPage page, ILocator root) : base(page, root) { }

        private ILocator SearchInput =>
            Root.Locator("input[placeholder*='Search'], input[type='search']").First;

        private ILocator NewButton =>
            Root.Locator("button:has-text('New'), button:has-text('New record'), a:has-text('New')").First;

        private ILocator SaveButton =>
            Root.Locator("button:has-text('Save'), button[type='submit']:has-text('Save')").First;

        public async Task SearchAsync(string term)
        {
            if (await SearchInput.CountAsync() == 0)
                return;

            await Exec.FillAsync(SearchInput, term);
            await SearchInput.PressAsync("Enter");
        }

        public async Task ClickNewAsync()
        {
            if (await NewButton.CountAsync() == 0)
                return;

            await Exec.ClickAsync(NewButton);
        }

        public async Task ClickSaveAsync()
        {
            if (await SaveButton.CountAsync() == 0)
                return;

            await Exec.ClickAsync(SaveButton);
        }
    }
}