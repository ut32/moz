using Moz.Configuration;

namespace Moz.Utils.FileManager
{ 
    internal class FileManager : IFileManager
    {
        private readonly CommonSettings _commonSettings;
        private readonly IFileManager _fileManager;
        
        public FileManager(CommonSettings commonSettings)
        {
            _commonSettings = commonSettings; 
            _fileManager = new AliyunOssManager();
        }

        public UploadResult Upload(UploadFile file)
        {
            return _fileManager.Upload(file);
        }
    }
}