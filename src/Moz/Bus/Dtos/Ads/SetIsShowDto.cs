using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Bus.Dtos.Ads
{
    /// <summary>
    /// tab_ad
    /// </summary>
    [Validator(typeof(SetAdIsShowDtoValidator))]
    public class SetAdIsShowDto
    {
        #region 属性
        
        public long Id { get; set; }
        
        public bool IsShow { get; set; } 
        
        #endregion     
    }
    
    
    public class SetAdIsShowDtoValidator : MozValidator<SetAdIsShowDto>
    {
        public SetAdIsShowDtoValidator(ILocalizationService localizationService)
        {
            RuleFor(t => t.Id).GreaterThan(0).WithMessage("参数错误");
        }
    }
    
}
