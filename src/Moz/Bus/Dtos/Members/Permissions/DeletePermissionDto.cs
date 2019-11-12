using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Domain.Dtos.Members.Permissions
{
    /// <summary>
    /// tab_permission
    /// </summary>
    [Validator(typeof(DeletePermissionRequestValidator))]
    public class DeletePermissionRequest
    {
        #region 属性
        
        /// <summary>
        /// 
        /// </summary>
        public long Id { get;set; } 
        
        #endregion     
    }
    
    
    public class DeletePermissionResponse
    {
    
    }
    
    
    public class DeletePermissionRequestValidator : MozValidator<DeletePermissionRequest>
    {
        public DeletePermissionRequestValidator(ILocalizationService localizationService)
        {
            
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("参数错误");
            
        }
    }
    
}
