using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Bus.Dtos.AdPlaces
{
    /// <summary>
    /// tab_ad_place
    /// </summary>
    [Validator(typeof(PagedQueryAdPlaceDtoValidator))]
    public class PagedQueryAdPlaceDto
    {
        #region 属性
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public string Keyword{ get;set; }
        #endregion     
    }

    public class QueryAdPlaceItem
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get;set; } 
        /// <summary>
        /// 
        /// </summary>
        public string Title { get;set; } 
        /// <summary>
        /// 
        /// </summary>
        public string Code { get;set; } 
        /// <summary>
        /// 
        /// </summary>
        public string Desc { get;set; } 
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime AddTime { get;set; }

        public string AddTimeString => AddTime.ToString("yyyy-MM-dd");
    }
    
    public class PagedQueryAdPlaceDtoValidator : MozValidator<PagedQueryAdPlaceDto>
    {
        public PagedQueryAdPlaceDtoValidator(ILocalizationService localizationService)
        {
            
        }
    }
    
}
