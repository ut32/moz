using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Bus.Dtos.Roles
{
    /// <summary>
    /// tab_role
    /// </summary>
    [Validator(typeof(UpdateRoleDtoValidator))]
    public class UpdateRoleDto
    {
        #region 属性
        
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
        
        
        #endregion     
    }
    
    
    public class UpdateRoleDtoValidator : MozValidator<UpdateRoleDto>
    {
        public UpdateRoleDtoValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("参数错误");
            RuleFor(x => x.Name).NotEmpty().WithMessage("名称不能为空");
            RuleFor(x => x.Code).NotEmpty().WithMessage("标识不能为空");

        }
    }
    
}
