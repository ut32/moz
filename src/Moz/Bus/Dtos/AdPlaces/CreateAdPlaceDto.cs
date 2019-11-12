
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
    [Validator(typeof(CreateAdPlaceRequestValidator))]
    public class CreateAdPlaceRequest
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
    
    
    public class CreateAdPlaceResponse
    {
    
    }
    
    
    public class CreateAdPlaceRequestValidator : MozValidator<CreateAdPlaceRequest>
    {
        public CreateAdPlaceRequestValidator(ILocalizationService localizationService)
        {

            RuleFor(x => x.Title).NotEmpty().WithMessage("标题不能为空");


            //RuleFor(x => x.Code).NotEmpty().WithMessage("不能为空");


            //RuleFor(x => x.Desc).NotEmpty().WithMessage("不能为空");


            //RuleFor(x => x.Addtime).Must(t=>true).WithMessage("发生错误");

        }
    }
    
}
