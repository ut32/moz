using Moz.Bus.Models.Members;

namespace Moz.Core.Service.Members
{
    public class MemberRegistrationRequest
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Moz.Core.Service.Members.MemberRegistrationRequest" /> class.
        /// </summary>
        /// <param name="member">Member.</param>
        /// <param name="email">Email.</param>
        /// <param name="username">Username.</param>
        /// <param name="password">Password.</param>
        public MemberRegistrationRequest(Member member, string email, string username, string password,
            bool isFromThirdParty = false)
        {
            Member = member;
            Email = email;
            Username = username;
            Password = password;
            IsFromThirdParty = isFromThirdParty;
        }

        /// <summary>
        ///     Customer
        /// </summary>
        public Member Member { get; }

        /// <summary>
        ///     Email
        /// </summary>
        public string Email { get; }

        /// <summary>
        ///     Username
        /// </summary>
        public string Username { get; }

        /// <summary>
        ///     Password
        /// </summary>
        public string Password { get; }

        /// <summary>
        ///     Gets or sets a value indicating whether this
        ///     <see cref="T:Moz.Core.Service.Members.MemberRegistrationRequest" /> is from third party.
        /// </summary>
        /// <value><c>true</c> if is from third party; otherwise, <c>false</c>.</value>
        public bool IsFromThirdParty { get; }
    }
}