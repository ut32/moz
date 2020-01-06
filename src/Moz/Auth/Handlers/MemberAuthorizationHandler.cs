using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Moz.Auth.Attributes;
using Moz.Bus.Models.Members;
using Moz.Core;

namespace Moz.Auth.Handlers
{
    internal class MemberAuthorizationHandler : AuthorizationHandler<MemberAuthorizationHandler>, IAuthorizationRequirement
    {
        private static readonly ConcurrentDictionary<string, List<MozAuthAttribute>> AllPermissions = new ConcurrentDictionary<string, List<MozAuthAttribute>>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            MemberAuthorizationHandler requirement)
        {
            
            //获取所有permission标签
            if (!((context.Resource as AuthorizationFilterContext)?.ActionDescriptor is ControllerActionDescriptor
                action))
            {
                context.Fail();
                return Task.CompletedTask;
            }

            List<MozAuthAttribute> attributes = null;
            if (AllPermissions.ContainsKey(action.DisplayName))
            {
                attributes = AllPermissions[action.DisplayName];
            }
            else
            {
                attributes = new List<MozAuthAttribute>();
                //从类上读取                                                           
                var types = new List<Type> {action.ControllerTypeInfo.UnderlyingSystemType};
                GetAllTypes(action.ControllerTypeInfo.UnderlyingSystemType, types);
                foreach (var type in types) attributes.AddRange(GetAttributes(type));
                //从方法上读取
                attributes.AddRange(GetAttributes(action.MethodInfo));
                AllPermissions[action.DisplayName] = attributes;
            }

            if (attributes == null || !attributes.Any())
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
        private bool CheckRole(SimpleMember member, IReadOnlyCollection<MozAuthAttribute> attributes)
        {
            var allRoles = attributes.Where(t => !t.Roles.IsNullOrEmpty()).SelectMany(t =>
            {
                var rolesAry = new[] {t.Roles};
                if (t.Roles.Contains(",")) rolesAry = t.Roles.Split(",");
                return rolesAry;
            }).ToList();
            return !allRoles.Any() || allRoles.Any(member.InRole);
        }

        private bool CheckPermission(SimpleMember member, IReadOnlyCollection<MozAuthAttribute> attributes)
        {
            var allPermissions = attributes.Where(t => !t.Permissions.IsNullOrEmpty()).SelectMany(t =>
            {
                var permissionNameAry = new[] {t.Permissions};
                if (t.Permissions.Contains(",")) permissionNameAry = t.Permissions.Split(",");
                return permissionNameAry;
            }).ToList();
            return allPermissions.All(member.HasPermission);
        }

        /// <summary>
        /// </summary>
        /// <param name="type"></param>
        /// <param name="list"></param>
        private void GetAllTypes(Type type, List<Type> list)
        {
            var baseType = type.BaseType;
            if (baseType != null)
            {
                list.Add(baseType);
                GetAllTypes(baseType, list);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        private static IEnumerable<MozAuthAttribute> GetAttributes(MemberInfo memberInfo)
        {
            return memberInfo.GetCustomAttributes(typeof(MozAuthAttribute), false).Cast<MozAuthAttribute>();
        }
    }
}