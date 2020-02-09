using Microsoft.AspNetCore.Authorization;

namespace Moz.Auth.Handlers
{
    public class DefaultAuthorizationRequirement:IAuthorizationRequirement
    {
        public  string AdminOrMember { get; }

        public DefaultAuthorizationRequirement(string adminOrMember)
        {
            AdminOrMember = adminOrMember;
        }
    }
}