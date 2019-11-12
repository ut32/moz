using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Moz.CMS.Models.Members;

namespace Moz.Bus.Models.Members
{
    [DataContract]
    public class SimpleMember
    {
        [DataMember]
        public long Id { get; internal set; }
        
        [DataMember]
        public string UId { get; internal set; }
        
        [DataMember]
        public string Email { get; internal set; } 
        
        [DataMember]
        public string Username { get; internal set; }
        
        [DataMember]
        public string Nickname { get;internal set; }
        
        [DataMember]
        public string Mobile { get;internal set; }
        
        [DataMember]
        public string Avatar { get;internal set; }
        
        [DataMember]
        public bool IsActive { get; internal set; }
        
        [DataMember]
        public bool IsDelete { get; internal set; }
        
        [DataMember]
        public DateTime? CannotLoginUntilDate { get; internal set; }
        
        [DataMember]
        public IEnumerable<Role> Roles { get; internal set; }
        
        [DataMember]
        public IEnumerable<Permission> Permissions { get;internal set; }

        public bool InRole(string roleCode)
        {
            return Roles.Any(t => t.Code.Equals(roleCode, StringComparison.OrdinalIgnoreCase));
        }

        public bool IsAdmin()
        {
            return Roles?.Any(t => t.IsAdmin) ?? false;
        }

        public bool HasPermission(string permissionCode)
        {
            return Permissions.Any(t => t.Code.Equals(permissionCode, StringComparison.OrdinalIgnoreCase));
        }
    }
}