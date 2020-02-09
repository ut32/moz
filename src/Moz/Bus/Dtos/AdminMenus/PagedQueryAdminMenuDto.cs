using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Bus.Dtos.AdminMenus
{
    /// <summary>
    /// tab_admin_menu
    /// </summary>
    [Validator(typeof(PagedQueryAdminMenusDtoValidator))]
    public class PagedQueryAdminMenusDto
    {
        #region 属性
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public string Keyword{ get;set; }
        #endregion     
    } 

    public class QueryAdminMenuItem
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
        public long? ParentId { get;set; } 
        /// <summary>
        /// 
        /// </summary>
        public string Link { get;set; } 
        /// <summary>
        /// 
        /// </summary>
        public int OrderIndex { get;set; } 
        /// <summary>
        /// 
        /// </summary>
        public string Icon { get;set; } 
        
        public bool IsSystem { get; set; }
    }
    
    public class PagedQueryAdminMenusDtoValidator : MozValidator<PagedQueryAdminMenusDto>
    {
        public PagedQueryAdminMenusDtoValidator(ILocalizationService localizationService)
        {
            
        }
    }
    
}
