using Moz.Bus.Models;

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