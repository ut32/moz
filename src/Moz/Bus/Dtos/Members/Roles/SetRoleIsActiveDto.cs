using FluentValidation;
using FluentValidation.Attributes;
using Moz.Validation;

namespace Moz.Domain.Dtos.Members.Roles
{
    [Validator(typeof(SetRoleIsActiveValidator))]
    public class SetRoleIsActiveRequest
    {
        public long Id { get; set; }
        public bool IsActive { get; set; }
    }

    public class SetRoleIsActiveResponse
    {
        
    }

    public class SetRoleIsActiveValidator : MozValidator<SetRoleIsActiveRequest>
    {
        public SetRoleIsActiveValidator()
        {
            RuleFor(t => t.Id).GreaterThan(0).WithMessage("参数错误");
        }
    }
}