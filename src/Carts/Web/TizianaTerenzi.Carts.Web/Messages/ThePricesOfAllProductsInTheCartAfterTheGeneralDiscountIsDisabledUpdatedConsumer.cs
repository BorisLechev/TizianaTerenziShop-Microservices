namespace TizianaTerenzi.Carts.Web.Messages
{
    using MassTransit;
    using TizianaTerenzi.Carts.Services.Data.GeneralDiscounts;
    using TizianaTerenzi.Common.Messages.Administration;

    public class ThePricesOfAllProductsInTheCartAfterTheGeneralDiscountIsDisabledUpdatedConsumer : IConsumer<ThePricesOfAllProductsInTheCartAfterTheGeneralDiscountIsDisabledUpdatedMessage>
    {
        private readonly IGeneralDiscountsService generalDiscountsService;

        public ThePricesOfAllProductsInTheCartAfterTheGeneralDiscountIsDisabledUpdatedConsumer(IGeneralDiscountsService generalDiscountsService)
        {
            this.generalDiscountsService = generalDiscountsService;
        }

        public async Task Consume(ConsumeContext<ThePricesOfAllProductsInTheCartAfterTheGeneralDiscountIsDisabledUpdatedMessage> context)
        {
            var message = context.Message;

            var result = await this.generalDiscountsService.ModifyThePricesAfterDisabledGeneralDiscountAsync(message.Discount);

            await Task.CompletedTask;
        }
    }
}
