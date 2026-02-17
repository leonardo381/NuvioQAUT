using Microsoft.Playwright;
using Framework.Core;
using System.Threading.Tasks;

namespace Application.UI.Components
{
    public class Toolbar
    {
        private readonly IPage _page;
        private readonly ElementExecutor _executor;

        public Toolbar(IPage page)
        {
            _page = page;
            _executor = new ElementExecutor(page);
        }

        // Adjust selectors as needed
        private ILocator SearchInput => _page.Locator("input[placeholder*='Search'], input[type='search']").First;

        public async Task SearchAsync(string term)
        {
            if (await SearchInput.CountAsync() == 0)
                return;

            await _executor.FillAsync(SearchInput, term);
            await SearchInput.PressAsync("Enter");
        }
    }
}