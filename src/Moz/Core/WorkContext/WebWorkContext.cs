using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Moz.Auth;
using Moz.Bus.Models.Localization;
using Moz.Bus.Models.Members;
using Moz.Bus.Services.Localization;
using Moz.CMS.Model.Localization;
using Moz.CMS.Models.Members;

namespace Moz.Core.WorkContext
{
    public class WebWorkContext : IWorkContext
    {
        private readonly IAuthService _authenticationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDistributedCache _distributedCache;
        private readonly ILanguageService _languageService;

        private Language _cachedLanguage;
        //private SimpleMember _cacheMember;

        public WebWorkContext(IHttpContextAccessor httpContextAccessor,
            IAuthService authenticationService,
            IDistributedCache distributedCache,
            ILanguageService languageService)
        {
            _httpContextAccessor = httpContextAccessor;
            _authenticationService = authenticationService;
            _distributedCache = distributedCache;
            _languageService = languageService;
        }

        public SimpleMember CurrentMember
        {
            get
            {
                var memberId = _authenticationService.GetAuthenticatedUserId();
                if (memberId <= 0) return null;
                var member = _distributedCache.GetOrSet($"cache_member_{memberId}", () => _authenticationService.GetAuthenticatedMember());
                return member;
            }
        }
       

        public bool IsAdmin => throw new NotImplementedException();

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