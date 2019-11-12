using FluentValidation;
using FluentValidation.Attributes;
using Moz.Validation;

namespace Moz.Domain.Dtos.Members.Permissions
{
    [Validator(typeof(SetPermissionOrderIndexValidator))]
    public class SetPermissionOrderIndexRequest
    {
        public long Id { get; set; }
        public int OrderIndex { get; set; } 
    }

    public class SetPermissionOrderIndexResponse
    {
         
    }

    public class SetPermissionOrderIndexValidator : MozValidator<SetPermissionOrderIndexRequest>
    {
        public SetPermissionOrderIndexValidator()
        {
            RuleFor(t => t.Id).GreaterThan(0).WithMessage("参数错误");
        }
    }
}