using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Bus.Dtos.Permissions
{
    /// <summary>
    /// tab_permission
    /// </summary>
    [Validator(typeof(GetPermissionDetailDtoValidator))]
    public class GetPermissionDetailDto
    {
        public long Id {get;set;}      
    }
    
    
    public class PermissionDetailApo
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
        public string Code { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public bool IsActive { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public long? ParentId { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public int OrderIndex { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public bool IsSystem { get;set; } 
        
    }
    
    
    public class GetPermissionDetailDtoValidator : MozValidator<GetPermissionDetailDto>
    {
        public GetPermissionDetailDtoValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("参数错误");
        }
    }
    
}
