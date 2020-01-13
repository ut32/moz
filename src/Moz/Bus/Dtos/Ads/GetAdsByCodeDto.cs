using System;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Bus.Dtos.Ads
{
    /// <summary>
    /// tab_ad
    /// </summary>
    [Validator(typeof(GetAdsByCodeRequestValidator))]
    public class GetAdsByCodeRequest
    {
        public string Code {get;set;}
    }
    
    
    public class GetAdsByCodeResponse
    {
        public List<GetAdsByCodeItem> Ads { get; set; }
    }

    public class  GetAdsByCodeItem
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string ImagePath { get; set; }

        public string FullImagePath => ImagePath.GetFullPath();
        public string TargetUrl { get; set; } 
    }
    
    
    public class GetAdsByCodeRequestValidator : MozValidator<GetAdsByCodeRequest>
    {
        public GetAdsByCodeRequestValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Code).NotEmpty().WithMessage("标识码不能为空");
        } 
    }
    
}
