using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Bus.Dtos.Roles
{
    /// <summary>
    /// tab_role
    /// </summary>
    [Validator(typeof(PagedQueryRoleDtoValidator))]
    public class PagedQueryRoleDto
    {
        #region 属性
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public string Keyword{ get;set; }
        #endregion     
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
        public string Code { get;set; } 
        /// <summary>
        /// 
        /// </summary>
        public bool IsAdmin { get;set; } 
        /// <summary>
        /// 
        /// </summary>
        public bool IsSystem { get;set; } 
    }
    
    public class PagedQueryRoleDtoValidator : MozValidator<PagedQueryRoleDto>
    {
        public PagedQueryRoleDtoValidator(ILocalizationService localizationService)
        {
            
        }
    }
    
}
