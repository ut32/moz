namespace Moz.Models.Members
{
    public enum MemberLoginResults
    {
        /// <summary>
        ///     Login successful
        /// </summary>
        Successful = 1,

        /// <summary>
        ///     Customer does not exist (email or username)
        /// </summary>
        MemberNotExist = 2,

        /// <summary>
        ///     Wrong password
        /// </summary>
        WrongPassword = 3,

        /// <summary>
        ///     Account have not been activated
        /// </summary>
        NotActive = 4,

        /// <summary>
        ///     Customer has been deleted
        /// </summary>
        Deleted = 5,

        /// <summary>
        ///     Locked out
        /// </summary>
        LockedOut = 6
    }
}