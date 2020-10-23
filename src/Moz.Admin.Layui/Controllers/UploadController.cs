using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Moz.Admin.Layui.Common;
using Moz.Administration.Models;
using Moz.Core;
using Moz.Exceptions;
using Moz.FileStorage;
using Moz.Utils;

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

            var uploadResult = _fileManager.Upload(new UploadFile()
            {
                FormFile = model.File
            });
            
            return Json(uploadResult);
        }
    }
}