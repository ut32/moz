using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Domain.Dtos.Members.Permissions
{
    /// <summary>
    /// tab_permission
    /// </summary>
    [Validator(typeof(GetPermissionDetailRequestValidator))]
    public class GetPermissionDetailRequest
    {
        #region 属性
        
        /// <summary>
        /// 
        /// </summary>
        public long Id { get;set; } 
        
        #endregion     
    }
    
    
    public class GetPermissionDetailResponse
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
        
    }
    
    
    public class GetPermissionDetailRequestValidator : MozValidator<GetPermissionDetailRequest>
    {
        public GetPermissionDetailRequestValidator(ILocalizationService localizationService)
        {
            
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("参数错误");
            
        }
    }
    
}
