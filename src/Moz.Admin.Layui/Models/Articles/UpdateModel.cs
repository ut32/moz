using Moz.Biz.Dtos.Articles;
using Moz.Domain.Dtos.Articles.ArticleModels;

namespace Moz.Administration.Models.Articles
{
    public class UpdateModel
    {
         public GetArticleDetailResponse Article { get; set; }
         public GetArticleModelDetailResponse ArticleModel { get; set; }
    }
    
}