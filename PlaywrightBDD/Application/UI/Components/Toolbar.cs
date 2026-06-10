using System.Threading.Tasks;
using Microsoft.Playwright;

namespace Application.UI.Components
{
    /// <summary>
    /// Represents the top toolbar in a collection page.
    /// Pure UI mapping. No business logic.
    /// </summary>
    public class Toolbar
    {
        private readonly ILocator _root;

        public Toolbar(ILocator root)
        {
            _root = root;
        }

        // ---- Buttons ----

        private ILocator CreateButton =>
            _root.Locator(".page-header").GetByRole(AriaRole.Button, new() { Name = "New record", Exact = false });

        private ILocator EditButton =>
            _root.GetByRole(AriaRole.Button, new() { Name = "Edit" });

        private ILocator DeleteButton =>
            _root.GetByRole(AriaRole.Button, new() { Name = "Delete" });

        private ILocator SearchBarRoot =>
            _root.Locator("form.searchbar");

        // In PocketBase this is a CodeMirror contenteditable
        private ILocator SearchEditor =>
            SearchBarRoot.Locator(".cm-content");

        private ILocator SearchButton =>
            SearchBarRoot.GetByRole(AriaRole.Button, new() { Name = "Search", Exact = false });

        private ILocator ClearButton =>
            SearchBarRoot.GetByRole(AriaRole.Button, new() { Name = "Clear", Exact = false });
        // ---- Actions ----

        public async Task ClickCreateAsync()
        {
            await CreateButton.ClickAsync();
        }

        public async Task ClickEditAsync()
        {
            await EditButton.ClickAsync();
        }

        public async Task ClickDeleteAsync()
        {
            await DeleteButton.ClickAsync();
        }

        /// <summary>
        /// Clears any existing search and executes a new search query using the UI search bar.
        /// </summary>
        public async Task SearchAsync(string query, int timeoutMs = 5000)
        {
            // Clear previous query if the Clear button is present
            if (await ClearButton.CountAsync() > 0)
            {
                await ClearButton.ClickAsync(new LocatorClickOptions
                {
                    Timeout = timeoutMs
                });
            }

            // Focus editor and set the query
            await SearchEditor.FillAsync(query, new LocatorFillOptions
            {
                Timeout = timeoutMs
            });

            // Click the Search button (this is how PB triggers the filter)
            await SearchButton.ClickAsync(new LocatorClickOptions
            {
                Timeout = timeoutMs
            });
        }
    }
}
