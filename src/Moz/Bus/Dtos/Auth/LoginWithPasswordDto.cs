// ReSharper disable ClassNeverInstantiated.Global

using FluentValidation;
using FluentValidation.Attributes;
using Moz.Validation;

namespace Moz.Bus.Dtos.Auth
{
    #region 密码登录 

    #region 密码登录 Request

    [Validator(typeof(LoginWithPasswordValidator))]
    public class LoginWithPasswordRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginWithPasswordValidator : MozValidator<LoginWithPasswordRequest>
    {
        public LoginWithPasswordValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("用户名不能为空");
            RuleFor(x => x.Password).NotEmpty().WithMessage("密码不能为空");
        }
    }

    #endregion

    #region 密码登录 Response

    public class LoginWithPasswordResponse : BaseRespData
    {
        public long MemberId { get; set; }
        public string AccessToken { get; set; } 
    }

    #endregion

    #endregion
}