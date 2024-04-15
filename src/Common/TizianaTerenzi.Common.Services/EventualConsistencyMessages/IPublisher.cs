namespace TizianaTerenzi.Common.Services.EventualConsistencyMessages
{
    public interface IPublisher
    {
        Task Publish<TMessage>(TMessage message);

        Task Publish<TMessage>(TMessage message, Type messageType);
    }
}
