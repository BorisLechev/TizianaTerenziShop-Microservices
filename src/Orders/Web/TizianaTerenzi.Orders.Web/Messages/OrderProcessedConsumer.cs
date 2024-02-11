namespace TizianaTerenzi.Orders.Web.Messages
{
    using MassTransit;
    using TizianaTerenzi.Common.Messages.Administration;
    using TizianaTerenzi.Orders.Services.Data.Orders;

    public class OrderProcessedConsumer : IConsumer<OrderProcessedMessage>
    {
        private readonly IOrdersService ordersService;

        public OrderProcessedConsumer(IOrdersService ordersService)
        {
            this.ordersService = ordersService;
        }

        public async Task Consume(ConsumeContext<OrderProcessedMessage> context)
        {
            var message = context.Message;

            var result = await this.ordersService.ProcessOrderAsync(message.OrderId);

            await Task.CompletedTask;
        }
    }
}
