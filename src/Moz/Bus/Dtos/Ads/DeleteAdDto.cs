using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Bus.Dtos.Ads
{
    /// <summary>
    /// tab_ad
    /// </summary>
    [Validator(typeof(DeleteAdDtoValidator))]
    public class DeleteAdDto
    {
        public long Id {get;set;}     
    }


    public class DeleteAdDtoValidator : MozValidator<DeleteAdDto>
    {
        public DeleteAdDtoValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("参数错误");
        }
    }
    
}
