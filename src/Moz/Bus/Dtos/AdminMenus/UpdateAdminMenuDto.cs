using FluentValidation;
using FluentValidation.Attributes;
using Moz.Validation;

namespace Moz.Domain.Dtos.AdminMenus
{
    [Validator(typeof(UpdateAdminMenuRequestValidator))]
    public class UpdateAdminMenuRequest:CreateAdminMenuRequest
    {
        public long Id { get; set; }
    }
    
    public class UpdateAdminMenuResponse
    {
        
    }

    public class UpdateAdminMenuRequestValidator : MozValidator<UpdateAdminMenuRequest>
    {
        public UpdateAdminMenuRequestValidator()
        {
            RuleFor(t => t.Id).GreaterThan(0).WithMessage("参数不正确");
            RuleFor(t => t.Name).NotNull().NotEmpty().WithMessage("名称不能为空");
            RuleFor(t => t.Link).NotNull().NotEmpty().WithMessage("链接不能为空");
        }
    }
}