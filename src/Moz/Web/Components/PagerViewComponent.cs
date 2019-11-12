using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moz.CMS.Dtos;
using Moz.CMS.Dtos;

namespace Moz.Web.Components
{
    public class PagerViewComponent: ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(PagedResultBase result)
        {
            // ReSharper disable once Mvc.ViewComponentViewNotResolved
            return Task.FromResult((IViewComponentResult)View("DefaultPagerView", result));
        }
    }
}