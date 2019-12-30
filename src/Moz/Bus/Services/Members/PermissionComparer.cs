using System.Collections.Generic;
using Moz.Bus.Models.Members;

namespace Moz.Bus.Services.Members
{
    public class PermissionComparer : IEqualityComparer<Permission>
    {
        public bool Equals(Permission x, Permission y)
        {
            return x?.Id == y?.Id;
        }

        public int GetHashCode(Permission obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}