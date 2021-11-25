using FluentValidation;
using FluentValidation.Attributes;
using Moz.Validation;

namespace Moz.Bus.Dtos.AdminMenus
{
    [Validator(typeof(UpdateAdminMenuDtoValidator))]
    public class UpdateAdminMenuDto:CreateAdminMenuDto
    {
        public long Id { get; set; }
    }

    public class UpdateAdminMenuDtoValidator : MozValidator<UpdateAdminMenuDto>
    {
        public UpdateAdminMenuDtoValidator()
        {
            RuleFor(t => t.Id).GreaterThan(0).WithMessage("参数不正确");
            RuleFor(t => t.Name).NotNull().NotEmpty().WithMessage("名称不能为空");
            RuleFor(t => t.Link).NotNull().NotEmpty().WithMessage("链接不能为空");
        }
    }
}