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
    [Validator(typeof(BulkDeleteAdPlacesRequestValidator))]
    public class BulkDeleteAdPlacesRequest
    {
        public long[] Ids {get;set;}   
    }
    
    
    public class BulkDeleteAdPlacesResponse
    {
    
    }
    
    
    public class BulkDeleteAdPlacesRequestValidator : MozValidator<BulkDeleteAdPlacesRequest>
    {
        public BulkDeleteAdPlacesRequestValidator(ILocalizationService localizationService)
        {
             RuleFor(x => x.Ids).Must(x=>x.Any()).WithMessage("至少选择一项");
        }
    }
    
}
