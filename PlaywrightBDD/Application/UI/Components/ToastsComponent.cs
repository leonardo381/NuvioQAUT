using Application.UI.Components.Base;
using Framework.Core;
using Microsoft.Playwright;

namespace Application.UI.Components
{
    public class ToastsComponent : UIComponent
    {
        public ToastsComponent(ILocator root, ElementExecutor executor) : base(root, executor) { }
    }
}