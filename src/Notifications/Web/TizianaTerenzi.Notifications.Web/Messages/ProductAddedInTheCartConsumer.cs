namespace TizianaTerenzi.Notifications.Web.Messages
{
    using MassTransit;
    using Microsoft.AspNetCore.SignalR;
    using TizianaTerenzi.Common.Messages.Products;
    using TizianaTerenzi.Notifications.Web.Hubs;

    public class ProductAddedInTheCartConsumer : IConsumer<ProductAddedInTheCartMessage>
    {
        private readonly IHubContext<NumberOfProductsInTheUsersCartHub> hubContext;

        public ProductAddedInTheCartConsumer(IHubContext<NumberOfProductsInTheUsersCartHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        public async Task Consume(ConsumeContext<ProductAddedInTheCartMessage> context)
        {
            await this.hubContext
                .Clients
                .Users(context.Message.UserId)
                .SendAsync("AddProductInTheUsersCart");
        }
    }
}
