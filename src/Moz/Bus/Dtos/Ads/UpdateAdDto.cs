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
    [Validator(typeof(UpdateAdRequestValidator))]
    public class UpdateAdRequest
    {
        #region 属性
        
        /// <summary>
        /// 
        /// </summary>
        public long Id { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        //public long AdPlaceId { get;set; } 
        
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
    
    
    public class UpdateAdResponse
    {
    
    }
    
    
    public class UpdateAdRequestValidator : MozValidator<UpdateAdRequest>
    {
        public UpdateAdRequestValidator(ILocalizationService localizationService)
        {

            RuleFor(x => x.Id).GreaterThan(0).WithMessage("发生错误");
            //RuleFor(x => x.AdPlaceId).GreaterThan(0).WithMessage("发生错误");
            RuleFor(x => x.Title).NotEmpty().WithMessage("标题不能为空");
            RuleFor(x => x.ImagePath).NotEmpty().WithMessage("图片不能为空");
        }
    }
    
}
