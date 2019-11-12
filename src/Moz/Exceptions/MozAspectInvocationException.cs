using System;
using AspectCore.DynamicProxy;

namespace Moz.Exceptions
{
    public class MozAspectInvocationException : AspectInvocationException
    {
        public MozAspectInvocationException(AspectContext aspectContext, Exception innerException, int errorCode = 666)
            : base(aspectContext, innerException)
        {
            ErrorMessage = innerException.Message;
            ErrorCode = errorCode;
        }

        public int ErrorCode { get; }
        public string ErrorMessage { get; }
    }
}