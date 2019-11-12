using System;
using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Models.Members;
using Moz.Validation;

namespace Moz.Bus.Dtos.Auth
{
    [Validator(typeof(ExternalAuthValidator))]
    public class ExternalAuthRequest 
    {
        /// <summary>
        /// Access Token
        /// </summary>
        public string AccessToken { get; set; }
        
        /// <summary>
        /// Refresh Token
        /// </summary>
        public string RefreshToken { get; set; }
        
        /// <summary>
        /// Expire Date
        /// </summary>
        public DateTime ExpireDt { get; set; }
        
        /// <summary>
        /// OpenId
        /// </summary>
        public string OpenId { get; set; }
        
        /// <summary>
        /// 平台
        /// </summary>
        public ExternalAuthProvider Provider { get; set; }
        
        /// <summary>
        /// 用户信息
        /// </summary>
        public ExternalAuthUserInfo UserInfo { get; set; }
    }

    public class ExternalAuthUserInfo
    {
        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; }
        
        /// <summary>
        /// 昵称
        /// </summary>
        public string Nickname { get; set; }
        
        /// <summary>
        /// 邮件地址
        /// </summary>
        public string Email { get; set; }
        
        /// <summary>
        /// 性别 1:男 0:女
        /// </summary>
        public int Gender { get; set; }
    }

    public class ExternalAuthValidator : MozValidator<ExternalAuthRequest>
    {
        public ExternalAuthValidator()
        {
            RuleFor(x => x.OpenId).NotEmpty().WithMessage("openid不能为空");
        }
    }

    public class ExternalAuthResponse:BaseRespData
    {
        public long MemberId { get; set; }
        public string Token { get; set; } 
    }
}