using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Bus.Dtos.Ads
{
    /// <summary>
    /// tab_ad
    /// </summary>
    [Validator(typeof(SetAdOrderDtoValidator))]
    public class SetAdOrderDto
    {
        #region 属性
        
        public long Id { get; set; }
        public int OrderIndex { get; set; }
        
        #endregion     
    }


    public class SetAdOrderDtoValidator : MozValidator<SetAdOrderDto>
    {
        public SetAdOrderDtoValidator(ILocalizationService localizationService)
        {
            RuleFor(t => t.Id).GreaterThan(0).WithMessage("参数错误");
        }
    }
    
}
