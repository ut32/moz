using System.Collections.Generic;
using Moz.Bus.Models.Members;
using Moz.CMS.Models.Members;
using Moz.Models.Members;

namespace Moz.Core.Service.Members
{
    public class PermissionComparer : IEqualityComparer<Permission>
    {
        public bool Equals(Permission x, Permission y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(Permission obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}