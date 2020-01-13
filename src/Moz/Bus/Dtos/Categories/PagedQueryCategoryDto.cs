using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Bus.Dtos.Categories
{
    /// <summary>
    /// tab_category
    /// </summary>
    [Validator(typeof(PagedQueryCategoryDtoValidator))]
    public class PagedQueryCategoryDto
    {
        #region 属性
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public string Keyword{ get;set; }
        #endregion     
    }
    
    public class QueryCategoryItem
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
        public string Alias { get;set; } 
        /// <summary>
        /// 
        /// </summary>
        public string Desciption { get;set; } 
        /// <summary>
        /// 
        /// </summary>
        public int OrderIndex { get;set; } 
        /// <summary>
        /// 
        /// </summary>
        public long? ParentId { get;set; } 
        /// <summary>
        /// 
        /// </summary>
        public string Path { get;set; } 
    }
    
    public class PagedQueryCategoryDtoValidator : MozValidator<PagedQueryCategoryDto>
    {
        public PagedQueryCategoryDtoValidator(ILocalizationService localizationService)
        {
            
        }
    }
    
}
