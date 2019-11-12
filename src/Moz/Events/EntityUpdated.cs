using Moz.Bus.Models;
using Moz.CMS.Models;
using Moz.CMS.Model;
using Moz.CMS.Models;
using Moz.Domain.Models;

namespace Moz.Events
{
    public class EntityUpdated<T> where T : BaseModel
    {
        public EntityUpdated(T entity)
        {
            Entity = entity;
        }

        public T Entity { get; }
    }
}