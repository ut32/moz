using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moz.Utils;
using Moz.Utils.FileManage;

namespace Moz.Service.Uploading
{
    public class UploadService : IUploadService
    {
        private readonly IFileManager _uploadManager;

        public UploadService(IFileManager uploadManager)
        {
            _uploadManager = uploadManager;
        }

        public List<UploadResult> UploadFiles(IEnumerable<UploadFile> files)
        {
            var list = new List<UploadResult>();
            Task.WaitAll(files.Select(file => Task.Factory.StartNew(f =>
            {
                if (f is UploadFile myfile)
                {
                    //var result = _uploadManager.Uploader.Upload(myfile);
                    //list.Add(result);
                }
            }, file)).ToArray());

            //存入数据库


            return list;
        }

//        public List<UploadResult> UploadFiles(IEnumerable<UploadFile> files)
//        {
//            throw new System.NotImplementedException();
//        }
    }
}