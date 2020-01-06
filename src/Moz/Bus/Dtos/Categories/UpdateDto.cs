using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Dtos.AdminMenus;
using Moz.Validation;

namespace Moz.Biz.Dtos.Categories
{
    [Validator(typeof(UpdateRequestValidator))]
    public class UpdateRequest:CreateAdminMenuRequest
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Desciption { get; set; }
    }
    
    public class UpdateResponse
    {
        
    }

    public class UpdateRequestValidator : MozValidator<UpdateRequest>
    {
        public UpdateRequestValidator()
        {
            RuleFor(t => t.Id).GreaterThan(0).WithMessage("参数不正确");
            RuleFor(t => t.Name).NotNull().NotEmpty().WithMessage("名称不能为空");
        }
    }
}