using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moz.Bus.Dtos;

namespace Moz.Web.Components
{
    [ViewComponent(Name = "Pager")]
    public class PagerViewComponent: ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(PagedListBase result)
        {
            // ReSharper disable once Mvc.ViewComponentViewNotResolved
            return Task.FromResult((IViewComponentResult)View("DefaultPagerView", result));
        }
    }
}