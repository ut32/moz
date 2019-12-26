using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moz.Admin.Api.Common;
using Moz.Admin.Api.Models.User;
using Moz.WebApi;

namespace Moz.Admin.Api.Controllers
{
    [ApiVersion("1.0")]
    public class UserController:AdminApiBaseController
    {

        [Route("login")]
        [HttpPost]
        public ApiResult<string> Login(ApiRequest<LoginReqModel> param)
        {
            return "登录成功";
        }

        [ApiActionFilter]
        [Route("get")]
        [ApiVersion("1")]
        public ApiResult<int> Get()
        {
            throw new Exception("ss");
        }
        
        
        [Route("get")]
        [ApiVersion("2")]
        public ApiResult<string> GetV2()
        {
            return "v2";
        }
        
        [Route("get1")]
        public string GetName()
        {

            return "草了";
        }
        
        [Route("get2")]
        public ActionResult<string> GetName1()
        {
            return Ok();
        }
        
        [Route("get3")]
        public async Task<ActionResult<string>> GetName3()
        {
            await Task.Delay(0);
            return "error";
        }
    }
}