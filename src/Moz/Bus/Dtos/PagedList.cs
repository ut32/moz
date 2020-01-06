using System.Collections.Generic;

namespace Moz.Bus.Dtos
{
    public class PagedList<T>:PagedListBase
    {
        public IList<T> List { get; set; }

        public PagedList()
        {
            List = new List<T>();
        }
    }
}