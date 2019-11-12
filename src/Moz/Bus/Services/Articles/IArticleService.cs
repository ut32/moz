using Moz.Biz.Dtos.Articles;
using Moz.Biz.Dtos.Articles.ArticleModels;
using Moz.Domain.Dtos.Articles;
using Moz.Domain.Dtos.Articles.ArticleModels;

namespace Moz.Biz.Services.Articles
{
    public interface IArticleService
    {

        #region 文章模型
        
        CreateArticleModelResponse CreateArticleModel(CreateArticleModelRequest request);
        UpdateArticleModelResponse UpdateArticleModel(UpdateArticleModelRequest request);
        DeleteArticleModelResponse DeleteArticleModel(DeleteArticleModelRequest request);
        GetArticleModelDetailResponse GetArticleModelDetail(GetArticleModelDetailRequest request);
        PagedQueryArticleModelResponse PagedQueryArticleModels(PagedQueryArticleModelRequest request);
        
        #endregion

        #region 文章
        
        CreateArticleResponse CreateArticle(CreateArticleRequest request);
        UpdateArticleResponse UpdateArticle(UpdateArticleRequest request);
        DeleteArticleResponse DeleteArticle(DeleteArticleRequest request);
        BulkDeleteArticlesResponse BulkDeleteArticles(BulkDeleteArticlesRequest request);
        GetArticleDetailResponse GetArticleDetail(GetArticleDetailRequest request);
        PagedQueryArticleResponse PagedQueryArticles(PagedQueryArticleRequest request);
        
        #endregion
    }
}