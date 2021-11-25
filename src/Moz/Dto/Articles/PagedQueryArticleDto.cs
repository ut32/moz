using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Bus.Dtos.Articles
{
    /// <summary>
    /// tab_article
    /// </summary>
    [Validator(typeof(PagedQueryArticleRequestValidator))]
    public class PagedQueryArticleDto
    {
        #region 属性 
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        
        public long ArticleModelId { get; set; }
        
        public long? CategoryId { get; set; }
        
        public string Keyword{ get;set; }
        
        #endregion     
    }
    
    
    public class PagedQueryArticles: PagedList<QueryArticleItem>
    {
    
    }
    
    public class QueryArticleItem : Models.Articles.Article
    {
        
    }
    
    public class PagedQueryArticleRequestValidator : MozValidator<PagedQueryArticleDto>
    {
        public PagedQueryArticleRequestValidator(ILocalizationService localizationService)
        {
            
        }
    }
    
}
