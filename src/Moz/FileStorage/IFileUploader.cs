namespace Moz.FileStorage
{
    public interface IFileUploader
    {
        UploadResult Upload(UploadFile file);
    }
}