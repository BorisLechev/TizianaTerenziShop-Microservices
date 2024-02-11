namespace TizianaTerenzi.Carts.Web.Messages
{
    using MassTransit;
    using TizianaTerenzi.Carts.Services.Data.Discounts;
    using TizianaTerenzi.Common.Messages.Administration;

    public class DiscountCodeCreatedConsumer : IConsumer<DiscountCodeCreatedMessage>
    {
        private readonly IDiscountCodesService discountCodesService;

        public DiscountCodeCreatedConsumer(IDiscountCodesService discountCodesService)
        {
            this.discountCodesService = discountCodesService;
        }

        public async Task Consume(ConsumeContext<DiscountCodeCreatedMessage> context)
        {
            var message = context.Message;

            var result = await this.discountCodesService.CreateDiscountCodeAsync(message);

            await Task.CompletedTask;
        }
    }
}
