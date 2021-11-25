using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Attributes;
using Moz.Validation;

namespace Moz.Bus.Dtos.Roles
{

    [Validator(typeof(ConfigPermissionDtoValidator))]
    public class ConfigPermissionDto
    {
        public long RoleId { get; set; } 
        public List<ConfigedPermissionItem> ConfigedPermissions { get; set; }
    }

    public class ConfigedPermissionItem
    {
        public long Id { get; set; }
    }

    public class ConfigPermissionDtoValidator : MozValidator<ConfigPermissionDto>
    {
        public ConfigPermissionDtoValidator()
        {
            RuleFor(t => t.RoleId).GreaterThan(0).WithMessage("参数错误");
        }
    }
}