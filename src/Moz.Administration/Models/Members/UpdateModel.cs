using System.Collections.Generic;
using Moz.Bus.Dtos.Members;
using Moz.Bus.Dtos.Members.Roles;
using Moz.Bus.Models.Members;

namespace Moz.Administration.Models.Members
{
    public class UpdateModel
    {
         public GetMemberDetailResponse Member { get; set; }
         public IList<QueryRoleItem> Roles { get; set; }
    }
    
    
}