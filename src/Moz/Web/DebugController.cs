using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Moz.Web
{
    [ApiExplorerSettings(IgnoreApi =true)]
    public class DebugController : Controller
    {
        private readonly IHostingEnvironment _environment;
        public DebugController(IHostingEnvironment env)
        {
            this._environment = env;
        }

        // GET
        public IActionResult Resource()
        {
            var views = string.Join(" ", this.GetType().Assembly.GetManifestResourceNames());
            return Json(views);
        }

        public IActionResult Env()
        {
            return Json(_environment);
        }
    }
}