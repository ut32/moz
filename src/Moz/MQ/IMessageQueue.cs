namespace Moz.MessageQueue
{
    public interface IMessageQueue
    {
        void Add<T>(T message);
    }
}