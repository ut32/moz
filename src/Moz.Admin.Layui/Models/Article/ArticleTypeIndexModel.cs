using Moz.Bus.Models.Articles;
using Moz.Common;
using Moz.Utils;

namespace Moz.Presentation.Administration.Models.Articles
{
    public class ArticleTypeIndexModel
    {
        public IPagedList<ArticleModel> PagedList { get; set; }
        //public ArticleTypeSearchModel Search { get; set; }
    }
}