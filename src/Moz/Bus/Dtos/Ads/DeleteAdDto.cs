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
    [Validator(typeof(DeleteAdRequestValidator))]
    public class DeleteAdRequest
    {
        public long Id {get;set;}     
    }
    
    
    public class DeleteAdResponse
    {
    
    }
    
    
    public class DeleteAdRequestValidator : MozValidator<DeleteAdRequest>
    {
        public DeleteAdRequestValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("参数错误");
        }
    }
    
}
