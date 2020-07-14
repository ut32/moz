namespace Moz.FileStorage
{
    public interface IFileManager
    {
        UploadResult Upload(UploadFile file);
    }
}