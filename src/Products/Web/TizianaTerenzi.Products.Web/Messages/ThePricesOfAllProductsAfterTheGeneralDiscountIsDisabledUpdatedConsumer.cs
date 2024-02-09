namespace TizianaTerenzi.Products.Web.Messages
{
    using MassTransit;
    using TizianaTerenzi.Common.Messages.Administration;
    using TizianaTerenzi.Products.Services.Data.Products;

    public class ThePricesOfAllProductsAfterTheGeneralDiscountIsDisabledUpdatedConsumer : IConsumer<ThePricesOfAllProductsAfterTheGeneralDiscountIsDisabledUpdatedMessage>
    {
        private readonly IProductsService productsService;

        public ThePricesOfAllProductsAfterTheGeneralDiscountIsDisabledUpdatedConsumer(IProductsService productsService)
        {
            this.productsService = productsService;
        }

        public async Task Consume(ConsumeContext<ThePricesOfAllProductsAfterTheGeneralDiscountIsDisabledUpdatedMessage> context)
        {
            var result = await this.productsService.UpdateThePricesOfAllProductsAfterTheDiscountIsDisabledAsync();

            await Task.CompletedTask;
        }
    }
}
