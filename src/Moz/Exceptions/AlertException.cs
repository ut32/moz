using System;

namespace Moz.Exceptions
{
    public class AlertException:MozException
    {
        public AlertException(string errorMessage) 
            : base(errorMessage, 600)
        {
            
        }
    }
}