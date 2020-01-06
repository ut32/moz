using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Bus.Dtos.AdminMenus
{
    /// <summary>
    /// tab_admin_menu
    /// </summary>
    [Validator(typeof(PagedQueryAdminMenuRequestValidator))]
    public class PagedQueryAdminMenuRequest
    {
        #region 属性
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public string Keyword{ get;set; }
        #endregion     
    }
    
    
    public class PagedQueryAdminMenuResponse: PagedList<QueryAdminMenuItem>
    {
    
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
    
    public class PagedQueryAdminMenuRequestValidator : MozValidator<PagedQueryAdminMenuRequest>
    {
        public PagedQueryAdminMenuRequestValidator(ILocalizationService localizationService)
        {
            
        }
    }
    
}
