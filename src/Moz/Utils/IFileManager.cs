using Moz.Utils.FileManage;

namespace Moz.Utils
{
    public interface IFileManager
    {
        UploadResult Upload(UploadFile file);
    }
}