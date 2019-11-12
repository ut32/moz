using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Moz.Exceptions;
using Moz.Validation;

namespace Moz.Administration.Common
{
    [AdminArea]
    [ExceptionHandler(typeof(AdminExceptionHandler))]
    [ValidationFilter]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AdminBaseController : Controller
    {
        [NonAction] 
        public ActionResult RespJson(object data)
        {
            var code = 0;
            var message = "操作成功";
            var result = new Dictionary<string, object>();
            foreach (var property in data.GetType().GetProperties())
            {
                if (property.Name.Equals("code", StringComparison.InvariantCultureIgnoreCase))
                {
                    int.TryParse(property.GetValue(data,null)?.ToString() ?? "0", out code);
                }
                else if (property.Name.Equals("message", StringComparison.InvariantCultureIgnoreCase))
                {
                    message = property.GetValue(data, null)?.ToString() ?? "";
                }
                else {
                    result.Add(property.Name, property.GetValue(data, null));
                }
            }
            return Json(new
            {
                Code = code,
                Message = message,
                Data = result
            });
        }
    }
}