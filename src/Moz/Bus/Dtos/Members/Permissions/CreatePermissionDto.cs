using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Domain.Dtos.Members.Permissions
{
    /// <summary>
    /// tab_permission
    /// </summary>
    [Validator(typeof(CreatePermissionRequestValidator))]
    public class CreatePermissionRequest
    {
        #region 属性
        
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
        
        #endregion     
    }
    
    
    public class CreatePermissionResponse
    {
    
    }
    
    
    public class CreatePermissionRequestValidator : MozValidator<CreatePermissionRequest>
    {
        public CreatePermissionRequestValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("名称不能为空");
            RuleFor(x => x.Code).NotEmpty().WithMessage("标识不能为空");
        }
    }
    
}
