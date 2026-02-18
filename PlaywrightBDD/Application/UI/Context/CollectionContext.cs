using System.Threading.Tasks;
using Application.UI.Pages;
using Framework.Core;
using Microsoft.Playwright;

namespace Application.UI.Context
{
    public class CollectionContext
    {
        private readonly CollectionPage _page;

        public CollectionContext(IPage page, ElementExecutor executor)
        {
            _page = new CollectionPage(page, executor);
        }
        public async Task OpenAsync(string collectionName)
        {
            // await _page.Shell.Menu.OpenCollectionAsync(collectionName);

            // Placeholder until i implement a generic menu method:
            await Task.CompletedTask;
        }

        // Expose the page if you want to allow tests/flows to use components directly
        public CollectionPage Page => _page;
    }
}