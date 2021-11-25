using System.Linq;
using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Bus.Dtos.Ads
{
    /// <summary>
    /// tab_ad
    /// </summary>
    [Validator(typeof(BulkDeleteAdsDtoValidator))]
    public class BulkDeleteAdsDto
    {
        public long[] Ids {get;set;}   
    }

    public class BulkDeleteAdsDtoValidator : MozValidator<BulkDeleteAdsDto>
    {
        public BulkDeleteAdsDtoValidator(ILocalizationService localizationService)
        {
             RuleFor(x => x.Ids).Must(x=>x.Any()).WithMessage("至少选择一项");
        }
    }
    
}
