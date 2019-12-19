using Moz.Bus.Models.Articles;
using Moz.Common;
using Moz.Utils;

namespace Moz.Presentation.Administration.Models.Articles
{
    public class ArticleListIndexModel
    {
        public IPagedList<Article> PagedList { get; set; }
        public ArticleSearchModel Search { get; set; }
    }
}