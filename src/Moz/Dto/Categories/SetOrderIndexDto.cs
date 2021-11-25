using System;
using FluentValidation;
using FluentValidation.Attributes;
using Moz.Validation;

namespace Moz.Bus.Dtos.Categories
{
    [Validator(typeof(SetOrderIndexDtoValidator))]
    public class SetOrderIndexDto
    {
        public long Id { get; set; }
        public int OrderIndex { get; set; }
    }

    public class SetOrderIndexDtoValidator : MozValidator<SetOrderIndexDto>
    {
        public SetOrderIndexDtoValidator()
        {
            RuleFor(t => t.Id).GreaterThan(0).WithMessage("参数错误");
        }
    }
}