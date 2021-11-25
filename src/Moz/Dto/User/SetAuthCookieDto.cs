using FluentValidation;
using FluentValidation.Attributes;
using Moz.Validation;

namespace Moz.Dto.User
{

    [Validator(typeof(SetAuthCookieDtoValidator))]
    public class SetAuthCookieDto
    {
        public string Token { get; set; }
    }

    public class SetAuthCookieDtoValidator : MozValidator<SetAuthCookieDto>
    {
        public SetAuthCookieDtoValidator()
        {
            RuleFor(t => t.Token).NotEmpty().WithMessage("参数错误");
        }
    }
}