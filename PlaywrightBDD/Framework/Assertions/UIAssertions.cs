using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Framework.Assertions
{
    public class UiAssert
    {
        private readonly IPage _page;

        public UiAssert(IPage page)
        {
            _page = page;
        }

        public async Task ElementVisible(string selector)
        {
            var visible = await _page.IsVisibleAsync(selector);
            GenericAssert.IsTrue(visible, $"Expected element '{selector}' to be visible.");
        }

        public async Task ElementHidden(string selector)
        {
            var visible = await _page.IsVisibleAsync(selector);
            GenericAssert.IsTrue(!visible, $"Expected element '{selector}' to be hidden.");
        }

        public async Task TextContains(string selector, string expected)
        {
            var text = await _page.InnerTextAsync(selector);
            GenericAssert.IsTrue(text.Contains(expected),
                $"Expected '{selector}' to contain '{expected}', but was '{text}'.");
        }

        public void UrlContains(string expected)
        {
            GenericAssert.IsTrue(_page.Url.Contains(expected),
                $"Expected URL to contain '{expected}', but was '{_page.Url}'.");
        }

        public void CellEquals(string column, string expected, string actual, string rowKey)
        {
            GenericAssert.IsEqual(actual, expected,
                $"Cell mismatch. RowKey='{rowKey}', Column='{column}'. Expected='{expected}', Actual='{actual}'.");
        }
    }
}