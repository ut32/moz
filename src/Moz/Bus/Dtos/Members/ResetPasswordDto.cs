using System.Linq;
using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Bus.Dtos.Members
{

    [Validator(typeof(ResetPasswordDtoValidator))]
    public class ResetPasswordDto
    { 
        public long[] MemberIds { get; set; }
        public string NewPassword { get; set; } 
    }

    public class ResetPasswordDtoValidator : MozValidator<ResetPasswordDto>
    {
        public ResetPasswordDtoValidator(ILocalizationService localizationService)
        {
            RuleFor(it => it.MemberIds).Must(it=>it.Any()).WithMessage("至少选择一项");
            RuleFor(it => it.NewPassword).NotEmpty().WithMessage("默认密码不能为空");
            RuleFor(it => it.NewPassword).MinimumLength(6).WithMessage("默认密码至少为6位");
        }
    }
}