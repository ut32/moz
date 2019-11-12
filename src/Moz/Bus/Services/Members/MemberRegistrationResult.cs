using System.Collections.Generic;
using System.Linq;

namespace Moz.Core.Service.Members
{
    public class MemberRegistrationResult
    {
        /// <summary>
        ///     Ctor
        /// </summary>
        public MemberRegistrationResult()
        {
            Errors = new List<string>();
        }

        /// <summary>
        ///     Gets a value indicating whether request has been completed successfully
        /// </summary>
        public bool Success => !Errors.Any();

        /// <summary>
        ///     Errors
        /// </summary>
        public IList<string> Errors { get; }

        /// <summary>
        ///     Add error
        /// </summary>
        /// <param name="error">Error</param>
        public void AddError(string error)
        {
            Errors.Add(error);
        }
    }
}