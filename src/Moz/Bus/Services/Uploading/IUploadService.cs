using System.Collections.Generic;
using Moz.Utils.FileManager;

namespace Moz.Service.Uploading
{
    public interface IUploadService
    {
        List<UploadResult> UploadFiles(IEnumerable<UploadFile> files);
    }
}