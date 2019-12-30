using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moz.Auth.Attributes;
using Moz.Bus.Models.Members;
using Moz.CMS.Models.Members;
using Moz.Core;

namespace Moz.Auth.Handlers
{
    internal class AdminAuthorizationHandler : AuthorizationHandler<AdminAuthorizationHandler>, IAuthorizationRequirement
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,AdminAuthorizationHandler requirement)
        {
            if (!(context.Resource is Endpoint endpoint))
            {
                context.Fail();
                return Task.CompletedTask;
            }

            //获取所有permission标签
            var attributes = endpoint.Metadata.GetOrderedMetadata<AdminAuthAttribute>();
            if (!attributes.Any())
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            //获取当前用户
            var workContext = EngineContext.Current.Resolve<IWorkContext>();
            var member = workContext.CurrentMember;
            if (member == null)
            {
                context.Fail();
                return Task.CompletedTask;
            }
            if (!member.IsAdmin())
            {
                context.Fail();
                return Task.CompletedTask;
            }

            //检查权限
            var isRoleAuth = CheckRole(member, attributes);
            var isPermissionAuth = CheckPermission(member, attributes);
            if (isRoleAuth && isPermissionAuth)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            context.Fail();
            return Task.CompletedTask;
        }

        /// <summary>
        /// </summary>
        /// <param name="member"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        private bool CheckRole(SimpleMember member, IReadOnlyCollection<AdminAuthAttribute> attributes)
        {
            var allRoles = attributes.Where(t => !t.Roles.IsNullOrEmpty()).SelectMany(t =>
            {
                var rolesAry = new[] {t.Roles};
                if (t.Roles.Contains(",")) rolesAry = t.Roles.Split(",");
                return rolesAry;
            }).ToList();
            return !allRoles.Any() || allRoles.Any(member.InRole);
        }

        private bool CheckPermission(SimpleMember member, IEnumerable<AdminAuthAttribute> attributes)
        {
            var allPermissions = attributes
                .Where(t => !t.Permissions.IsNullOrEmpty())
                .SelectMany(t =>
                {
                    var permissionNameAry = new[] {t.Permissions};
                    if (t.Permissions.Contains(",")) permissionNameAry = t.Permissions.Split(",");
                    return permissionNameAry;
                }).ToList();
            return allPermissions.All(member.HasPermission);
        }
    }
}