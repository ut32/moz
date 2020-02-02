using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Moz.Admin.Layui.Common;
using Moz.Administration.Models;
using Moz.Core;
using Moz.Exceptions;

namespace Moz.Admin.Layui.Controllers
{
    public class UploadController : AdminAuthBaseController
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IWorkContext _workContext;

        public UploadController(IWebHostEnvironment env, IWorkContext workContext)
        {
            this._environment = env;
            this._workContext = workContext;
        }
 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IActionResult Index(UploadModel model)
        {
            var member = _workContext.CurrentMember;
            if(member==null)
                throw new MozException("未登录");
            
            var guid = Guid.NewGuid().ToString("N");
            var extension = "png";
            if (model.File.ContentType.Contains("jpeg") || model.File.ContentType.Contains("jpg"))
            {
                extension = "jpg";
            }else if (model.File.ContentType.Equals("video/mp4", StringComparison.OrdinalIgnoreCase))
            {
                extension = "mp4";
            }

            var file = $"/upload/{guid}.{extension}";
            var filePath = Path.Combine(_environment.ContentRootPath,"wwwroot/upload");
            filePath = Path.Combine(filePath,$"{guid}.{extension}");
            using (var stream = new FileStream(filePath,FileMode.CreateNew))
            {
                model.File.CopyTo(stream);
            }
            
            return RespJson( new { FullPath = file.GetFullPath(), RelativePath = file });
        }
    }
}