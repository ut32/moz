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
    [Validator(typeof(SetAdIsShowRequestValidator))]
    public class SetAdIsShowRequest
    {
        #region 属性
        
        public long Id { get; set; }
        public bool IsShow { get; set; } 
        
        #endregion     
    }
    
    
    public class SetAdIsShowResponse
    {
    
    }
    
    
    public class SetAdIsShowRequestValidator : MozValidator<SetAdIsShowRequest>
    {
        public SetAdIsShowRequestValidator(ILocalizationService localizationService)
        {
            RuleFor(t => t.Id).GreaterThan(0).WithMessage("参数错误");
            //RuleFor(t => t.OrderIndex).Must(t => t.IsNumbers()).WithMessage("排序数字不正确");
        }
    }
    
}
