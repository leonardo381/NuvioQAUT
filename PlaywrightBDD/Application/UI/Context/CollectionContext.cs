using Application.UI.Pages;
using Microsoft.Playwright;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.UI.Contexts
{
    /// <summary>
    /// Generic actions for any PocketBase collection page.
    /// Avoids one-context-per-collection by working with generic UI structure.
    /// </summary>
    public sealed class CollectionContext
    {
        private readonly CollectionPage _page;

        public CollectionContext(IPage page)
        {
            _page = new CollectionPage(page);
        }

        public Task OpenCollectionAsync(string collectionName)
            => _page.Shell.Menu.OpenCollectionAsync(collectionName);

        public Task NewRecordAsync()
            => _page.Shell.Toolbar.ClickNewAsync();

        public async Task FillFormAsync(Dictionary<string, string> fields)
        {
            // Optionally: wait for modal visible if needed
            // await _page.Modal.WaitForVisibleAsync();

            foreach (var kv in fields)
                await _page.Modal.FillFieldAsync(kv.Key, kv.Value);
        }

        public Task SaveAsync()
            => _page.Modal.SaveAsync(); // use your ModalComponent method name

        public async Task AssertGridContainsAsync(string expectedText)
        {
            //await _page.Grid.WaitForLoadedAsync();
            var ok = await _page.Grid.ContainsTextAsync(expectedText);
            Assert.That(ok, Is.True, $"Expected grid to contain '{expectedText}'");
        }
    }
}