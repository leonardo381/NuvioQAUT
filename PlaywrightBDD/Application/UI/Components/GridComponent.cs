using Application.UI.Components.Base;
using Framework.Core;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.UI.Components
{
    /// <summary>
    /// Represents a generic table/grid component.
    /// Pure UI mapping + small helpers to support generic CRUD flows.
    /// </summary>
    public class GridComponent : UIComponent
    {
        private ILocator HeaderCells => Root.Locator("thead tr th");
        private ILocator BodyRows => Root.Locator("tbody tr");
        private ILocator LoadingSpinner => Root.Locator(".loading, .spinner, [data-loading='true']");

        public GridComponent(ILocator root, ElementExecutor executor) : base(root, executor) { }

        /// <summary>
        /// Waits for the grid to be considered loaded:
        /// - optional loading spinner disappears
        /// - grid root becomes visible.
        /// </summary>
        public async Task WaitForLoadedAsync()
        {
            // Tolerant wait: spinner might not exist
            try
            {
                await LoadingSpinner.WaitForAsync(new LocatorWaitForOptions
                {
                    State = WaitForSelectorState.Detached,
                    Timeout = 5000
                });
            }
            catch
            {
                // ignore if spinner isn't part of the DOM
            }

            await Root.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible
            });
        }

        /// <summary>
        /// Convenience alias to keep naming consistent with other components.
        /// </summary>
        public Task WaitLoadedAsync() => WaitForLoadedAsync();

        /// <summary>
        /// Returns all rows that contain the given text anywhere.
        /// </summary>
        public ILocator RowByText(string text)
            => BodyRows.Filter(new LocatorFilterOptions { HasTextString = text });

        /// <summary>
        /// Builds a map of normalized header text → column index.
        /// </summary>
        public async Task<Dictionary<string, int>> GetColumnIndexMapAsync()
        {
            var headers = await HeaderCells.AllTextContentsAsync();
            if (headers is null || headers.Count == 0)
                throw new InvalidOperationException("Could not read grid headers. Check HeaderCells selector.");

            var map = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < headers.Count; i++)
            {
                var name = Normalize(headers[i]);
                if (string.IsNullOrWhiteSpace(name))
                    continue;

                if (!map.ContainsKey(name))
                    map[name] = i;
            }

            return map;
        }

        /// <summary>
        /// Finds the first row where the given column's cell equals the expected value.
        /// Returns the row locator.
        /// </summary>
        public async Task<ILocator> FindRowByCellEqualsAsync(string columnName, string expectedValue)
        {
            var map = await GetColumnIndexMapAsync();

            if (!map.TryGetValue(Normalize(columnName), out var colIndex))
                throw new InvalidOperationException($"Column '{columnName}' not found in grid headers.");

            var rowCount = await BodyRows.CountAsync();
            if (rowCount == 0)
                throw new InvalidOperationException("Grid has no rows.");

            for (int r = 0; r < rowCount; r++)
            {
                var row = BodyRows.Nth(r);
                var actual = await GetCellTextAsync(row, colIndex);

                if (string.Equals(Normalize(actual), Normalize(expectedValue), StringComparison.OrdinalIgnoreCase))
                    return row;
            }

            throw new InvalidOperationException(
                $"Row not found where column '{columnName}' equals '{expectedValue}'.");
        }

        /// <summary>
        /// Generic helper used internally and by contexts: get text of a cell in a row by column index.
        /// </summary>
        public async Task<string> GetCellTextAsync(ILocator row, int columnIndex)
        {
            var cell = row.Locator($"td:nth-child({columnIndex + 1})");
            return Normalize(await cell.First.InnerTextAsync());
        }

        /// <summary>
        /// Reads a row into a dictionary: header -> cell text.
        /// </summary>
        public async Task<Dictionary<string, string>> ReadRowAsDictionaryAsync(ILocator row)
        {
            var colMap = await GetColumnIndexMapAsync();
            var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (var kv in colMap.OrderBy(k => k.Value))
            {
                var header = kv.Key;
                var idx = kv.Value;

                var value = await GetCellTextAsync(row, idx);
                result[header] = value;
            }

            return result;
        }


        private async Task<bool> IsEmptyStateRowAsync(ILocator row)
        {
            var marker = row.Locator("h6:has-text(\"No records found\")");
            return await marker.CountAsync() > 0;
        }
        /// <summary>
        /// Finds the index (0-based) of the first row where a specific column equals the expected value.
        /// Returns null if no row matches.
        /// </summary>
        public async Task<int?> FindRowIndexByColumnAsync(string columnName, string expectedValue)
        {
            var map = await GetColumnIndexMapAsync();

            if (!map.TryGetValue(Normalize(columnName), out var colIndex))
                throw new InvalidOperationException($"Column '{columnName}' not found in grid headers.");

            var rowCount = await BodyRows.CountAsync();
            if (rowCount == 0)
                return null;

            var expectedNorm = Normalize(expectedValue);

            for (int r = 0; r < rowCount; r++)
            {
                var row = BodyRows.Nth(r);

                var emptyStateMarker = row.Locator("h6:has-text(\"No records found\")");
                if (await emptyStateMarker.CountAsync() > 0)
                    continue;

                var actual = await GetCellTextAsync(row, colIndex);
                var actualNorm = Normalize(actual);

                // 1) Exact match ignoring case
                if (string.Equals(actualNorm, expectedNorm, StringComparison.OrdinalIgnoreCase))
                    return r;

                // 2) Fallback: cell contains the expected value (ignoring case)
                if (actualNorm.Contains(expectedNorm, StringComparison.OrdinalIgnoreCase))
                    return r;
            }

            return null;
        }

        /// <summary>
        /// Gets the text of a cell by row index and column name.
        /// </summary>
        public async Task<string?> GetCellTextAsync(int rowIndex, string columnName)
        {
            var map = await GetColumnIndexMapAsync();

            if (!map.TryGetValue(Normalize(columnName), out var colIndex))
                throw new InvalidOperationException($"Column '{columnName}' not found in grid headers.");

            var rowCount = await BodyRows.CountAsync();
            if (rowIndex < 0 || rowIndex >= rowCount)
                throw new ArgumentOutOfRangeException(nameof(rowIndex), $"Row index {rowIndex} is out of range. Row count: {rowCount}.");

            var row = BodyRows.Nth(rowIndex);
            return await GetCellTextAsync(row, colIndex);
        }

        /// <summary>
        /// Clicks a row by index. Contexts can use this before Edit/Delete.
        /// </summary>
        public async Task ClickRowAsync(int rowIndex)
        {
            var rowCount = await BodyRows.CountAsync();
            if (rowIndex < 0 || rowIndex >= rowCount)
                throw new ArgumentOutOfRangeException(nameof(rowIndex), $"Row index {rowIndex} is out of range. Row count: {rowCount}.");

            var row = BodyRows.Nth(rowIndex);
            await Exec.ClickAsync(row);
        }

        private static string Normalize(string? s)
            => (s ?? "").Trim().Replace("\u00A0", " ");
    }
}