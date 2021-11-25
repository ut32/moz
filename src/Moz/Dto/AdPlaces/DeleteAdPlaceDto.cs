using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Bus.Dtos.AdPlaces
{
    /// <summary>
    /// tab_ad_place
    /// </summary>
    [Validator(typeof(DeleteAdPlaceDtoValidator))]
    public class DeleteAdPlaceDto
    {
        public long Id {get;set;}     
    }

    public class DeleteAdPlaceDtoValidator : MozValidator<DeleteAdPlaceDto>
    {
        public DeleteAdPlaceDtoValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("参数错误");
        }
    }
    
}
