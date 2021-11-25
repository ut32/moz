using System;
using Moz.Bus.Dtos;
using Moz.Bus.Dtos.Auth;
using Moz.Bus.Dtos.Members;
using Moz.Bus.Models.Members;
using Moz.Model;

namespace Moz.Auth
{
    public interface IAuthService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //PublicResult<string> GetAuthenticatedUId();
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        PublicResult<SimpleUser> GetAuthenticatedMember();
 
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