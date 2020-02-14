using System;

namespace Moz.Exceptions
{
    public abstract class MozException : Exception
    {
        protected MozException(string errorMessage, Exception innerException)
            : this(errorMessage, 600, innerException)
        {
        }

        protected MozException(string errorMessage, int errorCode = 600, Exception innerException = null)
            : base(errorMessage, innerException)
        {
            ErrorCode = errorCode;
        }

        public int ErrorCode { get; set; }
    }
}