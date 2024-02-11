namespace TizianaTerenzi.Carts.Web.Messages
{
    using MassTransit;
    using TizianaTerenzi.Carts.Services.Data.Discounts;
    using TizianaTerenzi.Common.Messages.Administration;

    public class DiscountCodeDeletedConsumer : IConsumer<DiscountCodeDeletedMessage>
    {
        private readonly IDiscountCodesService discountCodesService;

        public DiscountCodeDeletedConsumer(IDiscountCodesService discountCodesService)
        {
            this.discountCodesService = discountCodesService;
        }

        public async Task Consume(ConsumeContext<DiscountCodeDeletedMessage> context)
        {
            var message = context.Message;

            var result = await this.discountCodesService.DeleteDiscountCodeAsync(message);

            await Task.CompletedTask;
        }
    }
}
