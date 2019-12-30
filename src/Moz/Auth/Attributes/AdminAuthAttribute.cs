using System;

namespace Moz.Auth.Attributes
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)] 
    public class AdminAuthAttribute:MozAuthAttribute
    {
        public AdminAuthAttribute() : 
            base("admin_authorize")
        { 
        }
    }
}