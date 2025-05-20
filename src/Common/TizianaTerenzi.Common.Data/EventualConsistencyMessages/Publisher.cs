namespace TizianaTerenzi.Common.Data.EventualConsistencyMessages
{
    using MassTransit;

    public class Publisher : IPublisher
    {
        private const int TimeoutMilliseconds = 2000;

        private readonly IBus bus;

        public Publisher(IBus bus)
        {
            this.bus = bus;
        }

        public async Task Publish<TMessage>(TMessage message)
            => await this.bus.Publish(message, GetCancellationToken());

        public async Task Publish<TMessage>(TMessage message, Type messageType)
            => await this.bus.Publish(message, messageType, GetCancellationToken());

        private static CancellationToken GetCancellationToken()
        {
            var timeout = TimeSpan.FromMilliseconds(TimeoutMilliseconds);
            var cancellationTokenSource = new CancellationTokenSource(timeout);

            return cancellationTokenSource.Token;
        }
    }
}
