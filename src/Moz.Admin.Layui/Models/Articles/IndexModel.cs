
using Moz.Domain.Dtos.Articles.ArticleModels;

namespace Moz.Administration.Models.Articles
{
    public class IndexModel
    {
        public long Type { get; set; }
        public GetArticleModelDetailResponse ArticleModel { get; set; }
    }
}
