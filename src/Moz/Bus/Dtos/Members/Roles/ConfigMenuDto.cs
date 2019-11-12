using System.Collections.Generic;
using FluentValidation;
using Moz.Validation;

namespace Moz.Domain.Dtos.Members.Roles
{
    public class ConfigMenuRequest
    {
        public long RoleId { get; set; } 
        public List<ConfigedMenuItem> ConfigedMenus { get; set; }
    }

    public class ConfigedMenuItem
    {
        public long Id { get; set; }
    }

    public class ConfigMenuResponse
    {
        
    }

    public class ConfigMenuValidator : MozValidator<ConfigMenuRequest>
    {
        public ConfigMenuValidator()
        {
            RuleFor(t => t.RoleId).GreaterThan(0).WithMessage("参数错误");
        }
    }
}