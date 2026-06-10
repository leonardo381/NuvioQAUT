using Microsoft.Playwright;

namespace Application.UI.Components
{
    public class ToastsComponent
    {
        private readonly ILocator _root;

        public ToastsComponent(ILocator root)
        {
            _root = root;
        }
    }
}
