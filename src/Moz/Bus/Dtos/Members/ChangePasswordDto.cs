using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Bus.Dtos.Members
{
    public class ChangePasswordResponse
    {
        
    }

    [Validator(typeof(ChangePasswordRequestValidator))]
    public class ChangePasswordRequest
    { 
        public long MemberId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; } 
    }

    public class ChangePasswordRequestValidator : MozValidator<ChangePasswordRequest>
    {
        public ChangePasswordRequestValidator(ILocalizationService localizationService)
        {
            RuleFor(it => it.MemberId).GreaterThan(0).WithMessage("参数不正确");
            RuleFor(it => it.OldPassword).NotEmpty().WithMessage("旧密码不能为空");
            RuleFor(it => it.NewPassword).NotEmpty().WithMessage("新密码不能为空");
            RuleFor(it => it.NewPassword).MinimumLength(6).WithMessage("新密码至少为6位");
            RuleFor(it => it.ConfirmPassword).Equal(it=>it.NewPassword).WithMessage("两次输入密码不一致");
        }
    }
}