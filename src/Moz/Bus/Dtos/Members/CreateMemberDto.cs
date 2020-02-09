using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Models.Members;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Bus.Dtos.Members
{
    /// <summary>
    /// tab_member
    /// </summary>
    [Validator(typeof(CreateMemberDtoValidator))]
    public class CreateMemberDto
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
        public string Mobile { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public GenderEnum? Gender { get; set; }
        
        
        /// <summary>
        /// 
        /// </summary>
        public string Nickname { get; set; }
        

        #endregion
    }

    public class CreateMemberApo
    {
        public long Id { get; set; }
        public string UId { get; set; }
        public string Username { get; set; }
    }

    public class CreateMemberDtoValidator : MozValidator<CreateMemberDto>
    {
        public CreateMemberDtoValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("用户名不能为空");
            RuleFor(x => x.Password).NotEmpty().WithMessage("密码不能为空");
            RuleFor(x => x.Password).MinimumLength(6).WithMessage("密码不能小于6位");
        }
    }
}
