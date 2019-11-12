using System;
using System.Linq;
using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Biz.Dtos.AdPlaces
{
    /// <summary>
    /// tab_ad_place
    /// </summary>
    [Validator(typeof(DeleteAdPlaceRequestValidator))]
    public class DeleteAdPlaceRequest
    {
        public long Id {get;set;}     
    }
    
    
    public class DeleteAdPlaceResponse
    {
    
    }
    
    
    public class DeleteAdPlaceRequestValidator : MozValidator<DeleteAdPlaceRequest>
    {
        public DeleteAdPlaceRequestValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("参数错误");
        }
    }
    
}
