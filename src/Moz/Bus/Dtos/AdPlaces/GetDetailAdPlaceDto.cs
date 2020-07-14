using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Bus.Dtos.AdPlaces
{
    /// <summary>
    /// tab_ad_place
    /// </summary>
    [Validator(typeof(GetAdPlaceDetailDtoValidator))]
    public class GetAdPlaceDetailDto
    {
        public long Id {get;set;}      
    }
    
    
    public class GetAdPlaceDetailInfo
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
        public System.DateTime AddTime { get;set; } 
        
    }
    
    
    public class GetAdPlaceDetailDtoValidator : MozValidator<GetAdPlaceDetailDto>
    {
        public GetAdPlaceDetailDtoValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("参数错误");
        }
    }
    
}
