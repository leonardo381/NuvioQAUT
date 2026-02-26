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
            Root.GetByRole(AriaRole.Button, new() { Name = "New record" });

        private ILocator EditButton =>
            Root.GetByRole(AriaRole.Button, new() { Name = "Edit" });

        private ILocator DeleteButton =>
            Root.GetByRole(AriaRole.Button, new() { Name = "Delete" });

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
    }
}