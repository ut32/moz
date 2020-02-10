using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;
using Moz.Exceptions;

namespace Moz.Validation
{
    public class ValidationFilterAttribute:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errMsg = context.ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .FirstOrDefault();
                if (errMsg.IsNullOrEmpty()) 
                    errMsg = "发生未知错误(验证不通过)";
                throw new AlertException(errMsg);
            }
            base.OnActionExecuting(context);
        }
    }
}