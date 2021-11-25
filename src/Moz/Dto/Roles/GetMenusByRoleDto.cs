using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Models.AdminMenus;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Bus.Dtos.Roles
{
    [Validator(typeof(GetMenusByRoleDtoValidator))]
    public class GetMenusByRoleDto
    {
        public long RoleId { get; set; }
    }

    public class GetMenusByRoleApo
    { 
        public List<AdminMenu> Menus { get; set; }
    }
    
    public class GetMenusByRoleDtoValidator: MozValidator<GetMenusByRoleDto>
    {
        public GetMenusByRoleDtoValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.RoleId).GreaterThan(0).WithMessage("参数错误");
        }
    } 
}