namespace Moz.FileStorage
{
    public class UploadResult
    {
        public int Code { get; set; }
        public string Error { get; set; }
        public UploadInfo Data { get; set; }
    }
}