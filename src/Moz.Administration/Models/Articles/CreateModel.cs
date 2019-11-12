using Moz.Domain.Dtos.Articles.ArticleModels;

namespace Moz.Administration.Models.Articles
{
    public class CreateModel
    {
        public CreateModel()
        {
            
        }
        public long Type { get; set; }
        public GetArticleModelDetailResponse ArticleModel { get; set; }
    }
}