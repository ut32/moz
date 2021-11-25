using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Model;
using Moz.Validation;

namespace Moz.Dto.User
{
    /// <summary>
    /// tab_member
    /// </summary>
    [Validator(typeof(CreateUserDtoValidator))]
    public class CreateUserDto
    {
        #region 属性

        /// <summary>
        /// 
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Gender? Gender { get; set; }


        #endregion
    }

    public class CreateUserInfo
    {
        public long Id { get; set; }
        public string Uuid { get; set; }
        public string Username { get; set; }
    }

    public class CreateUserDtoValidator : MozValidator<CreateUserDto>
    {
        public CreateUserDtoValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("用户名不能为空");
            RuleFor(x => x.Password).NotEmpty().WithMessage("密码不能为空");
            RuleFor(x => x.Password).MinimumLength(6).WithMessage("密码不能小于6位");
        }
    }
}
