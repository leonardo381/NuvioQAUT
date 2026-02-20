using Application.UI.Components.Base;
using Framework.Core;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.UI.Components
{
    public class GridComponent : UIComponent
    {
        private ILocator HeaderCells => Root.Locator("thead tr th");
        private ILocator BodyRows => Root.Locator("tbody tr");
        private ILocator LoadingSpinner => Root.Locator(".loading, .spinner, [data-loading='true']");

        public GridComponent(ILocator root, ElementExecutor executor) : base(root, executor) { }

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

        public ILocator RowByText(string text)
            => BodyRows.Filter(new LocatorFilterOptions { HasTextString = text });

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

        public async Task<string> GetCellTextAsync(ILocator row, int columnIndex)
        {
            var cell = row.Locator($"td:nth-child({columnIndex + 1})");
            return Normalize(await cell.First.InnerTextAsync());
        }

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

        private static string Normalize(string? s)
            => (s ?? "").Trim().Replace("\u00A0", " ");
    }
}