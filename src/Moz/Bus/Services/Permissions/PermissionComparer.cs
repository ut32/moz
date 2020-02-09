using System;
using System.Collections.Generic;
using Moz.Bus.Dtos.Permissions;
using Moz.Bus.Models.Members;

namespace Moz.Bus.Services.Permissions
{
    public class PermissionComparer:IEqualityComparer<Permission>
    {
        public bool Equals(Permission x, Permission y)
        {
            if (x == null || y == null) return false;
            return x.Id == y.Id;
        }

        public int GetHashCode(Permission obj)
        {
            return 0;
        }
    } 
}