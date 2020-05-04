using System.IO;

namespace Moz.Utils.FileManage
{
    public class UploadFile 
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public Stream Data { get; set; }
    }
}