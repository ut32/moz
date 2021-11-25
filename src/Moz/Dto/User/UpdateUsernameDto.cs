using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Bus.Dtos.Members
{
    /// <summary>
    /// 修改头像
    /// </summary>
    [Validator(typeof(UpdateUsernameDtoValidator))]
    public class UpdateUsernameDto
    {
        public long MemberId { get; set; } 
        public string Username { get; set; } 
    }
    
    /// <summary>
    /// 
    /// </summary>
    public class UpdateUsernameDtoValidator : MozValidator<UpdateUsernameDto>
    {
        public UpdateUsernameDtoValidator(ILocalizationService localizationService)
        {
            //RuleFor(it => it.MemberId).GreaterThan(0).WithMessage("参数错误");
            RuleFor(it => it.Username).NotEmpty().WithMessage("用户名不能为空");
            RuleFor(it => it.Username).MinimumLength(6).WithMessage("用户名至少6个字符");
        }
    }
}