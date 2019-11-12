using System.Collections.Generic;
using FluentValidation;
using Moz.Validation;

namespace Moz.Domain.Dtos.Members.Roles
{
    public class ConfigPermissionRequest
    {
        public long RoleId { get; set; } 
        public List<ConfigedPermissionItem> ConfigedPermissions { get; set; }
    }

    public class ConfigedPermissionItem
    {
        public long Id { get; set; }
    }

    public class ConfigPermissionResponse
    {
        
    }

    public class ConfigPermissionValidator : MozValidator<ConfigPermissionRequest>
    {
        public ConfigPermissionValidator()
        {
            RuleFor(t => t.RoleId).GreaterThan(0).WithMessage("参数错误");
        }
    }
}