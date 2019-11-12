using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Domain.Dtos.Members.Roles
{
    /// <summary>
    /// tab_role
    /// </summary>
    [Validator(typeof(CreateRoleRequestValidator))]
    public class CreateRoleRequest
    {
        #region 属性
        
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
        
        #endregion     
    }
    
    
    public class CreateRoleResponse
    {
    
    }
    
    
    public class CreateRoleRequestValidator : MozValidator<CreateRoleRequest>
    {
        public CreateRoleRequestValidator(ILocalizationService localizationService)
        {

            RuleFor(x => x.Name).NotEmpty().WithMessage("角色名称不能为空");
            RuleFor(x => x.Code).NotEmpty().WithMessage("标识码不能为空");
        }
    }
    
}
