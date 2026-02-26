using System.Threading.Tasks;
using Application.UI.Components.Base;
using Framework.Core;
using Microsoft.Playwright;

namespace Application.UI.Components
{
    /// <summary>
    /// Represents the left sidebar menu with collection links.
    /// Pure UI mapping, no business logic.
    /// </summary>
    public class SidebarMenu : UIComponent
    {
        public SidebarMenu(ILocator root, ElementExecutor executor)
            : base(root, executor)
        {
        }

        /// <summary>
        /// Locates a collection link by its visible text on the sidebar.
        /// Example: "users", "posts", "logs", etc.
        /// </summary>
        private ILocator CollectionLink(string collectionName) =>
            Root.Locator("a.sidebar-list-item")
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
            await Exec.ClickAsync(link, timeoutMs);
        }

        /// <summary>
        /// Convenience wrapper for the "users" collection.
        /// </summary>
        public Task OpenUsersAsync(int timeoutMs = 15000)
            => OpenCollectionAsync("users", timeoutMs);
    }
}