using Microsoft.Playwright;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.UI.Components
{
    public class GridComponent
    {
        private readonly IPage _page;
        private readonly ILocator _root;

        public GridComponent(IPage page, ILocator root)
        {
            _page = page;
            _root = root;
        }

        private ILocator Rows => _root.Locator("tbody tr");
        private ILocator HeaderCells => _root.Locator("thead th");

        public async Task<int> GetRowCountAsync() => await Rows.CountAsync();
        public async Task<int> GetColumnCountAsync() => await HeaderCells.CountAsync();

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
                var row = new List<string>();
                for (int c = 0; c < colCount; c++)
                {
                    row.Add(await GetCellTextAsync(r, c));
                }
                table.Add(row);
            }

            return table;
        }
    }
}