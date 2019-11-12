using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Domain.Dtos.Members.Roles
{
    
    [Validator(typeof(GetRoleDetailRequestValidator))]
    public class GetRoleDetailRequest
    {
        #region 属性
        
        /// <summary>
        /// 
        /// </summary>
        public long Id { get;set; } 
        
        #endregion     
    }
    
    
    public class GetRoleDetailResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string Name { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public bool IsActive { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public bool IsAdmin { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string Code { get;set; } 
        
    }
    
    
    public class GetRoleDetailRequestValidator : MozValidator<GetRoleDetailRequest>
    {
        public GetRoleDetailRequestValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("参数错误");
        }
    }
    
}
