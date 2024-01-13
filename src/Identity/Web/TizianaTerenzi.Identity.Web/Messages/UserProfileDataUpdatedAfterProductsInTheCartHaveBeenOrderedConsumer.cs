namespace TizianaTerenzi.Identity.Web.Messages
{
    using MassTransit;
    using TizianaTerenzi.Common.Messages.Carts;
    using TizianaTerenzi.Identity.Services.Data.Profile;

    public class UserProfileDataUpdatedAfterProductsInTheCartHaveBeenOrderedConsumer : IConsumer<UserProfileDataUpdatedAfterProductsInTheCartHaveBeenOrderedMessage>
    {
        private readonly IProfileService profileService;

        public UserProfileDataUpdatedAfterProductsInTheCartHaveBeenOrderedConsumer(IProfileService profileService)
        {
            this.profileService = profileService;
        }

        public async Task Consume(ConsumeContext<UserProfileDataUpdatedAfterProductsInTheCartHaveBeenOrderedMessage> context)
        {
            var message = context.Message;

            await this.profileService.SaveShippingDataAsync(message);

            await Task.CompletedTask;
        }
    }
}
