using System.Collections.Generic;
using System.Linq;
using Moz.Common.Form;
using Moz.Common.Types;

namespace Moz.FileStorage
{
    public class FileStorageDataSource : ISelectDataSource
    {
        public List<SelectItem> GetDataSource()
        {
            var fileUploaders = TypeFinder.FindClassesOfType<IFileUploader>();
            return fileUploaders.Select(it => new SelectItem
            {
                Name = it.DisplayName,
                Value = it.UniqueId
            }).ToList();
        }
    }
}