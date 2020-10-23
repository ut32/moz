using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Bus.Dtos.Members
{
    /// <summary>
    /// 修改头像
    /// </summary>
    [Validator(typeof(UpdateAvatarDtoValidator))]
    public class UpdateAvatarDto
    {
        public long MemberId { get; set; } 
        public string Avatar { get; set; }
    }
    
    /// <summary>
    /// 
    /// </summary>
    public class UpdateAvatarDtoValidator : MozValidator<UpdateAvatarDto>
    {
        public UpdateAvatarDtoValidator(ILocalizationService localizationService)
        {
            RuleFor(it => it.MemberId).GreaterThan(0).WithMessage("参数错误");
            RuleFor(it => it.Avatar).NotEmpty().WithMessage("头像不能为空");
        }
    }
}