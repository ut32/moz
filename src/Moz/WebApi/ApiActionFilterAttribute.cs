using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Moz.WebApi
{
    public class ApiActionFilterAttribute:ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception == null)
            {
                if (context.Result is ObjectResult result)
                {
                    context.Result = new JsonResult(new {
                        Code = 0,
                        Message = "",
                        Data = result.Value
                    });
                }else if (context.Result is OkResult)
                {
                    context.Result = new JsonResult(new
                    {
                        Code = 0,
                        Message = "",
                    });
                }
            }
            else
            {
                context.Result = new JsonResult(new
                {
                    Code = 1000,
                    Message = context.Exception.Message
                });
                context.ExceptionHandled = true;
            }
            base.OnActionExecuted(context);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine($"ApiActionFilterAttribute:OnActionExecuting:{context==null}");
            base.OnActionExecuting(context);
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            Console.WriteLine($"ApiActionFilterAttribute:OnResultExecuting:{context}");

            base.OnResultExecuting(context);
        }

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            Console.WriteLine($"ApiActionFilterAttribute:OnResultExecuted:{context.Exception==null}");
            base.OnResultExecuted(context);
        }

        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Console.WriteLine($"ApiActionFilterAttribute:OnActionExecutionAsync : ");
            

            return base.OnActionExecutionAsync(context, next);
        }
    }
}