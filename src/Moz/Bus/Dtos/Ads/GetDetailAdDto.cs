using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Bus.Dtos.Ads
{
    /// <summary>
    /// tab_ad
    /// </summary>
    [Validator(typeof(GetAdDetailDtoValidator))]
    public class GetAdDetailDto
    {
        public long Id {get;set;}      
    }
    
    
    public class GetAdDetailApo
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public long AdPlaceId { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string Title { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string ImagePath { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string TargetUrl { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public int Order { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public bool IsShow { get;set; } 
        
    }
    
    
    public class GetAdDetailDtoValidator : MozValidator<GetAdDetailDto>
    {
        public GetAdDetailDtoValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("参数错误");
        }
    }
    
}
