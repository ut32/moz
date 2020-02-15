using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Bus.Dtos.AdPlaces
{
    /// <summary>
    /// tab_ad_place
    /// </summary>
    [Validator(typeof(UpdateAdPlaceDtoValidator))]
    public class UpdateAdPlaceDto
    {
        #region 属性
        
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
        
        #endregion     
    }

    public class UpdateAdPlaceDtoValidator : MozValidator<UpdateAdPlaceDto>
    {
        public UpdateAdPlaceDtoValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Id).Must(t => true).WithMessage("发生错误");
            RuleFor(x => x.Title).NotEmpty().WithMessage("标题不能为空");

        }
    }
    
}
