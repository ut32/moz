using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Bus.Dtos.Permissions
{
    /// <summary>
    /// tab_permission
    /// </summary>
    [Validator(typeof(PagedQueryPermissionDtoValidator))]
    public class PagedQueryPermissionDto
    {
        #region 属性
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public string Keyword{ get;set; }
        #endregion     
    }
    
    public class QueryPermissionItem
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
        public string Code { get;set; } 
        /// <summary>
        /// 
        /// </summary>
        public bool IsActive { get;set; } 
        /// <summary>
        /// 
        /// </summary>
        public long? ParentId { get;set; } 
        /// <summary>
        /// 
        /// </summary>
        public int OrderIndex { get;set; } 
        /// <summary>
        /// 
        /// </summary>
        public bool IsSystem { get;set; } 
    }
    
    public class PagedQueryPermissionDtoValidator : MozValidator<PagedQueryPermissionDto>
    {
        public PagedQueryPermissionDtoValidator(ILocalizationService localizationService)
        {
            
        }
    }
    
}
