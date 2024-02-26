namespace TizianaTerenzi.Orders.Web.Messages
{
    using MassTransit;
    using TizianaTerenzi.Common.Messages.Identity;
    using TizianaTerenzi.Orders.Services.Data.Orders;

    public class AllUserOrderProductsDeletedConsumer : IConsumer<AllUserOrderProductsDeletedMessage>
    {
        private readonly IOrdersService ordersService;

        public AllUserOrderProductsDeletedConsumer(IOrdersService ordersService)
        {
            this.ordersService = ordersService;
        }

        public async Task Consume(ConsumeContext<AllUserOrderProductsDeletedMessage> context)
        {
            var message = context.Message;

            await this.ordersService.DeleteAllOrderProductsByUserIdAsync(message.UserId);

            await Task.CompletedTask;
        }
    }
}
