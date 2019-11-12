
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
    [Validator(typeof(CreateAdRequestValidator))]
    public class CreateAdRequest
    {
        #region 属性
        
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
        
        #endregion     
    }
    
    
    public class CreateAdResponse
    {
    
    }
    
    
    public class CreateAdRequestValidator : MozValidator<CreateAdRequest>
    {
        public CreateAdRequestValidator(ILocalizationService localizationService)
        {

            RuleFor(x => x.AdPlaceId).GreaterThan(0).WithMessage("AdPlaceId错误");
            RuleFor(x => x.Title).NotEmpty().WithMessage("标题不能为空");
            RuleFor(x => x.ImagePath).NotEmpty().WithMessage("图片不能为空");
            //RuleFor(x => x.TargetUrl).NotEmpty().WithMessage("不能为空");
            //RuleFor(x => x.Order).GreaterThan(0).WithMessage("不能为空");
            //RuleFor(x => x.IsShow).Must(t => true).WithMessage("发生错误");

        }
    }
    
}
