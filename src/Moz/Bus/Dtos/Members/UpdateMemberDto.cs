using System;
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
    [Validator(typeof(UpdateMemberRequestValidator))]
    public class UpdateMemberRequest
    {
        #region 属性
        
        /// <summary>
        /// 
        /// </summary>
        public long Id { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string Nickname { get;set; }
        
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
        public string Mobile { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string Avatar { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public GenderEnum? Gender { get;set; } 
        
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
        public string Address { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string RegionCode { get;set; } 
        
         
        
        /// <summary>
        /// 
        /// </summary>
        public bool IsActive { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public bool IsDelete { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public bool IsEmailValid { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public bool IsMobileValid { get;set; } 
        
        
        /// <summary>
        /// 角色
        /// </summary>
        public long[] Roles { get; set; }
        
        #endregion     
    }
    
    
    public class UpdateMemberResponse
    {
    
    }
    
    
    public class UpdateMemberRequestValidator : MozValidator<UpdateMemberRequest>
    {
        public UpdateMemberRequestValidator(ILocalizationService localizationService)
        {

            RuleFor(x => x.Id).Must(t => true).WithMessage("发生错误");
            RuleFor(x => x.Username).NotEmpty().WithMessage("用户名不能为空");
            RuleFor(x => x.Password).NotEmpty().WithMessage("密码不能为空");
            RuleFor(x => x.Password).MinimumLength(6).WithMessage("密码不能小于6位");
            //RuleFor(x => x.CannotLoginUntilDate).Must(t => true).WithMessage("发生错误");
            //RuleFor(x => x.IsActive).Must(t => true).WithMessage("发生错误");
            // RuleFor(x => x.IsDelete).Must(t => true).WithMessage("发生错误");
            // RuleFor(x => x.IsEmailValid).Must(t => true).WithMessage("发生错误");
            //RuleFor(x => x.IsMobileValid).Must(t => true).WithMessage("发生错误");
            //RuleFor(x => x.Nickname).NotEmpty().WithMessage("不能为空");
        }
    }
    
}
