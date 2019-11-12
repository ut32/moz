using FluentValidation;
using FluentValidation.Attributes;
using Moz.Validation;

namespace Moz.Domain.Dtos.Members.Permissions
{
    [Validator(typeof(SetPermissionIsActiveValidator))]
    public class SetPermissionIsActiveRequest
    {
        public long Id { get; set; }
        public bool IsActive { get; set; }
    }

    public class SetPermissionIsActiveResponse
    {
        
    }

    public class SetPermissionIsActiveValidator : MozValidator<SetPermissionIsActiveRequest>
    {
        public SetPermissionIsActiveValidator()
        {
            RuleFor(t => t.Id).GreaterThan(0).WithMessage("参数错误");
        }
    }
}