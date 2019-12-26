
using Microsoft.AspNetCore.Mvc;
using Moz.WebApi;

namespace Moz.Admin.Api.Common
{
    //[ExceptionHandler(typeof(MyExceptionHandler))] 
    //[ValidationFilter]
    [ApiAdminRoute]
    [ApiActionFilter]
    public class AdminApiBaseController : ApiBaseController
    {
        
    }
}