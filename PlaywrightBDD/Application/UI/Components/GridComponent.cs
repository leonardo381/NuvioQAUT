using Application.UI.Components.Base;
using Microsoft.Playwright;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.UI.Components
{
    public sealed class GridComponent : UIComponent
    {
        public GridComponent(IPage page, ILocator root) : base(page, root) { }

        private ILocator Rows => Root.Locator("tbody tr");
        private ILocator HeaderCells => Root.Locator("thead th");

        public Task<int> GetRowCountAsync() => Rows.CountAsync();
        public Task<int> GetColumnCountAsync() => HeaderCells.CountAsync();

        public ILocator Cell(int rowIndex, int colIndex)
            => Rows.Nth(rowIndex).Locator("td").Nth(colIndex);

        public async Task<string> GetCellTextAsync(int rowIndex, int colIndex)
            => (await Cell(rowIndex, colIndex).InnerTextAsync()).Trim();

        public async Task<List<List<string>>> ReadAllCellsAsync()
        {
            var table = new List<List<string>>();
            var rowCount = await GetRowCountAsync();
            var colCount = await GetColumnCountAsync();

            for (int r = 0; r < rowCount; r++)
            {
                var row = new List<string>(colCount);
                for (int c = 0; c < colCount; c++)
                    row.Add(await GetCellTextAsync(r, c));

                table.Add(row);
            }

            return table;
        }

        public async Task<bool> ContainsTextAsync(string text)
            => await Root.Locator($"text={text}").CountAsync() > 0;
    }
}