using System;
using FluentValidation;
using FluentValidation.Attributes;
using Moz.Validation;

namespace Moz.Bus.Dtos.AdminMenus
{
    [Validator(typeof(SetAdminMenuOrderIndexDtoValidator))]
    public class SetAdminMenuOrderIndexDto
    {
        public long Id { get; set; }
        public string OrderIndex { get; set; }
    }

    public class SetAdminMenuOrderIndexDtoValidator : MozValidator<SetAdminMenuOrderIndexDto>
    {
        public SetAdminMenuOrderIndexDtoValidator()
        {
            RuleFor(t => t.Id).GreaterThan(0).WithMessage("参数错误");
            RuleFor(t => t.OrderIndex).Must(t => t.IsNumbers()).WithMessage("排序数字不正确");
        }
    }
}