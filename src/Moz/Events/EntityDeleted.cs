using Moz.Bus.Models;

namespace Moz.Events
{
    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityDeleted<T> where T : BaseModel
    {
        public EntityDeleted(T entity)
        {
            Entity = entity;
        }

        public T Entity { get; }
    }
}