using Microsoft.AspNetCore.Mvc;
using Moz.Admin.Api.Common;
using Moz.Admin.Api.Models.User;
using Moz.WebApi;

namespace Moz.Admin.Api.Controllers
{
    public class UserController:AdminApiBaseController
    {

        [Route("login")]
        [HttpPost]
        public ApiResult<string> Login(ApiRequest<LoginReqModel> param)
        {
            return "登录成功"; 
        }
        
    }
}