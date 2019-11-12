using System;
using Moz.Bus.Dtos;

namespace Moz.CMS.Dtos
{
    public class PagedResultBase : BaseRespData
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        
        public int TotalPages
        {
            get
            {
                if (PageSize == 0) return TotalCount;
                var pages = TotalCount / PageSize;
                if (TotalCount % PageSize > 0)
                    pages++;
                return pages;
            }
        }
        public bool HasPreviousPage => Page > 1;
        public bool HasNextPage => Page < TotalPages;
        public int FirstRowOnPage => (Page - 1) * PageSize + 1;
        public int LastRowOnPage => Math.Min(Page * PageSize, TotalCount);
    }
}