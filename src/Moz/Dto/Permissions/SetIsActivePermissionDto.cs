using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Bus.Dtos.Permissions
{
    /// <summary>
    /// tab_schedule_task
    /// </summary>
    [Validator(typeof(SetIsActivePermissionDtoValidator))]
    public class SetIsActivePermissionDto
    {
        #region 属性
        
        /// <summary>
        /// 
        /// </summary>
        public long Id { get;set; } 

        #endregion     
    } 


    public class SetIsActivePermissionDtoValidator : MozValidator<SetIsActivePermissionDto>
    {
        public SetIsActivePermissionDtoValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("发生错误，参与不能为0");
        }
    } 
    
}
