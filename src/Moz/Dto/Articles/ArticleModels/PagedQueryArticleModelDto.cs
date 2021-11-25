using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Bus.Dtos.Articles.ArticleModels
{
    /// <summary>
    /// tab_article_model
    /// </summary>
    [Validator(typeof(PagedQueryArticleModelRequestValidator))]
    public class PagedQueryArticleModelRequest
    {
        #region 属性
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public string Keyword{ get;set; }
        #endregion     
    }
    
    
    public class PagedQueryArticleModelResponse: PagedList<QueryArticleModelItem>
    {
    
    }
    
    public class QueryArticleModelItem
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get;set; } 
        /// <summary>
        /// 
        /// </summary>
        public string Name { get;set; } 
        /// <summary>
        /// 
        /// </summary>
        public string Configuration { get;set; } 
        /// <summary>
        /// 
        /// </summary>
        public long? CategoryId { get;set; } 
    }
    
    public class PagedQueryArticleModelRequestValidator : MozValidator<PagedQueryArticleModelRequest>
    {
        public PagedQueryArticleModelRequestValidator(ILocalizationService localizationService)
        {
            
        }
    }
    
}
