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
    [Validator(typeof(GetAdPlaceDetailRequestValidator))]
    public class GetAdPlaceDetailRequest
    {
        public long Id {get;set;}      
    }
    
    
    public class GetAdPlaceDetailResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string Title { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string Code { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string Desc { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime Addtime { get;set; } 
        
    }
    
    
    public class GetAdPlaceDetailRequestValidator : MozValidator<GetAdPlaceDetailRequest>
    {
        public GetAdPlaceDetailRequestValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("参数错误");
        }
    }
    
}
