using Moz.Bus.Models;
using Moz.CMS.Models;
using Moz.CMS.Model;
using Moz.CMS.Models;
using Moz.Domain.Models;

namespace Moz.Events
{
    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntitiesDeleted<T> where T : BaseModel
    {
        public EntitiesDeleted(long[] ids)
        {
            Ids = ids;
        } 

        public long[] Ids { get; }
    }
}