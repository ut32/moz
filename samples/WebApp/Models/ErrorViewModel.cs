using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }

    public class DictionaryS<T, T1, T2> : Dictionary<int, string>
    {
        
    }

    public class Di : DictionaryS<int, string, string>
    {
        
    }
}