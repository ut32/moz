using Microsoft.AspNetCore.Mvc;
using Moz.Admin.Api.Common;
using Moz.Admin.Api.Models.User;
using Moz.Auth;
using Moz.WebApi;

namespace Moz.Admin.Api.Controllers
{
    public class UserController:AdminApiBaseController
    {

        private readonly IAuthService _authService;

        public UserController(IAuthService authService)
        {
            _authService = authService;
        }

        [Route("login")]
        [HttpPost]
        public ApiResult<LoginAuthResult> Login(ApiRequest<LoginReqModel> param)
        {
            var result = _authService.LoginWithUsernamePassword(new LoginWithUsernamePasswordRequest()
            {
                Username = param.Data.Username,
                Password = param.Data.Password
            });
            return result; 
        }

        [Route("Insert")]public ApiResult<string> Insert() => "添加成功";
        [Route("Update")]public ApiResult Update()  => ApiOk();
        [Route("Delete")]public ApiResult Delete()  => ApiError("删除失败");
        [Route("Get")]public ApiResult<int> Get()  => 0;

    }
}