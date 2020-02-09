using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Attributes;
using Moz.Validation;

namespace Moz.Bus.Dtos.Roles
{

    [Validator(typeof(ConfigMenuDtoValidator))]
    public class ConfigMenuDto
    {
        public long RoleId { get; set; } 
        public List<ConfigedMenuItem> ConfigedMenus { get; set; }
    }

    public class ConfigedMenuItem
    {
        public long Id { get; set; }
    }

    public class ConfigMenuDtoValidator : MozValidator<ConfigMenuDto>
    {
        public ConfigMenuDtoValidator() 
        {
            RuleFor(t => t.RoleId).GreaterThan(0).WithMessage("参数错误");
        }
    }
}