namespace TizianaTerenzi.Products.Web.Messages
{
    using MassTransit;
    using TizianaTerenzi.Common.Messages.Administration;
    using TizianaTerenzi.Products.Services.Data.Products;

    public class ThePricesOfAllProductsAfterTheGeneralDiscountIsAppliedUpdatedConsumer : IConsumer<ThePricesOfAllProductsAfterTheGeneralDiscountIsAppliedUpdatedMessage>
    {
        private readonly IProductsService productsService;

        public ThePricesOfAllProductsAfterTheGeneralDiscountIsAppliedUpdatedConsumer(IProductsService productsService)
        {
            this.productsService = productsService;
        }

        public async Task Consume(ConsumeContext<ThePricesOfAllProductsAfterTheGeneralDiscountIsAppliedUpdatedMessage> context)
        {
            var message = context.Message;

            var result = await this.productsService.UpdateThePricesOfAllProductsAfterTheDiscountIsAppliedAsync(message.Discount);

            await Task.CompletedTask;
        }
    }
}
