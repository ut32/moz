using System;
using System.Linq;
using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Domain.Dtos.ServicePerformances
{
    /// <summary>
    /// tab_service_performance
    /// </summary>
    [Validator(typeof(CreateServicePerformanceRequestValidator))]
    public class CreateServicePerformanceRequest
    {
        #region 属性
        
        /// <summary>
        /// 
        /// </summary>
        public string Name { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public int ElapsedMs { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string HttpRequestId { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime AddTime { get;set; } 
        
        #endregion     
    }
    
    
    public class CreateServicePerformanceResponse
    {
    
    }
    
    
    public class CreateServicePerformanceRequestValidator : MozValidator<CreateServicePerformanceRequest>
    {
        public CreateServicePerformanceRequestValidator(ILocalizationService localizationService)
        {
            
             RuleFor(x => x.Name).NotEmpty().WithMessage("不能为空");
            
            
             RuleFor(x => x.ElapsedMs).GreaterThan(0).WithMessage("不能为空");
            
            
             RuleFor(x => x.HttpRequestId).NotEmpty().WithMessage("不能为空");
            
            
            RuleFor(x => x.AddTime).Must(t=>true).WithMessage("发生错误");
            
        }
    }
    
}
