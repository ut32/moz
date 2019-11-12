using System.Linq;
using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Bus.Dtos.Members
{
    
    public class ResetPasswordResponse
    {
        
    }

    [Validator(typeof(ResetPasswordRequestValidator))]
    public class ResetPasswordRequest
    { 
        public long[] MemberIds { get; set; }
        public string NewPassword { get; set; } 
    }

    public class ResetPasswordRequestValidator : MozValidator<ResetPasswordRequest>
    {
        public ResetPasswordRequestValidator(ILocalizationService localizationService)
        {
            RuleFor(it => it.MemberIds).Must(it=>it.Any()).WithMessage("至少选择一项");
            RuleFor(it => it.NewPassword).NotEmpty().WithMessage("默认密码不能为空");
            RuleFor(it => it.NewPassword).MinimumLength(6).WithMessage("默认密码至少为6位");
        }
    }
}