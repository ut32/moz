using System.Collections.Generic;
using Moz.Utils.FileManage;

namespace Moz.Service.Uploading
{
    public interface IUploadService
    {
        List<UploadResult> UploadFiles(IEnumerable<UploadFile> files);
    }
}