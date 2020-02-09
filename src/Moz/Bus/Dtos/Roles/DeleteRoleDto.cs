using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Bus.Dtos.Roles
{
    /// <summary>
    /// tab_role
    /// </summary>
    [Validator(typeof(DeleteRoleDtoValidator))]
    public class DeleteRoleDto
    {
        public long Id {get;set;}     
    }
    
    public class DeleteRoleDtoValidator : MozValidator<DeleteRoleDto>
    {
        public DeleteRoleDtoValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("参数错误");
        }
    }
    
}
