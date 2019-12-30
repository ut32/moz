using System;

namespace Moz.Exceptions
{
    public class FatalException:MozException
    {
        public FatalException(string errorMessage) 
            : base(errorMessage, 10000)
        {
            
        }
    }
}