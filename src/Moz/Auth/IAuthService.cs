using System;
using Moz.Bus.Dtos;
using Moz.Bus.Dtos.Auth;
using Moz.Bus.Models.Members;
using Moz.WebApi;

namespace Moz.Auth
{
    public interface IAuthService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ServResult<string> GetAuthenticatedUId();
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ServResult<SimpleMember> GetAuthenticatedMember();

        /// <summary>
        /// 用户名密码登录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ServResult<MemberLoginApo> LoginWithUsernamePassword(ServRequest<LoginWithUsernamePasswordDto> request);

        /// <summary>
        /// 三方授权登录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ServResult<MemberLoginApo> ExternalAuth(ServRequest<ExternalAuthDto> request); 
 
 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ServResult SetAuthCookie(ServRequest<SetAuthCookieDto> request);


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ServResult RemoveAuthCookie();

    }
}