using System;
using System.Linq;
using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Biz.Dtos.Ads
{
    /// <summary>
    /// tab_ad
    /// </summary>
    [Validator(typeof(SetAdOrderRequestValidator))]
    public class SetAdOrderRequest
    {
        #region 属性
        
        public long Id { get; set; }
        public int OrderIndex { get; set; }
        
        #endregion     
    }
    
    
    public class SetAdOrderResponse
    {
    
    }
    
    
    public class SetAdOrderRequestValidator : MozValidator<SetAdOrderRequest>
    {
        public SetAdOrderRequestValidator(ILocalizationService localizationService)
        {
            RuleFor(t => t.Id).GreaterThan(0).WithMessage("参数错误");
            //RuleFor(t => t.OrderIndex).Must(t => t.IsNumbers()).WithMessage("排序数字不正确");
        }
    }
    
}
