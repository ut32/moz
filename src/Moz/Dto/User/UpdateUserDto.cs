using System;
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
    [Validator(typeof(UpdateUserDtoValidator))]
    public class UpdateUserDto
    {
        #region 属性
        
        /// <summary>
        /// 
        /// </summary>
        public long Id { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string Username { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string Password { get;set; }

        /// <summary>
        /// 
        /// </summary>
        public string Email { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string Avatar { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public Gender? Gender { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public DateTime? BirthDay { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? CannotLoginUntilDate { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public bool IsActive { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public bool IsDelete { get;set; } 
        
        /// <summary>
        /// 角色
        /// </summary>
        public long[] Roles { get; set; }
        
        #endregion     
    }

    public class UpdateUserDtoValidator : MozValidator<UpdateUserDto>
    {
        public UpdateUserDtoValidator(ILocalizationService localizationService)
        {

            RuleFor(x => x.Id).Must(t => true).WithMessage("发生错误");
            RuleFor(x => x.Username).NotEmpty().WithMessage("用户名不能为空");
            RuleFor(x => x.Password).NotEmpty().WithMessage("密码不能为空");
            RuleFor(x => x.Password).MinimumLength(6).WithMessage("密码不能小于6位");
        }
    }
    
}
