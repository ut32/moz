using System.IO;
using Microsoft.AspNetCore.Http;

namespace Moz.FileStorage
{
    public class UploadFile 
    {
        public IFormFile FormFile { get; set; }
    }
}