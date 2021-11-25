﻿using System;
using FluentValidation.Attributes;
using Moz.Bus.Models.Members;
using Moz.Bus.Services.Localization;
using Moz.Model;
using Moz.Validation;

namespace Moz.Dto.User
{
    /// <summary>
    /// tab_member
    /// </summary>
    [Validator(typeof(PagedQueryMemberDtoValidator))]
    public class PagedQueryUserDto
    {
        #region 属性
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public string Keyword{ get;set; }
        #endregion     
    }

    public class QueryUserItem
    {
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
        public string Email { get;set; } 
 
        /// <summary>
        /// 
        /// </summary>
        public string Avatar { get;set; }

        /// <summary>
        /// 
        /// </summary>
        public string AvatarFullPath => Avatar?.GetFullPath();
        /// <summary>
        /// 
        /// </summary>
        public Gender Gender { get;set; } 
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
        public string RegisterDatetimeString => RegisterDatetime.ToString("yyyy/MM/dd HH:mm");
        
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
        /// 
        /// </summary>
        public string Nickname { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string[] Roles { get; set; }
    }
    
    public class PagedQueryMemberDtoValidator : MozValidator<PagedQueryUserDto>
    {
        public PagedQueryMemberDtoValidator(ILocalizationService localizationService)
        {
            
        }
    }
    
}
