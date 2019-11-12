using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moz.Common.ValidateCode;

namespace Moz.Web.ValidateCode
{
    
    public class ValidateCodeController : Controller
    {
        [Route("/validatecode")]
        [ApiExplorerSettings(IgnoreApi =true)]
        public ActionResult Generate()
        {
            var vCode = new SafeCodeImage();
            var rand = new Random(Guid.NewGuid().GetHashCode());
            var code = rand.Next(1000, 9999).ToString();
            HttpContext.Session.SetString("code", code);
            var imagedate = vCode.GetImage(code);
            return File(imagedate, @"image/jpeg");
        }

        /// <summary>
        /// 这是一个方法
        /// </summary>
        /// <param name="a">年龄</param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi =false)]
        [Route("api/test")]
        [HttpGet]
        public string Test(int a)
        {
            return "hi";
        }
        
    }
}