
using Microsoft.AspNetCore.Mvc;
using Moz.WebApi;

namespace Moz.Admin.Api.Common
{
    //[ExceptionHandler(typeof(MyExceptionHandler))] 
    //[ValidationFilter]
    [ApiAdminRoute]
    [TypeFilter(typeof(ApiActionFilterAttribute))]
    public class AdminApiBaseController : ApiBaseController
    {

    }
}