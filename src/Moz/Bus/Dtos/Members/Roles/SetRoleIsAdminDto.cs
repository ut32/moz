using FluentValidation;
using FluentValidation.Attributes;
using Moz.Validation;

namespace Moz.Domain.Dtos.Members.Roles
{
    [Validator(typeof(SetRoleIsAdminValidator))]
    public class SetRoleIsAdminRequest
    {
        public long Id { get; set; }
        public bool IsAdmin { get; set; } 
    }

    public class SetRoleIsAdminResponse
    {
        
    }

    public class SetRoleIsAdminValidator : MozValidator<SetRoleIsAdminRequest>
    {
        public SetRoleIsAdminValidator()
        {
            RuleFor(t => t.Id).GreaterThan(0).WithMessage("参数错误");
        }
    }
}