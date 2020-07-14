using System.Collections.Generic;

namespace Moz.Common.Form
{
    public interface ISelectDataSource
    {
        List<SelectItem> GetDataSource();
    }
}