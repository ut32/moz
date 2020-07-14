using System;
using System.Linq;
using Moz.Common.Types;
using Moz.Core;
using Moz.Settings;

namespace Moz.FileStorage
{
    public class DefaultFileManager : IFileManager
    {
        private readonly GlobalSettings _globalSettings;
        private readonly IFileUploader _fileUploader;
        
        public DefaultFileManager(GlobalSettings globalSettings)
        {
            _globalSettings = globalSettings;
            var fileUploaders = TypeFinder.FindClassesOfType<IFileUploader>();
            var curFileUploadType = fileUploaders.FirstOrDefault(it => it.UniqueId.Equals(_globalSettings.FileUploader, StringComparison.OrdinalIgnoreCase));
            if (curFileUploadType == null)
            {
                _fileUploader = EngineContext.Current.Resolve<LocalFileUploader>();
            }
            else
            {
                _fileUploader = EngineContext.Current.Resolve(curFileUploadType.Type) as IFileUploader;
            }
            
        }

        public UploadResult Upload(UploadFile file)
        {
            if (_globalSettings.DisableSite)
            {
                //网站关闭
            }
            return _fileUploader.Upload(file);
        }
    }
}