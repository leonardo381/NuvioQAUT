using Framework.Core;
using Microsoft.Playwright;

namespace Application.UI.Pages
{
    public class DashboardPage : BasePage
    {
        public DashboardPage(IPage page, ElementExecutor executor) : base(page, executor) { }
    }
}