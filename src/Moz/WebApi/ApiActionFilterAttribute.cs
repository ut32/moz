using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Moz.WebApi
{
    public class ApiActionFilterAttribute:ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine("ApiActionFilterAttribute:OnActionExecuted");
            base.OnActionExecuted(context);
        }
    }
}