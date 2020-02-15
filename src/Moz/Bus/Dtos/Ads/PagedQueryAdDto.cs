using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Bus.Dtos.Ads
{
    /// <summary>
    /// tab_ad
    /// </summary>
    [Validator(typeof(PagedQueryAdsDtoValidator))]
    public class PagedQueryAdsDto
    {
        #region 属性
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        
        public long AdPlaceId { get; set; }
        
        public string Keyword{ get;set; }
        #endregion     
    }

    public class QueryAdItem
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get;set; } 
        /// <summary>
        /// 
        /// </summary>
        public long AdPlaceId { get;set; } 
        /// <summary>
        /// 
        /// </summary>
        public string Title { get;set; } 
        /// <summary>
        /// 
        /// </summary>
        public string ImagePath { get;set; } 
        /// <summary>
        /// 
        /// </summary>
        public string TargetUrl { get;set; } 
        /// <summary>
        /// 
        /// </summary>
        public int Order { get;set; } 
        /// <summary>
        /// 
        /// </summary>
        public bool IsShow { get;set; }  
    }
    
    public class PagedQueryAdsDtoValidator : MozValidator<PagedQueryAdsDto>
    {
        public PagedQueryAdsDtoValidator(ILocalizationService localizationService)
        {
            
        }
    }
    
}
