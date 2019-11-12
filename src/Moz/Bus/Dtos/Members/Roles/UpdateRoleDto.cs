using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Domain.Dtos.Members.Roles
{
    /// <summary>
    /// tab_role
    /// </summary>
    [Validator(typeof(UpdateRoleRequestValidator))]
    public class UpdateRoleRequest
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
        public bool IsAdmin { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string Code { get;set; } 
        
        #endregion     
    }
    
    
    public class UpdateRoleResponse
    {
    
    }
    
    
    public class UpdateRoleRequestValidator : MozValidator<UpdateRoleRequest>
    {
        public UpdateRoleRequestValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("参数错误");
            RuleFor(x => x.Name).NotEmpty().WithMessage("角色名称不能为空");
            RuleFor(x => x.Code).NotEmpty().WithMessage("角色代码不能为空");
        }
    }
    
}
