namespace Moz.Events
{
    public interface ISubscriber<in T>
    {
        void HandleEvent(T eventMessage);
    }
}