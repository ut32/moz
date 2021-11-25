using FluentValidation;
using FluentValidation.Attributes;
using Moz.Validation;

namespace Moz.Bus.Dtos.Permissions
{
    [Validator(typeof(SetOrderIndexPermissionDtoValidator))]
    public class SetOrderIndexPermissionDto
    {
        public long Id { get; set; }
        public int OrderIndex { get; set; }
    }

    public class SetOrderIndexPermissionDtoValidator : MozValidator<SetOrderIndexPermissionDto>
    {
        public SetOrderIndexPermissionDtoValidator()
        {
            RuleFor(t => t.Id).GreaterThan(0).WithMessage("参数错误");
        }
    }
}