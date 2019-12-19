using System;
using Moz.Bus.Dtos.Members;
using Moz.Bus.Models.Members;

namespace Moz.Administration.Models.Members
{
    public class SaveUpdateModel
    {
        
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
        
        
        public DateTime? BirthDay { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public long[] Roles { get; set; }
    }
}