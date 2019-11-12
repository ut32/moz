using Microsoft.AspNetCore.Http;

namespace Moz.Administration.Models
{
    public class UploadModel
    {
        public IFormFile File { get; set; }
    }
}