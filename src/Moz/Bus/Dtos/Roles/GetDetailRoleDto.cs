using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Bus.Dtos.Roles
{
    /// <summary>
    /// tab_role
    /// </summary>
    [Validator(typeof(GetRoleDetailDtoValidator))]
    public class GetRoleDetailDto
    {
        public long Id {get;set;}      
    }
    
    
    public class RoleDetailApo
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
        public string Code { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public bool IsAdmin { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public bool IsSystem { get;set; } 
        
    }
    
    
    public class GetRoleDetailDtoValidator : MozValidator<GetRoleDetailDto>
    {
        public GetRoleDetailDtoValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("参数错误");
        }
    }
    
}
