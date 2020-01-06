using System.Collections.Generic;
using FluentValidation;
using Moz.Bus.Models.Members;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Domain.Dtos.Members.Roles
{
    public class GetPermissionsByRoleRequest
    {
        public long RoleId { get; set; }
    }

    public class GetPermissionsByRoleResponse
    {
        public List<Permission> Permissions { get; set; }
    }
    
    public class GetPermissionsByRoleValidator: MozValidator<GetPermissionsByRoleRequest>
    {
        public GetPermissionsByRoleValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.RoleId).GreaterThan(0).WithMessage("参数错误");
        }
    }
}