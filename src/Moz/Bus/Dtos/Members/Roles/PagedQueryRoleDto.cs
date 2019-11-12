using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.CMS.Dtos;
using Moz.Validation;

namespace Moz.Bus.Dtos.Members.Roles
{
    /// <summary>
    /// tab_role
    /// </summary>
    [Validator(typeof(PagedQueryRoleRequestValidator))]
    public class PagedQueryRoleRequest
    {
        #region 属性
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public string Keyword{ get;set; }
        #endregion     
    }
    
    
    public class PagedQueryRoleResponse: PagedResult<QueryRoleItem>
    {
    
    }
    
    public class QueryRoleItem
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
        public bool IsActive { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public bool IsAdmin { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public bool IsSystem { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string Code { get;set; } 
    }
    
    public class PagedQueryRoleRequestValidator : MozValidator<PagedQueryRoleRequest>
    {
        public PagedQueryRoleRequestValidator(ILocalizationService localizationService)
        {
            
        }
    }
    
}
