using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Domain.Dtos.Members.Roles
{
    
    [Validator(typeof(DeleteRoleRequestValidator))]
    public class DeleteRoleRequest
    {
        #region 属性
        
        /// <summary>
        /// 
        /// </summary>
        public long Id { get;set; } 
        
        #endregion     
    }
    
    
    public class DeleteRoleResponse
    {
    
    }
    
    
    public class DeleteRoleRequestValidator : MozValidator<DeleteRoleRequest>
    {
        public DeleteRoleRequestValidator(ILocalizationService localizationService)
        {
            
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("参数错误");
            
        }
    }
    
}
