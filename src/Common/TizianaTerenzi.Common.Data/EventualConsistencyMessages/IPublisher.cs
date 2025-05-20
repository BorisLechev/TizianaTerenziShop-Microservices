namespace TizianaTerenzi.Common.Data.EventualConsistencyMessages
{
    public interface IPublisher
    {
        Task Publish<TMessage>(TMessage message);

        Task Publish<TMessage>(TMessage message, Type messageType);
    }
}
