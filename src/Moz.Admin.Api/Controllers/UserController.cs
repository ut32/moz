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

        [Route("Insert")]public ApiResult<string> Insert() => "添加成功";
        [Route("Update")]public ApiResult Update()  => ApiOk();
        [Route("Delete")]public ApiResult Delete()  => ApiError("删除失败");
        
        
        [Route("Get")]
        [ApiActionFilter]
        public ApiResult<int> Get()  => 0;

    }
}