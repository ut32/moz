using System;
using System.Collections.Generic;
using System.Linq;
using AspectCore.DynamicProxy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Moz.Auth;
using Moz.Bus.Models.Localization;
using Moz.Bus.Models.Members;
using Moz.Bus.Services.Localization;
using Moz.Bus.Services.Permissions;
using Moz.Bus.Services.Roles;

namespace Moz.Core.WorkContext
{
    public class WebWorkContext : IWorkContext
    {
        private readonly IAuthService _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDistributedCache _distributedCache;
        private readonly ILanguageService _languageService;
        private readonly IRoleService _roleService;
        private readonly IPermissionService _permissionService;

        private Language _cachedLanguage;
        //private SimpleMember _cacheMember;

        public WebWorkContext(IHttpContextAccessor httpContextAccessor,
            IAuthService authService,
            IDistributedCache distributedCache,
            ILanguageService languageService,
            IRoleService roleService, 
            IPermissionService permissionService)
        {
            _httpContextAccessor = httpContextAccessor;
            _authService = authService;
            _distributedCache = distributedCache;
            _languageService = languageService;
            _roleService = roleService;
            _permissionService = permissionService;
        }

        public SimpleMember CurrentMember
        {
            get
            {
                var result = _authService.GetAuthenticatedUId();
                if (result.Code > 0)
                    return null;

                var authenticatedMemberResult = _distributedCache.GetOrSet($"CACHE_MEMBER_{result.Data}",
                    () => _authService.GetAuthenticatedMember());
                if (authenticatedMemberResult.Code > 0)
                    return null;

                var authenticatedMember = authenticatedMemberResult.Data;

                //角色
                var getAvailableRolesResult = _roleService.GetAvailableRoles();
                if (getAvailableRolesResult.Code == 0 && getAvailableRolesResult.Data.Any())
                {
                    var myRoleCodes = authenticatedMember
                        .Roles
                        .Select(it => it.Code)
                        .ToList();

                    authenticatedMember.Roles = getAvailableRolesResult.Data
                        .Where(it => it.IsActive && myRoleCodes.Contains(it.Code,StringComparer.OrdinalIgnoreCase))
                        .ToList();
                }
                else
                {
                    authenticatedMember.Roles = new List<Role>();
                }

                //权限
                var availablePermissionsResult = _permissionService.GetAvailablePermissions();
                if (availablePermissionsResult.Code == 0 && availablePermissionsResult.Data.Any())
                {
                    var availableRolesCodes = authenticatedMember
                        .Roles
                        .Select(it => it.Code)
                        .ToList();
                    var availablePermissions = availablePermissionsResult.Data;
                    authenticatedMember.Permissions = availablePermissions
                        .Where(it => availableRolesCodes.Contains(it.RoleCode, StringComparer.OrdinalIgnoreCase))
                        .Select(it => new Permission
                        {
                            Code = it.Code,
                            Id = it.Id,
                            Name = it.Name
                        })
                        .Distinct(new PermissionComparer())
                        .ToList();
                }
                
                return authenticatedMember;
            }
        }



        #region Language

        private string GetStandardLanguageString(string language)
        {
            if (string.IsNullOrEmpty(language))
                return string.Empty;
            language = language.ToLower();
            if (language.Contains("zh"))
                return "zh-cn";
            if (language.Contains("en"))
                return "en-us";
            return string.Empty;
        }

        public Language WorkingLanguage
        {
            get
            {
                if (_cachedLanguage != null)
                    return _cachedLanguage;

                string language = _httpContextAccessor.HttpContext.Request.Query["lang"];
                if (language.IsNullOrEmpty()) language = _httpContextAccessor.HttpContext.Request.Cookies["lang"];
                if (language.IsNullOrEmpty())
                    language = _httpContextAccessor.HttpContext.Request.Headers["Accept-Language"];
                if (language.IsNullOrEmpty()) language = "en";
                language = GetStandardLanguageString(language);

                var languageEntity = _languageService
                    .GetAllLanguages()
                    .FirstOrDefault(l =>
                        language.Equals(l.LanguageCulture, StringComparison.InvariantCultureIgnoreCase)
                        && l.Published);
                if (languageEntity == null)
                    languageEntity = _languageService.GetAllLanguages().FirstOrDefault(l => l.Published);

                if (languageEntity == null) languageEntity = _languageService.GetAllLanguages().FirstOrDefault();

                _cachedLanguage = languageEntity;
                return _cachedLanguage;
            }
        }

        #endregion
    }
}