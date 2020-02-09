using System.Collections.Generic;
using Moz.Bus.Dtos.Members;
using Moz.Bus.Dtos.Roles;

namespace Moz.Admin.Layui.Models.Members
{
    public class UpdateModel
    {
         public GetMemberDetailApo Member { get; set; }
         public IList<QueryRoleItem> Roles { get; set; }
    }
    
    
}