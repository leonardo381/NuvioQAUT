using System.Threading.Tasks;
using Application.UI.Components.Base;
using Framework.Core;
using Microsoft.Playwright;

namespace Application.UI.Components
{
    public class ModalComponent : UIComponent
    {
        public ModalComponent(ILocator root, ElementExecutor executor)
            : base(root, executor)
        {
        }

        /// <summary>
        /// Waits for the modal container to be visible.
        /// </summary>
        public async Task WaitForOpenAsync(int timeoutMs = 500)
        {
            await Root.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = timeoutMs
            });
        }

        /// <summary>
        /// Fills an input by its associated label text inside the modal.
        /// Uses Exact matching to avoid strict-mode collisions
        /// like "Password" vs "Password confirm".
        /// </summary>
        public async Task FillFieldAsync(string label, string value, int timeoutMs = 5000)
        {
            var input = Root.GetByLabel(label, new LocatorGetByLabelOptions
            {
                Exact = true
            });

            await Exec.FillAsync(input, value, timeoutMs);
        }

        /// <summary>
        /// Clicks the primary confirm/save button in the modal.
        /// </summary>
        public async Task ConfirmAsync(int timeoutMs = 500)
        {
            // In your HTML, the button text is "Create"
            var btn = Root.GetByRole(AriaRole.Button, new() { Name = "Create" });

            await Exec.ClickAsync(btn, timeoutMs);
        }
    }
}