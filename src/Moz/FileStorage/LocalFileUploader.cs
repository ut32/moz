using System;
using System.ComponentModel;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Moz.Settings;

namespace Moz.FileStorage
{ 
    [Description("本地上传")]
    public class LocalFileUploader : IFileUploader
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        
        public LocalFileUploader(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public UploadResult Upload(UploadFile uploadFile) 
        {
            var guid = Guid.NewGuid().ToString("N");
            var extension = "png";
            if (uploadFile.FormFile.ContentType.Contains("jpeg") || uploadFile.FormFile.ContentType.Contains("jpg"))
            {
                extension = "jpg";
            }else if (uploadFile.FormFile.ContentType.Equals("video/mp4", StringComparison.OrdinalIgnoreCase))
            {
                extension = "mp4";
            }

            var dt = DateTime.Now.ToString("yyyyMM");
            var file = $"/upload/{dt}/{guid}.{extension}";
            var filePath = Path.Combine(_webHostEnvironment.ContentRootPath,$"wwwroot/upload/{dt}");
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            filePath = Path.Combine(filePath,$"{guid}.{extension}");
            using (var stream = new FileStream(filePath,FileMode.CreateNew))
            {
                uploadFile.FormFile.CopyTo(stream);
            }
            return new UploadResult
            {
                Code = 0,
                Error = null,
                Data = new UploadInfo{ Path = file }
            };
        }
    }
}