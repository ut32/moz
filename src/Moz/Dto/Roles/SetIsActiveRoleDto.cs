using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Bus.Dtos.Roles
{
    /// <summary>
    /// tab_schedule_task
    /// </summary>
    [Validator(typeof(SetIsActiveRoleDtoValidator))]
    public class SetIsActiveRoleDto
    {
        #region 属性
        
        /// <summary>
        /// 
        /// </summary>
        public long Id { get;set; } 

        #endregion     
    } 


    public class SetIsActiveRoleDtoValidator : MozValidator<SetIsActiveRoleDto>
    {
        public SetIsActiveRoleDtoValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("发生错误，参与不能为0");
        }
    } 
    
}
