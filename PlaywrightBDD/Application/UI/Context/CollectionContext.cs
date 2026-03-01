using System;
using System.Threading.Tasks;
using Application.UI.Components;
using Application.UI.Models;   // IRecordData
using Application.UI.Pages;

namespace Application.UI.Context
{
    /// <summary>
    /// Generic context to perform CRUD operations on any PocketBase collection.
    /// - Uses AppShell for navigation + toolbar.
    /// - Uses CollectionPage for grid + modal.
    /// - Driven by IRecordData models.
    /// - All CRUD logic centralized here.
    /// </summary>
    public sealed class CollectionContext
    {
        private readonly AppShell _shell;
        private readonly CollectionPage _page;

        // Convenience accessors
        private SidebarMenu Menu   => _shell.Menu;
        private Toolbar Toolbar    => _shell.Toolbar;
        private GridComponent Grid => _page.Grid;
        private ModalComponent Modal => _page.Modal;

        public CollectionContext(AppShell shell, CollectionPage page)
        {
            _shell = shell;
            _page = page;
        }

        // --------------------
        // Navigation
        // --------------------
        public async Task OpenAsync(string collectionName)
        {
            await Menu.OpenCollectionAsync(collectionName);
            await Grid.WaitLoadedAsync();
        }

        /// <summary>
        /// Opens the collection and filters the grid using the toolbar search bar.
        /// </summary>
        public async Task SearchAsync(string collectionName, string query)
        {
            await OpenAsync(collectionName);

            await Toolbar.SearchAsync(query);
            await Grid.WaitLoadedAsync();
        }
        // --------------------
        // CREATE
        // --------------------
        public async Task CreateAsync(
            string collectionName,
            IRecordData data)
        {
            await OpenAsync(collectionName);

            await Toolbar.ClickCreateAsync();
            await Modal.WaitOpenAsync();

            await FillModalFromRecordAsync(data);

            await Modal.ConfirmAsync();
            await Modal.WaitClosedAsync();
            await Grid.WaitLoadedAsync();
        }

        // --------------------
        // UPDATE
        // --------------------
        public async Task UpdateAsync(
            string collectionName,
            string keyColumn,
            string keyValue,
            IRecordData data)
        {
            await OpenAsync(collectionName);
            var rowIndex = await FindRowOrThrowAsync(keyColumn, keyValue);
            await Grid.ClickRowAsync(rowIndex);
            await Modal.WaitOpenAsync();
            await FillModalFromRecordAsync(data);
            await Modal.ConfirmAsync();
            await Modal.WaitClosedAsync();
            await Grid.WaitLoadedAsync();
        }

        // --------------------
        // DELETE
        // --------------------
        public async Task DeleteAsync(
            string collectionName,
            string keyColumn,
            string keyValue)
        {
            await OpenAsync(collectionName);

            var rowIndex = await FindRowOrThrowAsync(keyColumn, keyValue);

            // 1) Open record panel for the row
            await Grid.ClickRowAsync(rowIndex);
            await Modal.WaitOpenAsync();

            // 2) Let the modal handle Delete + confirm popup
            await Modal.DeleteRecordAsync();

            // 3) Wait for the record panel to close and grid to refresh
            await Modal.WaitClosedAsync();
            await Grid.WaitLoadedAsync();
        }

        // --------------------
        // CLONE
        // --------------------
        /// <summary>
        /// Clones an existing record (found by keyColumn/keyValue) and saves a new one
        /// using the provided cloneData (typically overriding the key like email).
        /// </summary>
        public async Task CloneAsync(
            string collectionName,
            string keyColumn,
            string keyValue,
            IRecordData cloneData)
        {
            await OpenAsync(collectionName);

            var rowIndex = await FindRowOrThrowAsync(keyColumn, keyValue);

            // 1) Open original record
            await Grid.ClickRowAsync(rowIndex);
            await Modal.WaitOpenAsync();

            // 2) Click Clone in the record panel
            await Modal.ClickCloneAsync();
            await Modal.WaitOpenAsync();

            // 3) Apply overrides (e.g. new email)
            await FillModalFromRecordAsync(cloneData);

            // 4) Save cloned record
            await Modal.ConfirmAsync();
            await Modal.WaitClosedAsync();
            await Grid.WaitLoadedAsync();
        }

        // --------------------
        // ASSERT
        // --------------------
        public async Task AssertRowMatchesAsync(
            string collectionName,
            string keyColumn,
            string keyValue,
            IRecordData expected)
        {
            await OpenAsync(collectionName);

            var rowIndex = await FindRowOrThrowAsync(keyColumn, keyValue);

            var expectedFields = expected.ToFields();

            // Get grid headers so we only assert columns that actually exist
            var headerMap = await Grid.GetColumnIndexMapAsync(); // header text -> index

            foreach (var kvp in expectedFields)
            {
                var columnName   = kvp.Key;
                var expectedValue = kvp.Value ?? string.Empty;

                // Skip fields that don't have a visible column in the grid
                if (!headerMap.ContainsKey(columnName))
                    continue;

                var actual = await Grid.GetCellTextAsync(rowIndex, columnName);
                var actualNormalized   = (actual ?? string.Empty).Trim();
                var expectedNormalized = expectedValue.Trim();

                if (!string.Equals(actualNormalized, expectedNormalized, StringComparison.Ordinal))
                {
                    throw new InvalidOperationException(
                        $"Grid mismatch in column '{columnName}': expected '{expectedNormalized}', got '{actualNormalized}'.");
                }
            }
        }

        public async Task AssertRowNotExistsAsync(
            string collectionName,
            string keyColumn,
            string keyValue)
        {
            await OpenAsync(collectionName);

            var rowIndex = await Grid.FindRowIndexByColumnAsync(keyColumn, keyValue);
            if (rowIndex != null)
            {
                throw new InvalidOperationException(
                    $"Row with {keyColumn}='{keyValue}' still present but expected to be missing.");
            }
        }

        // --------------------
        // Helpers
        // --------------------

        private async Task<int> FindRowOrThrowAsync(
            string keyColumn,
            string keyValue)
        {
            var rowIndex = await Grid.FindRowIndexByColumnAsync(keyColumn, keyValue);

            if (rowIndex is null)
            {
                throw new InvalidOperationException(
                    $"Row not found in collection grid where '{keyColumn}' = '{keyValue}'.");
            }

            return rowIndex.Value;
        }

        private async Task FillModalFromRecordAsync(IRecordData data)
        {
            var fields = data.ToFields();

            foreach (var kvp in fields)
            {
                var label = kvp.Key;
                var value = kvp.Value;

                if (value is null)
                    continue; // skip nulls for partial updates

                await Modal.FillFieldAsync(label, value);
            }
        }

        private async Task HandleDeleteConfirmationAsync()
        {
            await Task.CompletedTask;
        }
    }
}