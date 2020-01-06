using System.Collections.Generic;
using FluentValidation;
using Moz.Bus.Models.AdminMenus;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Bus.Dtos.AdminMenus
{
    public class GetMenusByRoleRequest
    {
        public long RoleId { get; set; }
    }

    public class GetMenusByRoleResponse
    {
        public List<AdminMenu> Menus { get; set; }
    }
    
    public class GetMenusByRoleValidator: MozValidator<GetMenusByRoleRequest>
    {
        public GetMenusByRoleValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.RoleId).GreaterThan(0).WithMessage("参数错误");
        }
    }
}