using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Models.Members;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Bus.Dtos.Roles
{
    [Validator(typeof(GetPermissionsByRoleDtoValidator))]
    public class GetPermissionsByRoleDto
    {
        public long RoleId { get; set; }
    }

    public class GetPermissionsByRoleApo
    {
        public List<Permission> Permissions { get; set; }
    } 
    
    public class GetPermissionsByRoleDtoValidator: MozValidator<GetPermissionsByRoleDto>
    {
        public GetPermissionsByRoleDtoValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.RoleId).GreaterThan(0).WithMessage("参数错误");
        }
    }
}