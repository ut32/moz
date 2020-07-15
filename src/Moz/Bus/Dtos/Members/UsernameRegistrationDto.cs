using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Models.Members;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Bus.Dtos.Members
{
    [Validator(typeof(UsernameRegistrationDtoValidator))]
    public class UsernameRegistrationDto
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        
        /// <summary>
        /// 确认密码
        /// </summary>
        public string ConfirmPassword { get; set; }
    }
    
    public class UsernameRegistrationDtoValidator : MozValidator<UsernameRegistrationDto>
    {
        public UsernameRegistrationDtoValidator(ILocalizationService localizationService)
        {
            RuleFor(it => it.Username).NotEmpty().WithMessage("用户名不能为空");
            RuleFor(it => it.Password).NotEmpty().WithMessage("密码不能为空");
            RuleFor(it => it.ConfirmPassword).Equal(it=>it.Password).WithMessage("两次输入密码不一致");
        }
    }
}