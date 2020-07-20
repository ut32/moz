using System;
using Moz.Bus.Dtos;
using Moz.Bus.Dtos.Auth;
using Moz.Bus.Dtos.Members;
using Moz.Bus.Models.Members;

namespace Moz.Auth
{
    public interface IAuthService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        PublicResult<string> GetAuthenticatedUId();
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        PublicResult<SimpleMember> GetAuthenticatedMember();

        /// <summary>
        /// 用户名密码登录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult<MemberLoginInfo> LoginWithUsernamePassword(LoginWithUsernamePasswordDto dto);

        /// <summary>
        /// 三方授权登录
        /// </summary>
        /// <param name="dto"></param> 
        /// <returns></returns>
        PublicResult<MemberLoginInfo> ExternalAuth(ExternalAuthDto dto); 
 
 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult SetAuthCookie(SetAuthCookieDto dto);


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        PublicResult RemoveAuthCookie();

    }
}