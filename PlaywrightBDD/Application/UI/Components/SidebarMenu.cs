using System.Threading.Tasks;
using Application.UI.Components.Base;
using Framework.Core;
using Microsoft.Playwright;

namespace Application.UI.Components
{
    public class SidebarMenu : UIComponent
    {
        public SidebarMenu(ILocator root, ElementExecutor executor)
            : base(root, executor)
        {
        }

        private ILocator CollectionLink(string collectionName) =>
            Root.Locator("a.sidebar-list-item")
                .Filter(new LocatorFilterOptions
                {
                    HasText = collectionName
                })
                .First;

        /// <summary>
        /// Generic open for any collection by its sidebar text.
        /// Example: "users", "posts", "logs", etc.
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