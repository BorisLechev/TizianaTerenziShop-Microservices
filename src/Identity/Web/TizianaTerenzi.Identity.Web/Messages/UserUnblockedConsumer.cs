namespace TizianaTerenzi.Identity.Web.Messages
{
    using MassTransit;
    using TizianaTerenzi.Common.Messages.Administration;
    using TizianaTerenzi.Identity.Services.Data.UserPenalties;

    public class UserUnblockedConsumer : IConsumer<UserUnblockedMessage>
    {
        private readonly IUserPenaltiesService userPenaltiesService;

        public UserUnblockedConsumer(IUserPenaltiesService userPenaltiesService)
        {
            this.userPenaltiesService = userPenaltiesService;
        }

        public async Task Consume(ConsumeContext<UserUnblockedMessage> context)
        {
            var message = context.Message;

            var result = await this.userPenaltiesService.UnblockUserAsync(message);

            await Task.CompletedTask;
        }
    }
}
