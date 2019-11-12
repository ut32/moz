using Moz.Bus.Models.Members;
using Moz.CMS.Models.Members;
using Moz.Core.Dtos.Members;
using Moz.Core.Service.Members;
using Moz.Models.Members;

namespace Moz.CMS.Services.Members
{
    public interface IMemberRegistrationService
    {
        /// <summary>
        /// </summary>
        /// <param name="usernameOrEmailOrMobile"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        MemberLoginResults ValidateMember(string usernameOrEmailOrMobile, string password);

        /// <summary>
        ///     Register customer
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Result</returns>
        MemberRegistrationResult RegisterMember(MemberRegistrationRequest request);

        /// <summary>
        ///     Change password
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Result</returns>
        //ChangePasswordResult ChangePassword(ChangePasswordRequest request);

        /// <summary>
        /// </summary>
        /// <param name="memeber"></param>
        /// <param name="newEmail"></param>
        /// <param name="requireValidation"></param>
        void SetEmail(Member memeber, string newEmail, bool requireValidation);

        /// <summary>
        ///     Sets a customer username
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="newUsername">New Username</param>
        void SetUsername(Member member, string newUsername);
        //ZoResponse<UpdateProfileResponse> UpdateProfile(ZoRequest<UpdateProfileRequest> request);
    }
}