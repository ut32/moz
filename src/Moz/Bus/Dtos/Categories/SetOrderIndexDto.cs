using System;
using FluentValidation;
using FluentValidation.Attributes;
using Moz.Validation;

namespace Moz.Biz.Dtos.Categories
{
    [Validator(typeof(SetOrderIndexRequestValidator))]
    public class SetOrderIndexRequest
    {
        public long Id { get; set; }
        public string OrderIndex { get; set; }
    }
    
    public class SetOrderIndexResponse
    {
        
    }

    public class SetOrderIndexRequestValidator : MozValidator<SetOrderIndexRequest>
    {
        public SetOrderIndexRequestValidator()
        {
            RuleFor(t => t.Id).GreaterThan(0).WithMessage("参数错误");
            RuleFor(t => t.OrderIndex).Must(t => t.IsNumbers()).WithMessage("排序数字不正确");
        }
    }
}