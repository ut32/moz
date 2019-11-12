
using System;
using System.Linq;
using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Biz.Dtos.Members
{
    /// <summary>
    /// tab_member
    /// </summary>
    [Validator(typeof(CreateMemberRequestValidator))]
    public class CreateMemberRequest
    {
        #region 属性
        
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
        public string PasswordSalt { get;set; } 
        
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
        public int? Gender { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? Birthday { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string RegisterIp { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime RegisterDatetime { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public int LoginCount { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string LastLoginIp { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime LastLoginDatetime { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? CannotLoginUntilDate { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime LastActiveDatetime { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public int FailedLoginAttempts { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public int OnlineTimeCount { get;set; } 
        
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
        public decimal? Lng { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public decimal? Lat { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string Geohash { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public sbyte IsActive { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public sbyte IsDelete { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public sbyte IsEmailValid { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public sbyte IsMobileValid { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string Nickname { get;set; } 
        
        #endregion     
    }
    
    
    public class CreateMemberResponse
    {
    
    }
    
    
    public class CreateMemberRequestValidator : MozValidator<CreateMemberRequest>
    {
        public CreateMemberRequestValidator(ILocalizationService localizationService)
        {
            
             RuleFor(x => x.Username).NotEmpty().WithMessage("不能为空");
            
            
             RuleFor(x => x.Password).NotEmpty().WithMessage("不能为空");
            
            
             RuleFor(x => x.PasswordSalt).NotEmpty().WithMessage("不能为空");
            
            
             RuleFor(x => x.Email).NotEmpty().WithMessage("不能为空");
            
            
             RuleFor(x => x.Mobile).NotEmpty().WithMessage("不能为空");
            
            
             RuleFor(x => x.Avatar).NotEmpty().WithMessage("不能为空");
            
            
             RuleFor(x => x.Gender).GreaterThan(0).WithMessage("不能为空");
            
            
            RuleFor(x => x.Birthday).Must(t=>true).WithMessage("发生错误");
            
            
             RuleFor(x => x.RegisterIp).NotEmpty().WithMessage("不能为空");
            
            
            RuleFor(x => x.RegisterDatetime).Must(t=>true).WithMessage("发生错误");
            
            
             RuleFor(x => x.LoginCount).GreaterThan(0).WithMessage("不能为空");
            
            
             RuleFor(x => x.LastLoginIp).NotEmpty().WithMessage("不能为空");
            
            
            RuleFor(x => x.LastLoginDatetime).Must(t=>true).WithMessage("发生错误");
            
            
            RuleFor(x => x.CannotLoginUntilDate).Must(t=>true).WithMessage("发生错误");
            
            
            RuleFor(x => x.LastActiveDatetime).Must(t=>true).WithMessage("发生错误");
            
            
             RuleFor(x => x.FailedLoginAttempts).GreaterThan(0).WithMessage("不能为空");
            
            
             RuleFor(x => x.OnlineTimeCount).GreaterThan(0).WithMessage("不能为空");
            
            
             RuleFor(x => x.Address).NotEmpty().WithMessage("不能为空");
            
            
             RuleFor(x => x.RegionCode).NotEmpty().WithMessage("不能为空");
            
            
             RuleFor(x => x.Lng).GreaterThan(0).WithMessage("不能为空");
            
            
             RuleFor(x => x.Lat).GreaterThan(0).WithMessage("不能为空");
            
            
             RuleFor(x => x.Geohash).NotEmpty().WithMessage("不能为空");
            
            
            RuleFor(x => x.IsActive).Must(t=>true).WithMessage("发生错误");
            
            
            RuleFor(x => x.IsDelete).Must(t=>true).WithMessage("发生错误");
            
            
            RuleFor(x => x.IsEmailValid).Must(t=>true).WithMessage("发生错误");
            
            
            RuleFor(x => x.IsMobileValid).Must(t=>true).WithMessage("发生错误");
            
            
             RuleFor(x => x.Nickname).NotEmpty().WithMessage("不能为空");
            
        }
    }
    
}
