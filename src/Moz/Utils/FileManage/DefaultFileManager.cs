using Moz.Settings;

namespace Moz.Utils.FileManage
{ 
    internal class DefaultFileManager : IFileManager
    {
        private readonly GlobalSettings _globalSettings;
        private readonly IFileManager _fileManager;
        
        public DefaultFileManager(GlobalSettings globalSettings)
        {
            _globalSettings = globalSettings; 
            //_fileManager = new AliyunOssManager();
        }

        public UploadResult Upload(UploadFile file)
        {
            return _fileManager.Upload(file);
        }
    }
}