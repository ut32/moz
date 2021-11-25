using FluentValidation;
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
    [Validator(typeof(GetMemberDetailDtoValidator))]
    public class GetUserDetailDto
    {
        public long Id {get;set;}      
    } 
    
    
    public class GetUserDetailInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string UId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Username { get; set; }

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
        public Gender? Gender { get;set; } 
        
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
        public int? LoginCount { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string LastLoginIp { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? LastLoginDatetime { get;set; } 
        
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


        public long[] Roles { get; set; }
        
    }
    
    
    public class GetMemberDetailDtoValidator : MozValidator<GetUserDetailDto>
    {
        public GetMemberDetailDtoValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("参数错误");
        }
    }
    
}
