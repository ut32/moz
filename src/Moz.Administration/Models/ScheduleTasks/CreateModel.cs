using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography.X509Certificates;

namespace Moz.Administration.Models.ScheduleTasks
{
    public class CreateModel
    {
        public CreateModel()
        {
            
        }
        
        public List<dynamic> Types { get; set; } 
    }
}