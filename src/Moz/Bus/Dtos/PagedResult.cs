using System.Collections.Generic;
using Moz.CMS.Dtos;

namespace Moz.Bus.Dtos
{
    public class PagedResult<T>:PagedResultBase
    {
        public IList<T> List { get; set; }

        public PagedResult()
        {
            List = new List<T>();
        }
        
    }
}