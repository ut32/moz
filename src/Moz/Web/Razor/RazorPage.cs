using Microsoft.AspNetCore.Html;
using Moz.Common;
using Moz.Core;
using Moz.Utils;

namespace Moz.Web.Razor
{
    public abstract class RazorPage<TModel> : Microsoft.AspNetCore.Mvc.Razor.RazorPage<TModel>
    {
        private HttpContextHelper _httpContextHelper;

        public override void EndContext()
        {
            //修改xpjax
            _httpContextHelper = EngineContext.Current.Resolve<HttpContextHelper>();
            if (_httpContextHelper.GetHeaderValueAs<bool>("X-PJAX")) Layout = null;
            base.EndContext();
        }

        protected HtmlString L(string value)
        {
            return new HtmlString($"x:{value}");
        }
    }
}