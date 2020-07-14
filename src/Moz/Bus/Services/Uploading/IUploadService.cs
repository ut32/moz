using System.Collections.Generic;
using Moz.FileStorage;

namespace Moz.Bus.Services.Uploading
{
    public interface IUploadService
    {
        List<UploadResult> UploadFiles(IEnumerable<UploadFile> files);
    }
}