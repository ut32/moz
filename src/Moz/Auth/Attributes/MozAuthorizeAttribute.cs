using System;
using Microsoft.AspNetCore.Authorization;

namespace Moz.Auth.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public abstract class MozAuthAttribute : AuthorizeAttribute
    {
        public const string MozAuthorizeSchemes = "MozAuthorizeSchemes";

        protected MozAuthAttribute(string policy)
            :base(policy)
        {
            AuthenticationSchemes = MozAuthorizeSchemes;
        }

        public string Permissions { get; set; }
    }
}