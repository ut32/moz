using FluentValidation;
using FluentValidation.Attributes;
using Moz.Validation;

namespace Moz.Biz.Dtos.Categories
{
    [Validator(typeof(GetDetailByIdRequestValidator))]
    public class GetDetailByIdRequest
    {
        public long Id { get; set; }
    }
    
    public class GetDetailByIdResponse
    {
        public long Id { get; set; }
        public long? ParentId { get; set; }
        public string Name { get; set; }
        
        public string Alias { get; set; }
        
        public string Description { get; set; }
    }

    public class GetDetailByIdRequestValidator : MozValidator<GetDetailByIdRequest>
    {
        public GetDetailByIdRequestValidator()
        {
            RuleFor(t => t.Id).GreaterThan(0).WithMessage("参数不正确");
        }
    }
}