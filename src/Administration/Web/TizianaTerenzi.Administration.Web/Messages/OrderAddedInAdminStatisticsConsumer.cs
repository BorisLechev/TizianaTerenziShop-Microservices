namespace TizianaTerenzi.Administration.Web.Messages
{
    using MassTransit;
    using TizianaTerenzi.Administration.Services.Data.Orders;
    using TizianaTerenzi.Common.Messages.Orders;

    public class OrderAddedInAdminStatisticsConsumer : IConsumer<OrderAddedInAdminStatisticsMessage>
    {
        private readonly IOrdersService ordersService;

        public OrderAddedInAdminStatisticsConsumer(IOrdersService ordersService)
        {
            this.ordersService = ordersService;
        }

        public async Task Consume(ConsumeContext<OrderAddedInAdminStatisticsMessage> context)
        {
            var message = context.Message;

            await this.ordersService.AddOrderStatisticsAsync(message);

            await Task.CompletedTask;
        }
    }
}
