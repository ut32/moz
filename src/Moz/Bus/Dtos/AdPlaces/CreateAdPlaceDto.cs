using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Bus.Dtos.AdPlaces
{
    /// <summary>
    /// tab_ad_place
    /// </summary>
    [Validator(typeof(CreateAdPlaceDtoValidator))]
    public class CreateAdPlaceDto
    {
        #region 属性
        
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

        #endregion     
    }
    
    
    public class CreateAdPlaceDtoValidator : MozValidator<CreateAdPlaceDto>
    {
        public CreateAdPlaceDtoValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("标题不能为空");
            //RuleFor(x => x.Code).NotEmpty().WithMessage("不能为空");
            //RuleFor(x => x.Desc).NotEmpty().WithMessage("不能为空");
        }
    }
    
}
