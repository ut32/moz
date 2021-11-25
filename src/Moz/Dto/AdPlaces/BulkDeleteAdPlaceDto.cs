using System.Linq;
using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Bus.Dtos.AdPlaces
{
    /// <summary>
    /// tab_ad_place
    /// </summary>
    [Validator(typeof(BulkDeleteAdPlacesDtoValidator))]
    public class BulkDeleteAdPlacesDto
    {
        public long[] Ids {get;set;}   
    }


    public class BulkDeleteAdPlacesDtoValidator : MozValidator<BulkDeleteAdPlacesDto>
    {
        public BulkDeleteAdPlacesDtoValidator(ILocalizationService localizationService)
        {
             RuleFor(x => x.Ids).Must(x=>x.Any()).WithMessage("至少选择一项");
        }
    }
    
}
