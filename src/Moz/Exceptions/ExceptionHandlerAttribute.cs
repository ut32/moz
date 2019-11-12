using System;

namespace Moz.Exceptions
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ExceptionHandlerAttribute : Attribute
    {
        public ExceptionHandlerAttribute(Type exceptionHandlerType)
        {
            ExceptionHandlerType = exceptionHandlerType;
        }

        public Type ExceptionHandlerType { get; }
    }
}