namespace TizianaTerenzi.Administration.Services.Data.UserPenalties
{
    using MassTransit;
    using TizianaTerenzi.Common.Messages.Administration;

    public class UserPenaltiesService : IUserPenaltiesService
    {
        private readonly IBus publisher;

        public UserPenaltiesService(IBus publisher)
        {
            this.publisher = publisher;
        }

        public async Task BlockUserAsync(string userId, string reasonToBeBlocked)
        {
            await this.publisher.Publish(new UserBlockedMessage
            {
                UserId = userId,
                ReasonToBeBlocked = reasonToBeBlocked,
            });
        }

        public async Task UnblockUserAsync(string userId)
        {
            await this.publisher.Publish(new UserUnblockedMessage
            {
                UserId = userId,
            });
        }
    }
}
