using Moz.Bus.Dtos;
using Moz.Bus.Dtos.Members;

namespace Moz.Bus.Services.Members
{
    public interface IRegistrationService
    {
        /// <summary>
        /// 三方平台注册
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult<RegistrationInfo> Register(ExternalRegistrationDto dto);

        /// <summary>
        /// 用户名注册
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult<RegistrationInfo> Register(UsernameRegistrationDto dto);   
    }
}