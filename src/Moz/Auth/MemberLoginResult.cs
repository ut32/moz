using System;
using System.Collections.Generic;
using System.Linq;
using Moz.Bus.Models.Members;

namespace Moz.Auth
{
    public class MemberLoginResult
    {
        public MemberLoginResult()
        {
            Errors =new List<string>();
        }

        public IList<string> Errors { get;}
        
        public bool IsError =>  Errors.Any();

        public void AddError(string error)
        {
            Errors.Add(error);
        }
        public string AccessToken { get; set; }
    }
}