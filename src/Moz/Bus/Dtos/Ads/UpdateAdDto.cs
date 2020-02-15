using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Bus.Dtos.Ads
{
    /// <summary>
    /// tab_ad
    /// </summary>
    [Validator(typeof(UpdateAdDtoValidator))]
    public class UpdateAdDto
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
        public string ImagePath { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string TargetUrl { get;set; } 
        
        
        #endregion     
    }
    
    
    public class UpdateAdDtoValidator : MozValidator<UpdateAdDto>
    {
        public UpdateAdDtoValidator(ILocalizationService localizationService)
        {

            RuleFor(x => x.Id).GreaterThan(0).WithMessage("发生错误");
            RuleFor(x => x.Title).NotEmpty().WithMessage("标题不能为空");
            RuleFor(x => x.ImagePath).NotEmpty().WithMessage("图片不能为空");
        }
    }
    
}
