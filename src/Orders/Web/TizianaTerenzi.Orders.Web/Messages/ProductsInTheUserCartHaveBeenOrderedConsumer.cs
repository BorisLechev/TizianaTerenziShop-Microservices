namespace TizianaTerenzi.Orders.Web.Messages
{
    using MassTransit;
    using TizianaTerenzi.Common.Messages.Carts;
    using TizianaTerenzi.Orders.Services.Data.Orders;

    public class ProductsInTheUserCartHaveBeenOrderedConsumer : IConsumer<ProductsInTheUserCartHaveBeenOrderedMessage>
    {
        private readonly IOrdersService ordersService;

        public ProductsInTheUserCartHaveBeenOrderedConsumer(IOrdersService ordersService)
        {
            this.ordersService = ordersService;
        }

        public async Task Consume(ConsumeContext<ProductsInTheUserCartHaveBeenOrderedMessage> context)
        {
            var message = context.Message;

            await this.ordersService.OrderAsync(message);

            await Task.CompletedTask;
        }
    }
}
