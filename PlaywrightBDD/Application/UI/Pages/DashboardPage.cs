using Framework.Core;
using Framework.Engine;
using Microsoft.Playwright;

namespace Application.UI.Pages
{
    public class DashboardPage : BasePage
    {
        public DashboardPage(IPage page, ElementExecutor executor, ExecutionSettings settings) : base(page, executor, settings) { }
    }
}