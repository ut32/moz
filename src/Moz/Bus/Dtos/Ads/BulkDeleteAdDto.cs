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
    [Validator(typeof(BulkDeleteAdsRequestValidator))]
    public class BulkDeleteAdsRequest
    {
        public long[] Ids {get;set;}   
    }
    
    
    public class BulkDeleteAdsResponse
    {
    
    }
    
    
    public class BulkDeleteAdsRequestValidator : MozValidator<BulkDeleteAdsRequest>
    {
        public BulkDeleteAdsRequestValidator(ILocalizationService localizationService)
        {
             RuleFor(x => x.Ids).Must(x=>x.Any()).WithMessage("至少选择一项");
        }
    }
    
}
