using System.Threading.Tasks;
using Application.UI.Components.Base;
using Framework.Core;
using Microsoft.Playwright;

namespace Application.UI.Components
{
    /// <summary>
    /// Represents a generic modal dialog.
    /// Pure UI mapping + very small helpers.
    /// </summary>
    public class ModalComponent : UIComponent
    {
        public ModalComponent(ILocator root, ElementExecutor executor)
            : base(root, executor)
        {
        }

        /// <summary>
        /// Waits for the modal container to be visible.
        /// Existing public API – kept for compatibility.
        /// </summary>
        public async Task WaitForOpenAsync(int timeoutMs = 5000)
        {
            await Root.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = timeoutMs
            });
        }

        /// <summary>
        /// Convenience alias for readability / consistency.
        /// </summary>
        public Task WaitOpenAsync(int timeoutMs = 5000)
            => WaitForOpenAsync(timeoutMs);

        /// <summary>
        /// Waits for the modal to be closed/removed from DOM.
        /// Useful after confirm/save.
        /// </summary>
        public async Task WaitClosedAsync(int timeoutMs = 5000)
        {
            await Root.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Detached,
                Timeout = timeoutMs
            });
        }

        /// <summary>
        /// Returns the input associated with a given label text inside the modal.
        /// Uses Exact matching to avoid collisions like "Password" vs "Password confirm".
        /// </summary>
        public ILocator GetFieldInput(string label)
        {
            return Root.GetByLabel(label, new LocatorGetByLabelOptions
            {
                Exact = true
            });
        }

        /// <summary>
        /// Fills an input by its associated label text inside the modal.
        /// Uses Exact matching to avoid strict-mode collisions
        /// like "Password" vs "Password confirm".
        /// </summary>
        public async Task FillFieldAsync(string label, string value, int timeoutMs = 5000)
        {
            var input = GetFieldInput(label);
            await Exec.FillAsync(input, value, timeoutMs);
        }

        /// <summary>
        /// Clicks the primary confirm/save button in the modal.
        /// Tries common action names: "Create", "Save", "Update".
        /// </summary>
        public async Task ConfirmAsync(int timeoutMs = 5000)
        {
            ILocator? button = null;

            var createBtn = Root.GetByRole(AriaRole.Button, new() { Name = "Create" });
            if (await createBtn.CountAsync() > 0)
            {
                button = createBtn;
            }
            else
            {
                var saveBtn = Root.GetByRole(AriaRole.Button, new() { Name = "Save" });
                if (await saveBtn.CountAsync() > 0)
                {
                    button = saveBtn;
                }
                else
                {
                    var updateBtn = Root.GetByRole(AriaRole.Button, new() { Name = "Update" });
                    if (await updateBtn.CountAsync() > 0)
                    {
                        button = updateBtn;
                    }
                }
            }

            if (button is null)
            {
                throw new PlaywrightException(
                    "Could not find primary confirm button in modal. " +
                    "Tried buttons with names: 'Create', 'Save', 'Update'.");
            }

            await Exec.ClickAsync(button, timeoutMs);
        }


        public async Task ClickCloneAsync(int timeoutMs = 5000)
        {
            var cloneBtn = Root.GetByRole(AriaRole.Button, new() { Name = "Clone", Exact = false });

            if (await cloneBtn.CountAsync() == 0)
                throw new PlaywrightException("Clone button not found in record panel.");

            await Exec.ClickAsync(cloneBtn, timeoutMs);
        }

                // --------- record options menu (… button) ---------

        private ILocator MoreOptionsButton =>
            Root.GetByRole(AriaRole.Button, new()
            {
                Name = "More record options",
                Exact = false
            });

        private ILocator DeleteMenuItem =>
            Root.GetByRole(AriaRole.Menuitem, new()
            {
                Name = "Delete",
                Exact = false
            });

        private ILocator DuplicateMenuItem =>
            Root.GetByRole(AriaRole.Menuitem, new()
            {
                Name = "Duplicate",
                Exact = false
            });

        public async Task OpenMoreOptionsAsync(int timeoutMs = 5000)
        {
            await Exec.ClickAsync(MoreOptionsButton, timeoutMs);
        }

        public async Task ClickDeleteFromMenuAsync(int timeoutMs = 5000)
        {
            await OpenMoreOptionsAsync(timeoutMs);
            await Exec.ClickAsync(DeleteMenuItem, timeoutMs);
        }

        public async Task ClickDuplicateFromMenuAsync(int timeoutMs = 5000)
        {
            await OpenMoreOptionsAsync(timeoutMs);
            await Exec.ClickAsync(DuplicateMenuItem, timeoutMs);
        }

        // ---------- global confirm popup ("Do you really want to delete…?") ----------

        // Uses the page behind the modal root to find the confirm overlay
        private ILocator ConfirmPopup =>
            Root.Page.Locator(".overlay-panel.confirm-popup");

        private ILocator ConfirmYesButton =>
            ConfirmPopup.GetByRole(AriaRole.Button, new()
            {
                Name  = "Yes",
                Exact = true
            });

        public async Task ConfirmDeleteAsync(int timeoutMs = 5000)
        {
            await ConfirmPopup.WaitForAsync(new LocatorWaitForOptions
            {
                State   = WaitForSelectorState.Visible,
                Timeout = timeoutMs
            });

            await Exec.ClickAsync(ConfirmYesButton, timeoutMs);

            await ConfirmPopup.WaitForAsync(new LocatorWaitForOptions
            {
                State   = WaitForSelectorState.Detached,
                Timeout = timeoutMs
            });
        }

        /// <summary>
        /// Full "delete record" UX:
        /// - open options menu
        /// - click Delete
        /// - confirm in popup (Yes)
        /// </summary>
        public async Task DeleteRecordAsync(int timeoutMs = 5000)
        {
            await ClickDeleteFromMenuAsync(timeoutMs);
            await ConfirmDeleteAsync(timeoutMs);
        }
    }
}