using System;

namespace Moz.Exceptions
{
    public class MozException : Exception
    {
        public MozException(string errorMessage)
            : this(errorMessage, 1000)
        {
        }

        public MozException(string errorMessage, Exception innerException)
            : this(errorMessage, 1000, innerException)
        {
        }

        public MozException(string errorMessage, int errorCode, Exception innerException = null)
            : base(errorMessage, innerException)
        {
            ErrorCode = errorCode;
        }

        public int ErrorCode { get; set; }
    }
}