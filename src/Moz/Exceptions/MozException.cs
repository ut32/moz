using System;

namespace Moz.Exceptions
{
    public class MozException : Exception
    {
        public MozException(string errorMessage, Exception innerException)
            : this(errorMessage, 600, innerException)
        {
        }

        public MozException(string errorMessage, int errorCode = 600, Exception innerException = null)
            : base(errorMessage, innerException)
        {
            ErrorCode = errorCode;
        }

        public int ErrorCode { get; set; }
    }
}