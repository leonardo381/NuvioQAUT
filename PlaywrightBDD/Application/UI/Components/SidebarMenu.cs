using Application.UI.Components.Base;
using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Application.UI.Components
{
    /// <summary>
    /// Sidebar navigation. Provides generic "open by name"
    /// so you don't create one context per collection.
    /// </summary>
    public sealed class SidebarMenu : UIComponent
    {
        public SidebarMenu(IPage page, ILocator root) : base(page, root) { }

        // Optional: expand a "Collections" section if it exists
        private ILocator CollectionsSection =>
            Root.Locator("a:has-text('Collections'), button:has-text('Collections')").First;

        public async Task OpenCollectionsSectionAsync()
        {
            if (await CollectionsSection.CountAsync() > 0)
                await Exec.ClickAsync(CollectionsSection);
        }

        /// <summary>
        /// Generic: open any collection by visible name in the sidebar.
        /// </summary>
        public async Task OpenCollectionAsync(string collectionName)
        {
            // Some UIs require expanding a section first
            await OpenCollectionsSectionAsync();

            var link = Root.Locator($"a:has-text('{collectionName}')").First;

            // If not found, do nothing for now (or throw later).
            if (await link.CountAsync() == 0)
                return;

            await Exec.ClickAsync(link);
        }

        // Convenience wrappers if you still want them
        public Task OpenUsersAsync() => OpenCollectionAsync("Users");
    }
}