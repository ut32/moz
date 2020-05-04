using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Moz.Admin.Layui.Common;
using Moz.Administration.Models;
using Moz.Core;
using Moz.Exceptions;
using Moz.Utils;
using Moz.Utils.FileManage;

namespace Moz.Admin.Layui.Controllers
{
    public class UploadController : AdminAuthBaseController
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IWorkContext _workContext;
        private readonly IFileManager _fileManager;

        public UploadController(IWebHostEnvironment env, IWorkContext workContext, IFileManager fileManager)
        {
            this._environment = env;
            this._workContext = workContext;
            _fileManager = fileManager;
        }
 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [RequestFormLimits(MultipartBodyLengthLimit = 539401174)]
        [RequestSizeLimit(539401174)]
        public IActionResult Index(UploadModel model)
        {
            var member = _workContext.CurrentMember;
            if(member==null)
                throw new AlertException("未登录");
            
            
            var guid = Guid.NewGuid().ToString("N");
            var extension = "png";
            if (model.File.ContentType.Contains("jpeg") || model.File.ContentType.Contains("jpg"))
            {
                extension = "jpg";
            }else if (model.File.ContentType.Equals("video/mp4", StringComparison.OrdinalIgnoreCase))
            {
                extension = "mp4";
            }

            var file = $"upload/{guid}.{extension}";
            
            var uploadResult = _fileManager.Upload(new UploadFile()
            {
                Data = model.File.OpenReadStream(),
                ContentType = "",
                FileName = file
            });
            
            /*
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
            */
            
            return RespJson( new { FullPath = uploadResult.Server + uploadResult.RelativePath, RelativePath = uploadResult.Server +"/"+uploadResult.RelativePath });
        }
    }
}