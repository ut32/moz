using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moz.Admin.Api.Common;
using Moz.Admin.Api.Models.User;
using Moz.Exceptions;

namespace Moz.Admin.Api.Controllers
{
    [ApiVersion("1.0")]
    public class UserController:AdminApiBaseController
    {

        [Route("get1")]
        public string GetName()
        {
            throw Alert("没毛病");
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