
using Microsoft.AspNetCore.Mvc;
using Moz.WebApi;

namespace Moz.Admin.Api.Common
{
    //[ExceptionHandler(typeof(MyExceptionHandler))] 
    //[ValidationFilter]
    [AdminApiRoute]
    [ApiVersion( "1.0" )]
    public class AdminApiBaseController : ApiBaseController
    {
        
    }
}