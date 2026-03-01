using System.Threading.Tasks;
using Application.UI.Components.Base;
using Framework.Core;
using Microsoft.Playwright;

namespace Application.UI.Components
{
    /// <summary>
    /// Represents the top toolbar in a collection page.
    /// Pure UI mapping. No business logic.
    /// </summary>
    public class Toolbar : UIComponent
    {
        public Toolbar(ILocator root, ElementExecutor executor)
            : base(root, executor)
        {
        }

        // ---- Buttons ----

        private ILocator CreateButton =>
            Root.Locator(".page-header").GetByRole(AriaRole.Button, new() { Name = "New record", Exact = false });

        private ILocator EditButton =>
            Root.GetByRole(AriaRole.Button, new() { Name = "Edit" });

        private ILocator DeleteButton =>
            Root.GetByRole(AriaRole.Button, new() { Name = "Delete" });

        private ILocator SearchBarRoot =>
            Root.Locator("form.searchbar");

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
            await Exec.ClickAsync(CreateButton);
        }

        public async Task ClickEditAsync()
        {
            await Exec.ClickAsync(EditButton);
        }

        public async Task ClickDeleteAsync()
        {
            await Exec.ClickAsync(DeleteButton);
        }

                /// <summary>
        /// Clears any existing search and executes a new search query using the UI search bar.
        /// </summary>
        public async Task SearchAsync(string query, int timeoutMs = 5000)
        {
            // Clear previous query if the Clear button is present
            if (await ClearButton.CountAsync() > 0)
            {
                await Exec.ClickAsync(ClearButton, timeoutMs);
            }

            // Focus editor and set the query
            await Exec.FillAsync(SearchEditor, query, timeoutMs);

            // Click the Search button (this is how PB triggers the filter)
            await Exec.ClickAsync(SearchButton, timeoutMs);
        }
    }
}