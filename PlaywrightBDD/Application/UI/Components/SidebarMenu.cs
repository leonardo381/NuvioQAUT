using System.Threading.Tasks;
using Microsoft.Playwright;

namespace Application.UI.Components
{
    /// <summary>
    /// Represents the left sidebar menu with collection links.
    /// Pure UI mapping, no business logic.
    /// </summary>
    public class SidebarMenu
    {
        private readonly ILocator _root;

        public SidebarMenu(ILocator root)
        {
            _root = root;
        }

        /// <summary>
        /// Locates a collection link by its visible text on the sidebar.
        /// Example: "users", "posts", "logs", etc.
        /// </summary>
        private ILocator CollectionLink(string collectionName) =>
            _root.Locator("a.sidebar-list-item")
                .Filter(new LocatorFilterOptions
                {
                    // Keep it simple and consistent with other components
                    HasTextString = collectionName
                })
                .First;

        /// <summary>
        /// Generic open for any collection by its sidebar text.
        /// This is what CollectionContext will use.
        /// </summary>
        public async Task OpenCollectionAsync(string collectionName, int timeoutMs = 15000)
        {
            var link = CollectionLink(collectionName);
            await link.ClickAsync(new LocatorClickOptions
            {
                Timeout = timeoutMs
            });
        }

        /// <summary>
        /// Convenience wrapper for the "users" collection.
        /// </summary>
        public Task OpenUsersAsync(int timeoutMs = 15000)
            => OpenCollectionAsync("users", timeoutMs);
    }
}
