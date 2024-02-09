namespace TizianaTerenzi.Carts.Web.Messages
{
    using MassTransit;
    using TizianaTerenzi.Carts.Services.Data.GeneralDiscounts;
    using TizianaTerenzi.Common.Messages.Administration;

    public class ThePricesOfAllProductsInTheCartAfterTheGeneralDiscountIsAppliedUpdatedConsumer : IConsumer<ThePricesOfAllProductsInTheCartAfterTheGeneralDiscountIsAppliedUpdatedMessage>
    {
        private readonly IGeneralDiscountsService generalDiscountsService;

        public ThePricesOfAllProductsInTheCartAfterTheGeneralDiscountIsAppliedUpdatedConsumer(IGeneralDiscountsService generalDiscountsService)
        {
            this.generalDiscountsService = generalDiscountsService;
        }

        public async Task Consume(ConsumeContext<ThePricesOfAllProductsInTheCartAfterTheGeneralDiscountIsAppliedUpdatedMessage> context)
        {
            var message = context.Message;

            var result = await this.generalDiscountsService.ModifyThePricesAfterAppliedGeneralDiscountAsync(message.Discount);

            await Task.CompletedTask;
        }
    }
}
