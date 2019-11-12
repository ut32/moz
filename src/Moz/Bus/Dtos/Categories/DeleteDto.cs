using FluentValidation;
using FluentValidation.Attributes;
using Moz.Validation;

namespace Moz.Biz.Dtos.Categories
{
    [Validator(typeof(DeleteRequestValidator))]
    public class DeleteRequest
    {
        public long Id { get; set; }
    }
    
    public class DeleteResponse
    {
        
    }

    public class DeleteRequestValidator : MozValidator<DeleteRequest>
    {
        public DeleteRequestValidator()
        {
            RuleFor(t => t.Id).GreaterThan(0).WithMessage("参数错误");
        }
    }
}