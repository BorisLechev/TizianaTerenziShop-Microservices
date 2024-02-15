namespace TizianaTerenzi.Identity.Web.Messages
{
    using MassTransit;
    using TizianaTerenzi.Common.Messages.Administration;
    using TizianaTerenzi.Identity.Services.Data.UserPenalties;

    public class UserBlockedConsumer : IConsumer<UserBlockedMessage>
    {
        private readonly IUserPenaltiesService userPenaltiesService;

        public UserBlockedConsumer(IUserPenaltiesService userPenaltiesService)
        {
            this.userPenaltiesService = userPenaltiesService;
        }

        public async Task Consume(ConsumeContext<UserBlockedMessage> context)
        {
            var message = context.Message;

            var result = await this.userPenaltiesService.BlockUserAsync(message);

            await Task.CompletedTask;
        }
    }
}
