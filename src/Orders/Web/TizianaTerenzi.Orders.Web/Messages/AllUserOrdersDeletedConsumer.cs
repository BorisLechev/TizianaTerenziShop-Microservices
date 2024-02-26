namespace TizianaTerenzi.Orders.Web.Messages
{
    using MassTransit;
    using TizianaTerenzi.Common.Messages.Identity;
    using TizianaTerenzi.Orders.Services.Data.Orders;

    public class AllUserOrdersDeletedConsumer : IConsumer<AllUserOrdersDeletedMessage>
    {
        private readonly IOrdersService ordersService;

        public AllUserOrdersDeletedConsumer(IOrdersService ordersService)
        {
            this.ordersService = ordersService;
        }

        public async Task Consume(ConsumeContext<AllUserOrdersDeletedMessage> context)
        {
            var message = context.Message;

            var result = await this.ordersService.DeleteAllOrdersByUserIdAsync(message.UserId);

            await Task.CompletedTask;
        }
    }
}
